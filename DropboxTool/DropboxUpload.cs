using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DropboxTool
{
    public class DropboxUpload : MonoBehaviour
    {
        public static void StartProcess(string appName, List<string> folderName)
        {
            var directoryProductName = Directory.GetCurrentDirectory()
                .Split(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/")[1];
            GetFiles(appName, directoryProductName, folderName);
        }
        
        private static void GetFiles(string appName, string productName, List<string> folderName)
        { Dictionary<string, Directorys> nameDirectory = new Dictionary<string, Directorys>();
            List<string> folderList = new List<string>(); 
            var userName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); //Get the directory's max range is (user_profile)
            var currentDirectory = Directory.GetCurrentDirectory() + "/Assets/_game/"; // CurrentDirectory : /Users../ProjectName/Assets/
            var appsName = appName; // App name on dropbox
            var dateTime = DateTime.Now.ToString(); // Date time now
            dateTime = dateTime.Replace("/", "-"); // It's see's the / as directory not a name you can change it with escape character
            var dropboxShortcut = userName + "/Dropbox/Unco-DropBoxTool/"+appsName+"/"+productName+new string(' ', 5)+dateTime; 
            
            
            
            Directory.CreateDirectory(dropboxShortcut); // Create an directory on dropbox
            
            //Folder List
            foreach (var folderNames in folderName)
            {
                folderList.Add(currentDirectory+folderNames);
            }
            //End Folder List
            
            //Checking and Sorting files, folders.
            foreach (var folder in folderList)
            {
                string[] allfiles = Directory.GetFileSystemEntries(folder, "*.*", SearchOption.AllDirectories);
                
                List<string> files = new List<string>();
                List<string> folders = new List<string>();
                foreach (var allfile in allfiles)
                {
                    //is it file or folder?
                    if ((File.GetAttributes(allfile) & FileAttributes.Directory) == FileAttributes.Directory && !(allfile.Contains(".mat")))
                    {
                        //directory
                        folders.Add(allfile);
                    }
                    else
                    {
                        //file
                        files.Add(allfile);
                    }
                }
                folders.AddRange(files);
                //End Checking and Sorting files, folders.
                //Checking folders and starting to upload
                foreach (var assignFiles in folders)
                {
                    if (!assignFiles.Contains(".DS_Store") && !assignFiles.Contains(".meta")) // if you want to include .metas or .ds_Stores delete this if
                    {
                        var directoryName = Regex.Split(assignFiles.Split(productName + "/Assets")[1], "[^/]+$");
                        //assignFiles.Split(splitedString[0])[1] file names
                        
                        var directoryValue = new Directorys()
                        {
                            DirectoryName = dropboxShortcut + directoryName[0], // Directory name exp: /Username/Dropbox/Apps/app_name/product_name - with time date/Prefabs..
                            ObjectName = assignFiles.Split(directoryName[0])[1] // Object name exp: hello_world.png
                        };
                        nameDirectory.Add(assignFiles, directoryValue);
                        
                    }
                }
                //End Checking folders and starting to upload
                
                //Upload Files
                UploadFiles(nameDirectory);
                //End Upload files
                
                
                nameDirectory.Clear(); // this line included when the for returns again its still remains the old file directory's thats why we have to clear.
            }

            print("     <-> Uploaded Successfully <->");
        }
        private static void UploadFiles(Dictionary<string,Directorys> directoryName)
        {
            foreach (var filePath in directoryName)
            {
                try
                {
                    File.Copy(filePath.Key, filePath.Value.DirectoryName, true); // Copy the files and paste it on directory its only takes files.
                }
                catch
                {
                    Directory.CreateDirectory(filePath.Value.DirectoryName + filePath.Value.ObjectName); // This is included to create directory the same name in the project folder
                }
                
            }
        }
    }
}