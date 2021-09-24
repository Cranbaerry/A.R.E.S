import os, sys, requests, shutil, threading, json, winshell
from PyQt5 import QtWidgets, uic, QtCore
from PyQt5.QtWidgets import *
from win32com.client import Dispatch
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"
class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(381, 270)
        uic.loadUi('GUI.ui', self)  # Load the .ui file
        self.show()
        self.SelVRC = self.findChild(QtWidgets.QPushButton, 'SelVRC')
        self.SelVRC.clicked.connect(self.SelVRC1)
        self.SelUnity = self.findChild(QtWidgets.QPushButton, 'SelUnity')
        self.SelUnity.clicked.connect(self.SelUnity1)
        self.SelUnity.setEnabled(False)
        self.Install = self.findChild(QtWidgets.QPushButton, 'Install')
        self.Install.clicked.connect(self.Install1)
        self.Install.setEnabled(False)
        self.LogOwnAvisCB = self.findChild(QtWidgets.QCheckBox, 'LogOwnAvisCB')
        self.LogFriendsAvisCB = self.findChild(QtWidgets.QCheckBox, 'LogFriendsAvisCB')
        self.LogToConsoleCB = self.findChild(QtWidgets.QCheckBox, 'LogToConsoleCB')
        self.AllowAPICB = self.findChild(QtWidgets.QCheckBox, 'AllowAPICB')
        self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'progressBar')
        self.ProgBar.setValue(0)
        try:
            os.remove("MOD.rar")
        except:
            pass
        try:
            os.remove("GUI.rar")
        except:
            pass
        try:
            os.system('rmdir /S /Q "{}"'.format("MOD"))
        except:
            pass
        try:
            os.system('rmdir /S /Q "{}"'.format("GUI"))
        except:
            pass

    def SelVRC1(self):
        self.VRCPath = QFileDialog.getOpenFileName(self, 'Select VRChat.exe', 'VRChat', "EXE Files (*.exe)")[0].replace("/VRChat.exe", "")
        self.SelUnity.setEnabled(True)

    def SelUnity1(self):
        self.UPath = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
        self.Install.setEnabled(True)

    def Install1(self):
        threading.Thread(target=self.Install2, args=()).start()

    def Install2(self):
        self.Install.setEnabled(False)
        self.SelVRC.setEnabled(False)
        self.SelUnity.setEnabled(False)
        self.isupdating = False
        self.ProgBar.setValue(8)
        try:
            os.system('taskkill /F /im "Avatar Logger GUI.exe"')
        except:
            pass
        try:
            os.system('taskkill /F /im "Unity Hub.exe"')
        except:
            pass
        try:
            os.system('taskkill /F /im "Unity.exe"')
        except:
            pass
        try:
            os.system('taskkill /F /im "VRChat.exe"')
        except:
            pass
        self.ProgBar.setValue(16)
        if os.path.isdir(self.VRCPath + "/AvatarLog") or os.path.isdir(self.VRCPath + "/GUI") or os.path.isfile(self.VRCPath + "/Leaf.xNet.dll"):
            self.isupdating = True
            try:
                shutil.move(self.VRCPath + "/GUI/uploaded.txt", self.VRCPath + "/olduploaded.txt")
            except:
                pass
            try:
                shutil.move(self.VRCPath + "/AvatarLog/Log.txt", self.VRCPath + "/OldLog.txt")
            except:
                pass
            try:
                shutil.move(self.VRCPath + "/AvatarLog/Config.json", self.VRCPath + "/OldConfig.json")
            except:
                pass
            try:
                os.system('rmdir /S /Q "{}"'.format(self.VRCPath + "/AvatarLog"))
            except:
                pass
            try:
                os.system('rmdir /S /Q "{}"'.format(self.VRCPath + "/GUI"))
            except:
                pass
            try:
                os.remove(self.VRCPath + "/Leaf.xNet.dll")
            except:
                pass
            try:
                os.remove(self.VRCPath + "/Mods/AvatarLogger.dll")
            except:
                pass
        self.ProgBar.setValue(24)
        DLLink = requests.get("https://pastebin.com/raw/Q0w5ttLH", timeout=10).text
        self.ProgBar.setValue(32)
        self.GUIURL = DLLink.split("|")[0]
        self.ModURL = DLLink.split("|")[1]
        os.system("curl -L " + self.GUIURL + " > GUI.rar")
        os.system("curl -L " + self.ModURL + " > MOD.rar")
        self.ProgBar.setValue(40)
        os.mkdir("MOD")
        os.system("UnRAR.exe x MOD.rar MOD")
        self.ProgBar.setValue(48)
        os.mkdir("GUI")
        os.system("UnRAR.exe x GUI.rar GUI")
        self.ProgBar.setValue(56)
        with open("MOD/AvatarLog/Config.json", "r+", errors="ignore") as c:
            self.ModConfig = json.loads(c.read())
        self.LOA = self.LogOwnAvisCB.isChecked()
        self.ModConfig["LogOwnAvatars"] = self.LOA
        self.LFA = self.LogFriendsAvisCB.isChecked()
        self.ModConfig["LogFriendsAvatars"] = self.LFA
        self.LTC = self.LogToConsoleCB.isChecked()
        self.ModConfig["LogToConsole"] = self.LTC
        self.AA = self.AllowAPICB.isChecked()
        self.ModConfig["SendToAPI"] = self.AA
        with open("MOD/AvatarLog/Config.json", "w+", errors="ignore") as c:
            c.write(json.dumps(self.ModConfig, indent=4))
        with open("GUI/Settings.json", "r+", errors="ignore") as c:
            self.GUISettings = json.loads(c.read())
        self.GUISettings["Avatar_Folder"] = self.VRCPath
        self.GUISettings["Unity_Exe"] = self.UPath
        with open("GUI/Settings.json", "w+", errors="ignore") as c:
            c.write(json.dumps(self.GUISettings, indent=4))
        self.ProgBar.setValue(64)
        shutil.move("GUI", self.VRCPath)
        try:
            os.remove(self.VRCPath + "/Leaf.xNet.dll")
        except:
            pass
        shutil.move("MOD/Leaf.xNet.dll", self.VRCPath)
        shutil.move("MOD/AvatarLog", self.VRCPath)
        shutil.move("MOD/Mods/AvatarLogger.dll", self.VRCPath + "/Mods")
        self.ProgBar.setValue(72)
        try:
            os.remove("MOD.rar")
        except:
            pass
        try:
            os.remove("GUI.rar")
        except:
            pass
        try:
            os.system('rmdir /S /Q "{}"'.format("MOD"))
        except:
            pass
        try:
            os.system('rmdir /S /Q "{}"'.format("GUI"))
        except:
            pass
        self.ProgBar.setValue(80)
        if self.isupdating:
            try:
                shutil.move(self.VRCPath + "/OldLog.txt", self.VRCPath + "/AvatarLog/Log.txt")
            except:
                pass
            try:
                shutil.move(self.VRCPath + "/OldConfig.json", self.VRCPath + "/AvatarLog/Config.json")
            except:
                pass
            try:
                shutil.move(self.VRCPath + "/olduploaded.txt", self.VRCPath + "/GUI/uploaded.txt")
            except:
                pass
        self.ProgBar.setValue(88)
        self.DT = winshell.desktop()
        self.path = os.path.join(self.DT, 'Avatar Logger GUI.lnk')
        self.target = self.VRCPath + "/GUI/Avatar Logger GUI.exe"
        self.wDir = self.VRCPath + "/GUI"
        self.icon = self.VRCPath + "/GUI/Avatar Logger GUI.exe"
        self.shell = Dispatch('WScript.Shell')
        self.ProgBar.setValue(96)
        self.shortcut = self.shell.CreateShortCut(self.path)
        self.shortcut.Targetpath = self.target
        self.shortcut.WorkingDirectory = self.wDir
        self.shortcut.IconLocation = self.icon
        self.shortcut.save()
        self.ProgBar.setValue(100)
        self.ProgBar.setEnabled(False)
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
app.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling)
# app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application