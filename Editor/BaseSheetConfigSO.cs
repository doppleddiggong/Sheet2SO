using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace DoppleLittleHelper
{
    [CreateAssetMenu(fileName = "BaseSheetConfig", menuName = "DoppleLittleHelper/BaseSheetConfig", order = int.MaxValue)]
    public partial class BaseSheetConfigSO : BaseSO
    {
        [Header("[Master Sheet Info]")]
        [SerializeField] List<SheetData> sheetList = new ();

        public List<SheetData> SheetList => sheetList;

        public override void Clear()
        {
            sheetList.Clear();
        }

        public void LoadMasterSheet(string filePath)
        {
            if (File.Exists(filePath))
            {
                string rawText = File.ReadAllText(filePath);

                var dataList = ParseDelimitedData<SheetData>(rawText, SheetFormat.GetDelimiter(DoppleLittleHelper.FORMAT_TYPE.tsv));
                if (dataList == null || dataList.Count == 0)
                {
                    Debug.LogError($"[SHEET2SO] Parsing Data is EMPTY");
                    return;
                }

                // 파싱된 데이터 적용
                sheetList.Clear();
                foreach (var item in (IEnumerable<SheetData>)dataList)
                    sheetList.Add(item);
                this.Save();
            }
            else
            {
                Debug.LogError($"[SHEET2SO] Data Find Failed: {filePath}");
            }
        }

        protected void SetData<T>(string sheetType, string rawText, char delimiter) where T : BaseData, new()
        {
            if (string.IsNullOrEmpty(rawText))
            {
                Debug.LogError($"[SHEET2SO][{sheetType}] Data is Empty");
                return;
            }

            Debug.Log($"[SHEET2SO][{sheetType}] Parsing Start");

            // 데이터 파싱
            var dataList = ParseDelimitedData<T>(rawText, delimiter);
            if (dataList == null || dataList.Count == 0)
            {
                Debug.LogError($"[SHEET2SO][{sheetType}] Parsing Data is EMPTY");
                return;
            }

            // 파싱된 데이터 적용
            this.AddParsedData(sheetType, dataList);

            Debug.Log($"[SHEET2SO][{sheetType}] Parsing Complete: ({dataList.Count})");
        }

        List<T> ParseDelimitedData<T>(string rawText, char delimiter) where T : BaseData, new()
        {
            int DATAVALUETYPE_INDEX = 1; // 데이터 타입 행 (예: int, double, double)
            int DATAKEY_INDEX = 2;       // 데이터 키 행 (예: Index, Atk, Def)
            int DATA_INDEX = 3;          // 실제 데이터 시작 행 (예: 1000, 2, 10000)

            // 줄바꿈 정규화 및 행 분리
            string[] readLines = rawText.Replace("\r", "").Split('\n');

            // 기본 검증
            if (readLines == null || readLines.Length <= DATA_INDEX)
            {
                Debug.LogError($"[SHEET2SO]Data is Wrong. Need Min {DATA_INDEX + 1} ROWS");
                return null;
            }

            Debug.Log($"[SHEET2SO]Data ROWS : {readLines.Length}");

            // 헤더 정보 추출
            string[] dataKeys = readLines[DATAKEY_INDEX].Split(delimiter);
            string[] dataTypes = readLines[DATAVALUETYPE_INDEX].Split(delimiter);

            // 데이터 파싱
            List<T> dataList = new List<T>();

            for (int i = DATA_INDEX; i < readLines.Length; i++)
            {
                string lineData = readLines[i].Trim();

                if (string.IsNullOrEmpty(lineData))
                    continue;

                string[] splitLineData = lineData.Split(delimiter);
                if (splitLineData == null || splitLineData.Length == 0)
                    continue;

                T data = new T();
                data.Parse(splitLineData, dataKeys, dataTypes);
                dataList.Add(data);
            }

            return dataList;
        }


        public virtual void ClearSOData() { }
        public virtual void AutoFindDataReferences() { }
        public virtual void ProcessDataToSO(string sheetType, string dataText, char delimiter) { }
        protected virtual void AddParsedData(string type, IEnumerable<object> dataList) { }
    }

    public class SheetFormat
    {
        public static FORMAT_TYPE GetFormatType(string url) => url.Contains("tsv") ? FORMAT_TYPE.tsv : FORMAT_TYPE.csv;
        public static char GetDelimiter(FORMAT_TYPE format)
        {
            switch (format)
            {
                case FORMAT_TYPE.tsv:
                    return '\t';
                case FORMAT_TYPE.csv:
                    return ',';
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, "This Google Sheet format is not supported.");
            }
        }
    }

    public enum FORMAT_TYPE : byte
    {
        tsv,
        csv,
    }
}