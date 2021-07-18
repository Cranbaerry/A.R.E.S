import qdarkstyle, os, sys, requests, urllib, json, re, threading
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__() # Call the inherited classes __init__ method
        uic.loadUi('untitled.ui', self) # Load the .ui file
        self.show() # Show the GUI
        self.updateimage("https://i.ibb.co/3pHS4wB/Default-Placeholder.png")
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.DirLabel.setText("CurrentDirectory: "+self.Settings["Avatar_Folder"])
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
        self.DirLabel.setText("CurrentDirectory: "+self.Settings["Avatar_Folder"])

    def sortFunction(self, value):
        return value[0]

    def Back(self):

        if self.AvatarIndex == 0:
            return
        self.BackButton.hide()
        self.NextButton.hide()
        self.AvatarIndex -= 1
        self.AvatarUpdate(self.AvatarIndex)

    def Next(self):
        if self.AvatarIndex > self.MaxAvatar:
            return
        self.NextButton.hide()
        self.BackButton.hide()
        self.AvatarIndex += 1
        self.AvatarUpdate(self.AvatarIndex)

    def AvatarUpdate(self, AVIndex):
        self.AVIS = self.Avatars[AVIndex]
        threading.Thread(target=self.updateimage, args={self.AVIS[7],}).start()
        self.RawData = self.findChild(QtWidgets.QTextEdit, 'RawData')
        self.RawData.setPlainText(self.Cleantext(self.AVIS))

    def loadavatars(self):
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)"
        LogFolder = self.Settings["Avatar_Folder"]
        with open(LogFolder+"\Log.txt", "r+", errors="ignore") as s:
            self.Logs = s.read()
            self.Avatars = re.findall(pat, self.Logs)
        self.Avatars = sorted(self.Avatars, key=self.sortFunction, reverse=True)
        self.MaxAvatar = len(self.Avatars)
        self.AvatarIndex = 0
        self.AvatarUpdate(0)

    def Cleantext(self, data):
        klean = f"""Time Detected:{data[0]}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nVersion:{data[10]}"""
        return klean

    def Search(self):
        if self.AvatarNameRB.isChecked():
            pass
        if self.AvatarAuthorRB.isChecked():
            pass
        if self.AvatarIDRB.isChecked():
            pass

app = QtWidgets.QApplication(sys.argv) # Create an instance of QtWidgets.QApplication
app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui() # Create an instance of our class
app.exec_() # Start the application