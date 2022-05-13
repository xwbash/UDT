using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DropboxTool
{
    public class DropboxUploader : OdinEditorWindow
    {
        
        [GUIColor(0.9f, 0.9f,0.9f)]
        [Header("Dropbox App Name")]
        public string DropboxAppName;
        
        [Header("Folder Names")]
        public List<string> FolderNames = new List<string>();
 
        [HorizontalGroup()]
        [Button(ButtonSizes.Medium, ButtonStyle.CompactBox), GUIColor(0,1f,0)]
        public void UploadFiles()
        {
            DropboxUpload.StartProcess(DropboxAppName, FolderNames);

        }
        [MenuItem("Window/Dropbox Tool")]
        public static void ShowWindow()
        {
            GetWindow<DropboxUploader>("<-> Dropbox Upload Tool <->");
        }



       
    }
}