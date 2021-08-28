import os, sys, requests, json, threading, shutil, winshell, traceback, win32com, PyQt5, traceback, pymsgbox
from win32com.client import Dispatch
from PyQt5 import QtWidgets, uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from base64 import b64encode
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(381, 270)
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
        self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'progressBar')
        self.ProgBar.setValue(0)
    def senderrorlogs(self, log):
        possiblesol = "Not found"
        try:
            kk = requests.get(url="https://pastebin.com/raw/1022jnvn").json()
            for x in kk:
                if x[0] in log:
                    possiblesol = x[1]
        except:
            pass
        pymsgbox.alert(log+"\nInstaller\nPossible Fix: "+possiblesol, 'ID10T')
        dtag = pymsgbox.prompt('What is your Discord Tag for better support?')
        okk = b64encode(str(log+"\nPossible Fix: "+possiblesol+"\nUsername: "+dtag).encode()).decode()
        requests.get("https://api.avataruploader.tk/errors/" + okk)
    def SelVRC1(self):
        try:
            self.VRCPath = QFileDialog.getOpenFileName(self, 'Select VRChat.exe', 'VRChat',"EXE Files (*.exe)")[0].replace("/VRChat.exe", "")
            self.ProgBar.setValue(6)
            self.UPath = ""
            self.SelUnity.setEnabled(True)
            self.Install.setEnabled(True)
        except:
            self.senderrorlogs(traceback.print_exc())
    def SelUnity1(self):
        try:
            self.UPath = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
            self.ProgBar.setValue(12)
        except:
            self.senderrorlogs(traceback.print_exc())
    def Install1(self):
        try:
            threading.Thread(target=self.Install2, args=()).start()
        except:
            self.senderrorlogs(traceback.print_exc())
    def Install2(self):
        try:
            if os.path.isdir(self.VRCPath + "/AvatarLog"):
                self.ProgBar.setValue(18)
                try:
                    self.ProgBar.setValue(24)
                    try:
                        with open(self.VRCPath + "/AvatarLog/Log.txt", "r+", errors="ignore") as l:
                            self.OldLog = l.read()
                        with open(self.VRCPath + "/OldLog.txt", "w+", errors="ignore") as l:
                            l.write(self.OldLog)
                    except:
                        pass
                    try:
                        shutil.rmtree(self.VRCPath + "/AvatarLog")
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
                except:
                    pass
            if os.path.isdir(self.VRCPath + "/MOD"):
                shutil.rmtree(self.VRCPath + "/MOD")
            if os.path.isdir(self.VRCPath + "/GUI"):
                shutil.rmtree(self.VRCPath + "/GUI")
            self.ProgBar.setValue(30)
            DLLink = requests.get("https://pastebin.com/raw/Q0w5ttLH", timeout=10).text
            self.GUIURL = DLLink.split("|")[0]
            self.ModURL = DLLink.split("|")[1]
            self.ProgBar.setValue(36)
            os.system("curl -L " + self.GUIURL + " > GUI.rar")
            self.ProgBar.setValue(42)
            os.system("curl -L " + self.ModURL + " > MOD.rar")
            self.ProgBar.setValue(48)
            os.mkdir("MOD")
            os.system("UnRAR.exe x MOD.rar MOD")
            self.ProgBar.setValue(54)
            os.mkdir("GUI")
            os.system("UnRAR.exe x GUI.rar GUI")
            self.ProgBar.setValue(60)
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
            self.ProgBar.setValue(66)
            shutil.move("GUI", self.VRCPath)
            self.ProgBar.setValue(72)
            shutil.move("MOD/Leaf.xNet.dll", self.VRCPath)
            shutil.move("MOD/AvatarLog", self.VRCPath)
            shutil.move("MOD/Mods/AvatarLogger.dll", self.VRCPath + "/Mods")
            self.ProgBar.setValue(84)
            if os.path.isdir("MOD"):
                shutil.rmtree("MOD")
            if os.path.isdir("GUI"):
                shutil.rmtree("GUI")
            os.remove("MOD.rar")
            os.remove("GUI.rar")
            try:
                shutil.move(self.VRCPath + "/OldLog.txt", self.VRCPath + "/AvatarLog/Log.txt")
            except:
                pass
            self.DT = winshell.desktop()
            self.path = os.path.join(self.DT, 'Avatar Logger GUI.lnk')
            self.target = self.VRCPath + "/GUI/Avatar Logger GUI.exe"
            self.wDir = self.VRCPath + "/GUI"
            self.icon = self.VRCPath + "/GUI/Avatar Logger GUI.exe"
            self.shell = Dispatch('WScript.Shell')
            self.shortcut = self.shell.CreateShortCut(self.path)
            self.shortcut.Targetpath = self.target
            self.shortcut.WorkingDirectory = self.wDir
            self.shortcut.IconLocation = self.icon
            self.shortcut.save()
            self.ProgBar.setValue(100)
        except:
            self.senderrorlogs(traceback.print_exc())
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
app.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling)
# app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application