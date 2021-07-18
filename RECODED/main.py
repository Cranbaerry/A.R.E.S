import qdarkstyle, os, sys, requests, urllib, json, re, threading
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        uic.loadUi('untitled.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        self.updateimage("https://i.ibb.co/3pHS4wB/Default-Placeholder.png")
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
        self.LogFolder = self.Settings["Avatar_Folder"]
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.LogSize = self.findChild(QtWidgets.QLabel, 'LogSize')
        if os.path.exists(self.LogFolder + "/Log.txt"):
            self.LSize = os.path.getsize(self.LogFolder + "/Log.txt")
            self.LogSize.setText(str(round(self.LSize/(1024*1024)))+"MB")
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
        klean = f"""Time Detected:{data[0]}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nVersion:{data[10]}"""
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
