import qdarkstyle, os, sys, requests, urllib, json, re
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

    def updateimage(self, url):
        self.leftbox = self.findChild(QtWidgets.QLabel, 'PreviewImage')
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"}
        data = requests.request("GET", url, data=payload, headers=headers)
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.leftbox.setPixmap(pixmap)

    def updatesettings(self):
        self.LogFolderInput = self.findChild(QtWidgets.QTextEdit, 'LogFolderDir')
        LogFolder = self.LogFolderInput.toPlainText()
        self.Settings["Avatar_Folder"] = LogFolder
        with open("Settings.json", "w+") as s:
            s.write(json.dumps(self.Settings, indent=4))
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.DirLabel.setText("CurrentDirectory: "+self.Settings["Avatar_Folder"])

    def loadavatars(self):
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)"
        LogFolder = self.Settings["Avatar_Folder"]
        with open(LogFolder+"\Public.txt", "r+") as s:
            self.Logs = s.read()
            self.ho = re.findall(pat, self.Logs)
        with open(LogFolder+"\Private.txt", "r+") as s:
            self.Logs = s.read()
            self.ho1 = re.findall(pat, self.Logs)
        self.avatars = self.ho + self.ho1
        #print(json.dumps(self.avatars))

app = QtWidgets.QApplication(sys.argv) # Create an instance of QtWidgets.QApplication
app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui() # Create an instance of our class
app.exec_() # Start the application