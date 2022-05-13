using System;
using System.Collections.Generic;
using System.IO;
using Dropbox.Api;
using Dropbox.Api.Files;
using UnityEngine;
using UnityEditor;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Debug = UnityEngine.Debug;


namespace UncoDropBox
{
    public class DropboxTest : MonoBehaviour
    {
        private static string accessToken =
            "";

        private static string _email = "";
        private static string _password = "";
        [MenuItem("Assets/Unco Dropbox/Upload To Dropbox")]
        private static void GetAccessToken()
        {

            //Login
            bool exception;
            var webDriver = new ChromeDriver(Directory.GetCurrentDirectory()+"/Assets/UncoDropBox");
            print(webDriver.IsActionExecutor);
            webDriver.Navigate().GoToUrl("https://www.dropbox.com/developers/apps/info/d8t1sic07r3eyvt#settings");
            print(webDriver.IsActionExecutor);

            exception = false;
            while (!exception)
            {
                try
                {
                    var emailTextBox = GetElement("/html/body/div[12]/div[1]/div[2]/div/div/div[3]/div/div[1]/div[2]/div/div/form/div[1]/div[1]/div[2]/input", webDriver);
                    var passwordTextBox = GetElement("/html/body/div[12]/div[1]/div[2]/div/div/div[3]/div/div[1]/div[2]/div/div/form/div[1]/div[2]/div[2]/input", webDriver);
                    var buttonLogin = GetElement("//*[@id=\"login-or-register-page-content\"]/div/div[3]/div/div[1]/div[2]/div/div/form/div[2]/button", webDriver);
                    emailTextBox.SendKeys(_email);
                    passwordTextBox.SendKeys(_password);
                    buttonLogin.Click();
                    exception = true;
                }
                catch (Exception e)
                {
                    //Debug.Log(e);
                    exception = false;
                }
            }
            //End Login
            //Generate Access
 
            exception = false;
            while (!exception)
            {
                try
                {
                    var generateButton =
                        GetElement(
                            "/html/body/div[11]/div[2]/div[2]/div/div[1]/div[1]/div/table/tbody/tr[7]/td[2]/div[6]/input",
                            webDriver);
                    generateButton.Click();
                    exception = true;
                }
                catch (Exception e)
                {
                    //Debug.Log(e);
                    exception = false;
                }
            }

            
            exception = false;
            IWebElement accessToken = null;
            while (!exception)
            {
                try
                {
                    accessToken = GetElement("/html/body/div[11]/div[2]/div[2]/div/div[1]/div[1]/div/table/tbody/tr[7]/td[2]/div[6]/div/input", webDriver);
                    exception = true;
                }
                catch (Exception e)
                {
                    //Debug.Log(e);
                    exception = false;
                }
            }

        }
        
        private static IWebElement GetElement(string element, ChromeDriver webDriver)
        {
            return webDriver.FindElement(By.XPath(element));
        }
        
        
        private void GetFiles()
        {
            List<string> folderList = new List<string>(); // #@ Directory + FileName
            List<string> fileList = new List<string>();
            List<string> fileNames = new List<string>();
            var dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "-");
            string dropBoxFolderDirectory = Application.productName + " " + dateTime;
            string fileDirectory = "Assets/"; 
            var dropboxClient = new DropboxClient(accessToken);
        
        
            folderList.Add("Models"); // Foldernames
            folderList.Add("Prefabs");
            print(dropBoxFolderDirectory);
            foreach (var folders in folderList)
            {
                fileList.Clear();
                fileNames.Clear();
                foreach (var assignFiles in System.IO.Directory.GetFiles(fileDirectory + folders))
                {
                    if (!assignFiles.Contains(".DS_Store") && !assignFiles.Contains(".meta"))
                    {
                        string[] array = assignFiles.Split(folders+"/");
                        fileNames.Add(array[1]);
                        fileList.Add(assignFiles);
                    }
                }
                CreateDirectory(dropboxClient, "/"+dropBoxFolderDirectory+"/"+folders);
                UploadFiles(fileList, dropboxClient, "/"+dropBoxFolderDirectory+"/"+folders, fileNames);
            
            }
        }

        private void UploadFiles(List<string> filesList, DropboxClient dropboxClient, string dropBoxFolderDirectory, List<string> fileNames)
        {
            var index = 0;
            foreach (var file in filesList)
            {
                var mem = new MemoryStream(File.ReadAllBytes(file));
                var updated = dropboxClient.Files.UploadAsync(dropBoxFolderDirectory+"/"+fileNames[index], WriteMode.Overwrite.Instance, body: mem);
                updated.Wait();
                var asyncFile = dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync(dropBoxFolderDirectory+"/"+fileNames[index]);
                asyncFile.Wait();
                Debug.Log(" <-> Dropbox Upload Success <->");
                index += 1;
            }
        }

        private void CreateDirectory(DropboxClient dropboxClient, string dropboxDirectory)
        {
            dropboxClient.Files.CreateFolderBatchCheckAsync(dropboxDirectory);
            var folderArg = new CreateFolderArg(dropboxDirectory);
            var folder = dropboxClient.Files.CreateFolderV2Async(folderArg);
            folder.Wait();
        }

    }
}

//https://www.dropbox.com/1/oauth2/authorize?client_id=dz66n8y9qank09k&response_type=code&redirect_uri=localhost:8080