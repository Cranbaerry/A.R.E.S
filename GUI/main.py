import qdarkstyle, os, sys, requests, urllib, json, re, threading, queue
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from datetime import datetime

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(842, 525)
        uic.loadUi('untitled.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        self.updateimage("https://i.ibb.co/3pHS4wB/Default-Placeholder.png")
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
        self.LogFolder = self.Settings["Avatar_Folder"]
        self.ModConfig = self.Settings["Avatar_Folder"]+"/Config.json"
        try:
            with open(self.ModConfig, "r+") as s:
                self.ModSettings = json.loads(s.read())
        except:
            pass
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.LogSize = self.findChild(QtWidgets.QLabel, 'LogSize')
        if os.path.exists(self.LogFolder + "/Log.txt"):
            self.LSize = os.path.getsize(self.LogFolder + "/Log.txt")
            self.LogSize.setText(str(round(self.LSize/(1024*1024)))+"MB")
        if self.Settings["ALLOW_API_UPLOAD"]:
            if os.path.exists(self.LogFolder + "/Log.txt"):
                threading.Thread(target=self.startuploads, args={}).start()
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
        self.LogFolderButton = self.findChild(QtWidgets.QPushButton, 'SetLogFolder')
        self.LogFolderButton.clicked.connect(self.updatesettings)
        self.LoadButton = self.findChild(QtWidgets.QPushButton, 'LoadButton')
        self.LoadButton.clicked.connect(self.loadavatars)
        self.BackButton = self.findChild(QtWidgets.QPushButton, 'BackButton')
        self.BackButton.clicked.connect(self.Back)
        self.SearchButton = self.findChild(QtWidgets.QPushButton, 'SearchButton')
        self.SearchButton.clicked.connect(self.Search)
        self.NextButton = self.findChild(QtWidgets.QPushButton, 'NextButton')
        self.NextButton.clicked.connect(self.Next)
        self.AvatarNameRB = self.findChild(QtWidgets.QRadioButton, 'AvatarNameRB')
        self.AvatarAuthorRB = self.findChild(QtWidgets.QRadioButton, 'AvatarAuthorRB')
        self.AvatarIDRB = self.findChild(QtWidgets.QRadioButton, 'AvatarIDRB')
        self.Status = self.findChild(QtWidgets.QLabel, 'Status')
        self.PrivateBox = self.findChild(QtWidgets.QCheckBox, 'PrivateBox')
        self.PublicBox = self.findChild(QtWidgets.QCheckBox, 'PublicBox')
        self.DLVRCAButton = self.findChild(QtWidgets.QPushButton, 'DLVRCAButton')
        self.DLVRCAButton.clicked.connect(self.DownVRCA)
        self.HotswapButton = self.findChild(QtWidgets.QPushButton, 'HotswapButton')
        self.HotswapButton.clicked.connect(self.HotSwap)
        self.DeleteLogButton = self.findChild(QtWidgets.QPushButton, 'DeleteLogButton')
        self.DeleteLogButton.clicked.connect(self.DeleteLogs)
        self.apibox = self.findChild(QtWidgets.QCheckBox, 'apibox')
        self.apibox.clicked.connect(self.updateapi)
        self.apibox.setCheckState(self.Settings["ALLOW_API_UPLOAD"])
        self.Instructions = self.findChild(QtWidgets.QTextEdit, 'Instructions')
        try:
            ss=requests.get("https://pastebin.com/raw/37Kt7J0r").text
            self.Instructions.setText(ss)
        except:
            pass
        try:
            self.LogOwnAvatarsbox = self.findChild(QtWidgets.QCheckBox, 'LogOwnAvatarsbox')
            self.LogOwnAvatarsbox.clicked.connect(self.LogOwnAvatarsbox1)
            self.LogOwnAvatarsbox.setCheckState(self.ModSettings["LogOwnAvatars"])

            self.LogFriendsAvatarsbox = self.findChild(QtWidgets.QCheckBox, 'LogFriendsAvatarsbox')
            self.LogFriendsAvatarsbox.clicked.connect(self.LogFriendsAvatarsbox1)
            self.LogFriendsAvatarsbox.setCheckState(self.ModSettings["LogFriendsAvatars"])

            self.LogToConsolebox = self.findChild(QtWidgets.QCheckBox, 'LogToConsolebox')
            self.LogToConsolebox.clicked.connect(self.LogToConsolebox1)
            self.LogToConsolebox.setCheckState(self.ModSettings["LogToConsole"])
        except:
            pass

    def LogToConsolebox1(self):
        self.ModSettings["LogToConsole"] = self.LogToConsolebox.isChecked()
        with open(self.ModConfig, "w+") as s:
            s.write(json.dumps(self.ModSettings, indent=4))

    def LogFriendsAvatarsbox1(self):
        self.ModSettings["LogFriendsAvatars"] = self.LogFriendsAvatarsbox.isChecked()
        with open(self.ModConfig, "w+") as s:
            s.write(json.dumps(self.ModSettings, indent=4))

    def LogOwnAvatarsbox1(self):
        self.ModSettings["LogOwnAvatars"] = self.LogOwnAvatarsbox.isChecked()
        with open(self.ModConfig, "w+") as s:
            s.write(json.dumps(self.ModSettings, indent=4))

    def updateapi(self):
        self.Settings["ALLOW_API_UPLOAD"] = self.apibox.isChecked()
        with open("Settings.json", "w+") as s:
            s.write(json.dumps(self.Settings, indent=4))

    def upload1(self):
        global domain
        global avis
        global q
        q = queue.Queue()
        if not os.path.exists("uploaded.txt"):
            with open("uploaded.txt", "a") as p:
                pass
        kk = requests.get(url="https://pastebin.com/raw/8DzGLek5").text
        # input(kk)
        domain = kk
        pubpath = self.LogFolder + "/Log.txt"
        with open("uploaded.txt", "r+", errors="ignore") as k:
            avis = k.read()
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)"
        with open(pubpath, "r+", errors="ignore") as g:
            kk = g.read()
            ho = re.findall(pat, kk)
            for x in ho:
                q.put(x)

    def upload(self, data):
        x = data
        hooh = {
            "TimeDetected": x[0],
            "AvatarID": x[1],
            "AvatarName": x[2],
            "AvatarDescription": x[3],
            "AuthorID": x[4],
            "AuthorName": x[5],
            "AssetURL": x[6],
            "ImageURL": x[7],
            "ThumbnailURL": x[8],
            "ReleaseStatus": x[9],
            "Version": x[10]
        }
        url = "http://" + domain + "/upload"
        headers = {"Content-Type": "application/json"}
        if str(x[1]) not in avis:
            try:
                response = requests.request("POST", url, json=hooh, headers=headers)
            except:
                pass
            with open("uploaded.txt", "a+", errors="ignore") as k:
                k.writelines(x[1] + "\n")
            #print("uploaded: " + str(x[2]))

    def startuploads(self):
        headers = {
            'accept': 'application/json',
        }
        try:
            response = requests.get('https://avataruploader.loca.lt/status', headers=headers, timeout=5)
            if "ONLINE" in response.text:
                self.upload1()
                tt = 10
                while True:
                    if threading.activeCount() <= tt:
                        threading.Thread(target=self.upload, args={q.get(), }).start()
                    if q.qsize() <= 0:
                        break
                while threading.activeCount() <= 0:
                    pass
            else:
                print("SERVER OFFLINE")
        except:
            print("SERVER OFFLINE")

    def updateimage(self, url):
        self.leftbox = self.findChild(QtWidgets.QLabel, 'PreviewImage')
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"}
        data = requests.request("GET", url, data=payload, headers=headers)
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.leftbox.setPixmap(pixmap)
        self.NextButton.show()
        self.BackButton.show()

    def updatesettings(self):
        self.LogFolderInput = self.findChild(QtWidgets.QTextEdit, 'LogFolderDir')
        LogFolder = self.LogFolderInput.toPlainText()
        self.Settings["Avatar_Folder"] = LogFolder
        with open("Settings.json", "w+") as s:
            s.write(json.dumps(self.Settings, indent=4))
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
        sys.exit()

    def sortFunction(self, value):
        return value[0]

    def Back(self):

        if self.AvatarIndex == 0:
            return
        self.BackButton.hide()
        self.NextButton.hide()
        self.AvatarIndex -= 1
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))

    def Next(self):
        if self.AvatarIndex + 1 >= self.MaxAvatar:
            return
        self.NextButton.hide()
        self.BackButton.hide()
        self.AvatarIndex += 1
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))

    def AvatarUpdate(self, AVIndex):
        self.AVIS = self.Avatars[AVIndex]
        if self.AVIS[9] == "private":
            self.Status.setStyleSheet("background-color: red; border: 3px solid black;")
            self.Status.setText("Private")
        if self.AVIS[9] == "public":
            self.Status.setStyleSheet("background-color: blue; border: 3px solid black;")
            self.Status.setText("public")
        threading.Thread(target=self.updateimage, args={self.AVIS[7], }).start()
        self.RawData = self.findChild(QtWidgets.QTextEdit, 'RawData')
        self.RawData.setPlainText(self.Cleantext(self.AVIS))

    def loadavatars(self):
        self.leftbox.show()
        self.Status.show()
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)"
        self.LogFolder = self.Settings["Avatar_Folder"]
        try:
            with open(self.LogFolder + "\Log.txt", "r+", errors="ignore") as s:
                self.Logs = s.read()
                self.Avatars = re.findall(pat, self.Logs)
            avii = []
            self.Avatars = sorted(self.Avatars, key=self.sortFunction, reverse=True)
            allowed = []
            if self.PrivateBox.isChecked():
                allowed.append("private")
            if self.PublicBox.isChecked():
                allowed.append("public")
            for x in self.Avatars:
                if x[9] in allowed:
                    avii.append(x)
            self.Avatars = avii
            self.MaxAvatar = len(self.Avatars)
            self.AvatarIndex = 0
            self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
            self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))
            self.AvatarUpdate(0)
        except:
            self.RawData.setPlainText("INVALID LOG FOLDER")

    def Cleantext(self, data):
        klean = f"""Time Detected:{datetime.utcfromtimestamp(int(data[0])).strftime('%Y-%m-%d %H:%M:%S')}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nVersion:{data[10]}"""
        return klean

    def Search(self):
        self.loadavatars()
        AvatarsS = []
        self.lineEdit = self.findChild(QtWidgets.QLineEdit, 'lineEdit')
        self.searched = self.lineEdit.text()
        if self.AvatarNameRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[2]).lower():
                    AvatarsS.append(x)
        if self.AvatarAuthorRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[5]).lower():
                    AvatarsS.append(x)
        if self.AvatarIDRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[1]).lower():
                    AvatarsS.append(x)
        self.Avatars = AvatarsS
        self.MaxAvatar = len(self.Avatars)
        self.AvatarIndex = 0
        if self.MaxAvatar == 0:
            self.RawData.setPlainText("NO RESULTS")
            self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
            self.resultsbox.setText("LOADED: " + str(self.AvatarIndex) + "/" + str(self.MaxAvatar))
            self.leftbox.hide()
            self.Status.hide()
            return
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))

    def DownVRCAT(self, url, dir1):
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"}
        data = requests.request("GET", url, data=payload, headers=headers, stream=True)
        with open(dir1, "wb") as v:
            v.write(data.content)

    def DownVRCA(self):
        self.fileName = QFileDialog.getSaveFileName(self, 'Save VRCA', "Avatar", ".vrca")
        self.DLLink = self.Avatars[self.AvatarIndex][6]
        self.SaveDir = "".join(self.fileName)
        threading.Thread(target=self.DownVRCAT, args={self.DLLink, self.SaveDir, }).start()

    def ReplaceID(self, oldid, newid):
        with open("decompressedfile", "rb") as f:
            kok = f.read()
        kok = kok.replace(bytes(oldid, 'utf-8'), bytes(newid, 'utf-8'))
        with open("decompressedfile1", "wb") as f:
            f.write(kok)

    def HotSwap(self):
        self.NewID = self.findChild(QtWidgets.QLineEdit, 'NewID')
        self.NewID1 = self.NewID.text()
        if "New Avatar ID" in self.NewID1:
            self.NewID.setText("New Avatar ID Required!")
            return
        self.DownVRCAT(self.Avatars[self.AvatarIndex][6], "HOTSWAP/Avatar.vrca")
        os.chdir("HOTSWAP")
        if os.path.exists("custom.vrca"):
            os.remove("custom.vrca")
        os.system("HOTSWAP.exe d Avatar.vrca")
        self.oldid = self.Avatars[self.AvatarIndex][1]
        self.ReplaceID(self.oldid, self.NewID1)
        os.system("HOTSWAP.exe c")
        if os.path.exists("Avatar.vrca"):
            os.remove("Avatar.vrca")
        if os.path.exists("decompressedfile"):
            os.remove("decompressedfile")
        if os.path.exists("decompressedfile1"):
            os.remove("decompressedfile1")
        os.chdir("..")

    def DeleteLogs(self):
        if os.path.exists(self.LogFolder + "/Log.txt"):
            os.remove(self.LogFolder + "/Log.txt")

app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application
