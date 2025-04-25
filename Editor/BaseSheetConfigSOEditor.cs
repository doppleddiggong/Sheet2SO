using UnityEditor;
using UnityEngine;

namespace DoppleLittleHelper
{
    [CustomEditor(typeof(BaseSheetConfigSO))]
    public class BaseSheetConfigSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BaseSheetConfigSO config = (BaseSheetConfigSO)target;

            if (GUILayout.Button("Data Clear"))
                config.Clear();
            
            if (GUILayout.Button("Load from MasterSheet.tsv"))
            {
                string folderPath = "Assets/DoppleLittleHelper/Sheet2SO/Editor/";
                string filePath = EditorUtility.OpenFilePanel("Select MasterSheet File", folderPath, "tsv");

                if (!string.IsNullOrEmpty(filePath))
                {
                    config.LoadMasterSheet(filePath);
                    Debug.Log($"[SHEET2SO] TSV File Load Complete : {filePath}");
                }
                else
                {
                    Debug.LogWarning("[SHEET2SO] File Select Canceled");
                }
            }

            if (GUILayout.Button("Auto FindData References"))
                config.AutoFindDataReferences();
        }
    }
}