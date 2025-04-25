using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DoppleLittleHelper
{
    [Serializable]
    public class SheetData : BaseData
    {
        public int index;
        public string sheetName;
        public DoppleLittleHelper.FORMAT_TYPE sheetFormat = DoppleLittleHelper.FORMAT_TYPE.tsv;
        public string url;

        public string spreadSheetId;
        public string sheetId;

        public override void Parse(string[] _data, string[] _keys, string[] _types)
        {
            int _idx = 0;

            int.TryParse(_data[_idx++], out index);
            sheetName = _data[_idx++];
            Enum.TryParse(_data[_idx++].ToUpper(), out sheetFormat);
            url = _data[_idx++];

            spreadSheetId = ExtractDocumentId(url);
            sheetId = ExtractLastGid(url);

            if( string.IsNullOrEmpty(spreadSheetId))
                Debug.Log("[SHEET2SO] spreadSheetId Wrong : CHECK URL DATA");
            if (string.IsNullOrEmpty(sheetId))
                Debug.Log("[SHEET2SO] sheetId Wrong : CHECK URL DATA");

            CustomParse();
        }

        public static string ExtractDocumentId(string url)
        {
            var match = Regex.Match(url, @"spreadsheets/d/([^/]+)");
            return match.Success ? match.Groups[1].Value : null;
        }

        public static string ExtractLastGid(string url)
        {
            var matches = Regex.Matches(url, @"gid=(\d+)");
            return matches.Count > 0 ? matches[matches.Count - 1].Groups[1].Value : null;
        }

        public string GetExportURL() => $"https://docs.google.com/spreadsheets/d/{spreadSheetId}/export?format={sheetFormat}&gid={sheetId}";
    }
}