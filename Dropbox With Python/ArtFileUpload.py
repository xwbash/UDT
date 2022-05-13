#Dropbox Test
def GetDropBox():
    os.system("pip3 install dropbox")
try:
    import dropbox
except ImportError:
    GetDropBox()
import os
import sys

#Client Side
dataDirectory = ""
dataFiles = ["Prefabs", "Models"]

#Dropbox Side
dropboxApi = ""
dropboxDirectory = "/Press Run 5D" #GameName


def AttachVariables():
    #dropboxDirectory=sys.argv[1]
    #dropboxApi=sys.argv[2]
    print(sys.argv[1])
    print(sys.argv[2])
    


AttachVariables()




class Data:
    def UploadFile(self, dataList, dataApi, cloudDirectory, dataFileDirectory): 
        dropBox = dropbox.Dropbox(dataApi)
        dropBox.files_create_folder(cloudDirectory)
        for fileDirect in dataFileDirectory:
            dropBox.files_create_folder(cloudDirectory+"/"+fileDirect)
            files = os.listdir(dataDirectory+fileDirect)
            print("<-> Folder created <->")
            for data in files:
                #.DS_Store is ignored file thats why we checking if its exist
                if(data.__contains__(".DS_Store")): 
                    continue   
                else:
                    print("<-> File to upload <-> %s" %(dataDirectory+fileDirect+"/"+data))
                    f = open(dataDirectory+fileDirect+"/"+data, "rb")
                    dropBox.files_upload(f.read(), (dropboxDirectory+"/"+fileDirect+"/"+data), autorename=True)
                    f.close()
            print("<-> File uploaded <->")

        
data=Data()
data.UploadFile(dataFiles, dropboxApi, dropboxDirectory, dataFiles)
quit()
