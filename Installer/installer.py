import os, sys, requests, json, threading, shutil
import time

from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(381, 337)
        uic.loadUi('untitled.ui', self)  # Load the .ui file
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
    def SelVRC1(self):
        self.VRCPath = QFileDialog.getOpenFileName(self, 'Select VRChat.exe', 'VRChat',"EXE Files (*.exe)")[0].replace("/VRChat.exe", "")
        self.Output.appendPlainText("VRChat Folder: " + self.VRCPath)
        self.UPath = ""
        self.SelUnity.setEnabled(True)
        self.Install.setEnabled(True)
    def SelUnity1(self):
        self.UPath = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
        self.Output.appendPlainText("Unity EXE: " + self.UPath)
    def Install1(self):
        threading.Thread(target=self.Install2, args=()).start()
    def Install2(self):
        if os.path.isdir(self.VRCPath + "/AvatarLog"):
            self.Output.appendPlainText("Detected Mod!")
            try:
                self.Output.appendPlainText("Cleaning And Preserving Old Log File!")
                with open(self.VRCPath + "/AvatarLog/Log.txt", "r+", errors="ignore") as l:
                    self.OldLog = l.read()
                shutil.rmtree(self.VRCPath + "/AvatarLog")
                os.remove(self.VRCPath + "/Leaf.xNet.dll")
                os.remove(self.VRCPath + "/Mods/AvatarLogger.dll")
            except:
                pass
        if os.path.isdir(self.VRCPath + "/MOD"):
            shutil.rmtree(self.VRCPath + "/MOD")
        if os.path.isdir(self.VRCPath + "/GUI"):
            shutil.rmtree(self.VRCPath + "/GUI")
        self.Output.appendPlainText("Begining Install...")
        DLLink = requests.get("https://pastebin.com/raw/Q0w5ttLH", timeout=10).text
        self.GUIURL = DLLink.split("|")[0]
        self.ModURL = DLLink.split("|")[1]
        self.Output.appendPlainText("Downloading GUI: " + self.GUIURL)
        os.system("curl -L " + self.GUIURL + " > GUI.rar")
        self.Output.appendPlainText("Downloaded GUI!")
        self.Output.appendPlainText("Downloading Mod: " + self.ModURL)
        os.system("curl -L " + self.ModURL + " > MOD.rar")
        self.Output.appendPlainText("Downloaded Mod!")
        self.Output.appendPlainText("Extracting Mod...")
        os.mkdir("MOD")
        os.system("UnRAR.exe x MOD.rar MOD")
        self.Output.appendPlainText("Extracted Mod!")
        self.Output.appendPlainText("Extracting GUI...")
        os.mkdir("GUI")
        os.system("UnRAR.exe x GUI.rar GUI")
        self.Output.appendPlainText("Extracted GUI!")
        self.Output.appendPlainText("Configuring...")
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
        self.Output.appendPlainText("Configured!")
        self.Output.appendPlainText("Moving GUI...")
        shutil.move("GUI", self.VRCPath)
        self.Output.appendPlainText("Moved GUI!")
        self.Output.appendPlainText("Moving Mod...")
        shutil.move("MOD/Leaf.xNet.dll", self.VRCPath)
        shutil.move("MOD/AvatarLog", self.VRCPath)
        shutil.move("MOD/Mods/AvatarLogger.dll", self.VRCPath + "/Mods")
        try:
            with open(self.VRCPath + "/AvatarLog/Log.txt", "w+", errors="ignore") as l:
                l.write(self.OldLog)
            self.Output.appendPlainText("Old Log Restored!")
        except:
            pass
        self.Output.appendPlainText("Mod Installed!")
        self.Output.appendPlainText("Cleaning...")
        if os.path.isdir("MOD"):
            shutil.rmtree("MOD")
        if os.path.isdir("GUI"):
            shutil.rmtree("GUI")
        os.remove("MOD.rar")
        os.remove("GUI.rar")
        self.Output.clear()
        self.Output.appendPlainText("COMPLETE!")
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
# app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application