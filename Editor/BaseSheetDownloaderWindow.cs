using UnityEngine;
using UnityEditor;

namespace DoppleLittleHelper
{
    public class BaseSheetDownloaderWindow : DoppleLittleHelper.BaseSheetDownloader<BaseSheetConfigSO>
    {
        static BaseSheetDownloaderWindow window;

        [MenuItem("Tools/DoppleLittleHelper/Base Sheet Downloader")]
        public static void ShowWindow()
        {
            window = GetWindow<BaseSheetDownloaderWindow>("Base Sheet Downloader");
            window.minSize = new Vector2(400, 300);
        }
    }
}