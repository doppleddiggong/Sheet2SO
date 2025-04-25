using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

namespace DoppleLittleHelper
{    
    public class BaseSheetDownloader<TConfig> : EditorWindow where TConfig : BaseSheetConfigSO
    {
        protected TConfig configSO;
        protected bool isProcessing = false;
        protected bool isCancelled = false;
        protected DateTime createTime;
        protected float curProgress = 0f;
        protected string curStatus = "";
        protected Queue<SheetData> dataQueue;

        protected virtual void OnEnable()
        {
            LoadConfig();
        }

        protected virtual void LoadConfig()
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(TConfig).Name}");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                configSO = AssetDatabase.LoadAssetAtPath<TConfig>(path);
            }
            else
            {
                Debug.LogError($"[SHEET2SO] {typeof(TConfig).Name} Not Found");
            }
        }

        private void OnGUI()
        {
            if (configSO == null)
            {
                EditorGUILayout.HelpBox("[SHEET2SO] BaseSheetConfigSO Not Found", MessageType.Error);
                return;
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Base Sheet Downloader", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);

            EditorGUI.BeginChangeCheck();
            configSO = (TConfig)EditorGUILayout.ObjectField("Sheet Config", configSO, typeof(TConfig), false);

            if (configSO.SheetList.Count == 0)
            {
                EditorGUILayout.HelpBox("[SHEET2SO] MasterSheet Data is not set in SheetConfigSO", MessageType.Warning);
                return;
            }

            OnGUI_Data();

            foreach (var item in configSO.SheetList)
            {
                if (GUILayout.Button($"Download {item.sheetName}"))
                {
                    StartSingleDataDownload(item);
                }
            }
        }

        protected virtual void OnGUI_Data()
        {
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(configSO);
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.Space(10);

            if (isProcessing)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Progress", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                Rect progressRect = EditorGUILayout.GetControlRect();
                EditorGUI.ProgressBar(progressRect, curProgress, $"{curStatus} ({curProgress * 100:F1}%)");

                EditorGUILayout.Space(5);
                if (GUILayout.Button("Canceled"))
                {
                    isCancelled = true;
                }
                EditorGUILayout.EndVertical();
                return;
            }

            if (GUILayout.Button("Download All Data", GUILayout.Height(30)))
            {
                DownloadAllData();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Select SheetData Download", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            if (isProcessing)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("Downloading...", EditorStyles.boldLabel);
                if (GUILayout.Button("Cancel"))
                {
                    isCancelled = true;
                }
                EditorGUILayout.EndVertical();
            }
        }

        protected virtual void InitializeGeneration()
        {
            Debug.Log("[SHEET2SO] InitializeGeneration START");
            createTime = DateTime.Now;
            isProcessing = true;
            isCancelled = false;
            curProgress = 0f;
            curStatus = "Initializing...";

            configSO.ClearSOData();
            Debug.Log("[SHEET2SO] InitializeGeneration COMPLETE");
        }


        protected virtual void DownloadAllData()
        {
            if (isProcessing)
            {
                Debug.LogWarning("[SHEET2SO] Data Processing is in progress");
                return;
            }

            Debug.Log($"[SHEET2SO] Start Download All Data: {configSO.SheetList.Count} Sheets");
            InitializeGeneration();

            dataQueue = new Queue<SheetData>(configSO.SheetList);

            EditorApplication.update += ProcessAllDataUpdate;
        }

        protected virtual void StartSingleDataDownload(SheetData sheetData)
        {
            if (isProcessing)
            {
                Debug.LogWarning("[SHEET2SO] Data Processing is in progress");
                return;
            }

            string url = sheetData.GetExportURL();

            if (string.IsNullOrEmpty(url) || !url.Contains("docs.google.com/spreadsheets"))
            {
                Debug.LogError($"[SHEET2SO] Wrong URL Format: {url}");
                return;
            }

            Debug.Log($"[SHEET2SO] Start Download: {sheetData.sheetName}");
            InitializeGeneration();

            dataQueue = new Queue<SheetData>();
            dataQueue.Enqueue(sheetData);

            EditorApplication.update += ProcessAllDataUpdate;
        }

        protected virtual void ProcessAllDataUpdate()
        {
            if (dataQueue.Count > 0 && !isCancelled)
            {
                var currentItem = dataQueue.Dequeue();

                curStatus = $"Processing: {currentItem.sheetName}";
                curProgress = 1f - (float)dataQueue.Count / configSO.SheetList.Count;

                Debug.Log($"[SHEET2SO] Processing: {currentItem.sheetName}");
                ProcessDataForType(currentItem);
            }
            else
            {
                isProcessing = false;
                
                EditorApplication.update -= ProcessAllDataUpdate;
                
                Debug.Log("[SHEET2SO] All Data Processing is Complete");
                Repaint();
            }
        }

        protected virtual void ProcessDataForType(SheetData sheetData)
        {
            string url = sheetData.GetExportURL();

            if (!url.Contains("docs.google.com/spreadsheets"))
            {
                Debug.LogError($"[SHEET2SO] Wrong Google Sheets URL Format: {url}");
                return;
            }

            string exportUrl = url;
            Debug.Log($"[SHEET2SO] [{sheetData.sheetName}] URL Request START: {exportUrl}");

            using (var www = UnityWebRequest.Get(exportUrl))
            {
                www.timeout = 30;
                var operation = www.SendWebRequest();

                while (!operation.isDone)
                {
                    if (isCancelled)
                    {
                        www.Abort();
                        return;
                    }
                }

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"[SHEET2SO] [{sheetData.sheetName}] Download Success");
                    string dataText = www.downloadHandler.text;

                    var sheetFormat = SheetFormat.GetFormatType(url);

                    SaveDataToFile(sheetData.sheetName, dataText, sheetFormat);

                    var delimiter = SheetFormat.GetDelimiter(sheetFormat);
                    
                    configSO.ProcessDataToSO(sheetData.sheetName, dataText, delimiter);
                }
                else
                {
                    Debug.LogError($"[SHEET2SO] [{sheetData.sheetName}] Download Failed: {exportUrl}\nError: {www.error}");
                }
            }
        }


        protected virtual void SaveDataToFile(string type, string dataText, DoppleLittleHelper.FORMAT_TYPE sheetFormat)
        {
            string directoryPath = $"{Application.dataPath}/../Documents/SheetData/{createTime:yyyy-MM-dd HH-mm-ss}/";

            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(Path.Combine(directoryPath, $"{type}.{sheetFormat}"), dataText);
        }
    }
} 