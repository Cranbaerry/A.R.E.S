import qdarkstyle, os, sys, requests, urllib, json, re, threading, queue, traceback, tempfile, shutil, time, subprocess, hashlib
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from datetime import datetime
from PyQt5.QtCore import *
from generatehtml import makehtml
from winregistry import WinRegistry
DEBUGG = True
class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(836, 602)
        uic.loadUi('untitled.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        VERSION = "6.5"
        self.UPDATEBUTTON = self.findChild(QtWidgets.QPushButton, 'UPDATEBUTTON')
        self.UPDATEBUTTON.hide()
        try:
            ss = requests.get("https://pastebin.com/raw/w3f0jC9P", timeout=10).text
            if VERSION != ss:
                self.UPDATEBUTTON.show()
                self.UPDATEBUTTON.clicked.connect(self.UPDATEPUSHED)
                self.UPDATEBUTTON.setStyleSheet("background-color: red; border: 3px solid black;")
        except:
            pass
        self.updateimage("https://i.ibb.co/3pHS4wB/Default-Placeholder.png")
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
        with WinRegistry() as client:
            self.UDir = client.read_entry(r"HKEY_CURRENT_USER\Software\Unity Technologies\Installer\Unity", "Location x64").value
            #print(self.UDir)
            self.VRCDir = client.read_entry(r"HKEY_CURRENT_USER\Software\VRChat", "").value
            self.VRCDir = self.VRCDir+r"\AvatarLog"
            #print(self.VRCDir)

        self.Settings["Avatar_Folder"] = self.VRCDir
        with open("Settings.json", "w+") as s:
            s.write(json.dumps(self.Settings, indent=4))
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
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
        self.searchapibutton = self.findChild(QtWidgets.QPushButton, 'searchapibutton')
        self.searchapibutton.clicked.connect(self.callapiforavis)
        if self.Settings["ALLOW_API_UPLOAD"]:
            kk = requests.get(url="https://pastebin.com/raw/8DzGLek5").text
            self.domain = kk
            self.searchapibutton.setEnabled(True)
            threading.Thread(target=self.HWIDLaunch, args={}).start()
            if os.path.exists(self.LogFolder + "/Log.txt"):
                threading.Thread(target=self.startuploads, args={}).start()
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
        self.LoadButton = self.findChild(QtWidgets.QPushButton, 'LoadButton')
        self.LoadButton.clicked.connect(self.loadavatar0)
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
        self.HotswapButton.clicked.connect(self.HotSwap1)
        self.DeleteLogButton = self.findChild(QtWidgets.QPushButton, 'DeleteLogButton')
        self.DeleteLogButton.clicked.connect(self.DeleteLogs)
        self.UnityButton = self.findChild(QtWidgets.QPushButton, 'UnityButton')
        self.UnityButton.clicked.connect(self.OpenUnity)
        self.apibox = self.findChild(QtWidgets.QCheckBox, 'apibox')
        self.apibox.clicked.connect(self.updateapi)
        self.NSFWcheckbox = self.findChild(QtWidgets.QCheckBox, 'PrivateBox_2')
        self.NSFWcheckbox.hide()
        self.Violencecheckbox = self.findChild(QtWidgets.QCheckBox, 'PublicBox_2')
        self.Violencecheckbox.hide()
        self.Gorecheckbox = self.findChild(QtWidgets.QCheckBox, 'HTMLBox_2')
        self.Gorecheckbox.hide()
        self.Othernsfwcheckbox = self.findChild(QtWidgets.QCheckBox, 'TagSettings_2')
        self.Othernsfwcheckbox.hide()
        self.Tagscheckbox = self.findChild(QtWidgets.QCheckBox, 'TagSettings')
        self.Tagscheckbox.clicked.connect(self.tagstogs)
        self.HTMLBox = self.findChild(QtWidgets.QCheckBox, 'HTMLBox')
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

    def HWIDLaunch(self):
        self.HWID = str(subprocess.check_output('wmic csproduct get uuid'), 'utf-8').split('\n')[1].strip()
        print(self.HWID)
        self.HHWID = hashlib.md5(self.HWID.encode()).hexdigest()
        print(self.HHWID)
        headers = {"Content-Type": "application/json",
                   "Bypass-Tunnel-Reminder": "bypass"}
        try:
            response = requests.get(f'https://{self.domain}/checkin/'+self.HHWID, headers=headers, timeout=5)
            print(response.text)
        except Exception as E:
            print(E)


    def HotSwap1(self):
        self.HotswapButton.setEnabled(False)
        threading.Thread(target=self.HotSwap, args={}).start()


    def ErrorLog(self, Log):
        with open("Error.log", "a+") as e:
            e.writelines(Log+"\n")
    
    def UPDATEPUSHED(self):
        os.startfile("https://github.com/LargestBoi/AvatarLogger-GUI/releases")

    def tagstogs(self):
        if self.Tagscheckbox.isChecked():
            self.NSFWcheckbox.show()
            self.Violencecheckbox.show()
            self.Gorecheckbox.show()
            self.Othernsfwcheckbox.show()
        else:
            self.NSFWcheckbox.setChecked(False)
            self.Violencecheckbox.setChecked(False)
            self.Gorecheckbox.setChecked(False)
            self.Othernsfwcheckbox.setChecked(False)
            self.NSFWcheckbox.hide()
            self.Violencecheckbox.hide()
            self.Gorecheckbox.hide()
            self.Othernsfwcheckbox.hide()



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
        sys.exit()

    def upload1(self):
        global avis
        global q
        q = queue.Queue()
        if not os.path.exists("uploaded.txt"):
            with open("uploaded.txt", "a") as p:
                pass
        pubpath = self.LogFolder + "/Log.txt"
        with open("uploaded.txt", "r+", errors="ignore") as k:
            avis = k.read()
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)\nTags: (.*)"
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
            "Version": x[10],
            "Tags": x[11]
        }
        url = "http://" + self.domain + "/upload"
        headers = {"Content-Type": "application/json",
                   "Bypass-Tunnel-Reminder": "bypass"}
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
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }

        try:
            response = requests.get(f'https://{self.domain}/status', headers=headers, timeout=5)
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
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }

        data = requests.request("GET", url, data=payload, headers=headers)
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.leftbox.setPixmap(pixmap)
        self.NextButton.show()
        self.BackButton.show()

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

    def loadavatar0(self):
        self.loadavatars(True)
    def loadavatars(self, makehtmll=False):
        self.leftbox.show()
        self.Status.show()
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nVersion:(.*)\nTags: (.*)"
        self.LogFolder = self.Settings["Avatar_Folder"]
        try:
            with open(self.LogFolder + "\Log.txt", "r+", errors="ignore") as s:
                self.Logs = s.read()
                self.Avatars = re.findall(pat, self.Logs)
            self.Avatars = sorted(self.Avatars, key=self.sortFunction, reverse=True)
            if self.HTMLBox.isChecked():
                if makehtmll:
                    threading.Thread(target=makehtml, args={json.dumps(self.Avatars),}).start()

            self.MaxAvatar = len(self.Avatars)
            self.AvatarIndex = 0
            self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
            self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))
            self.AvatarUpdate(0)
        except:
            self.RawData.setPlainText("INVALID LOG FOLDER")
            if DEBUGG:
                self.ErrorLog(traceback.print_exc())

    def Cleantext(self, data):
        klean = f"""Time Detected:{datetime.utcfromtimestamp(int(data[0])).strftime('%Y-%m-%d %H:%M:%S')}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nVersion:{data[10]}\nTags:{data[11]}"""
        return klean

    def Search(self):
        self.loadavatars()
        self.filter()

    def sortFunctionapi(self, value):
        kk = str(value[0])
        value[0] = kk
        return value[0]
    def callapiforavis(self):
        seardata = {
            "author": False,
            "avatarid": False,
            "name": False,
            "searchterm": "string"
        }
        self.lineEdit = self.findChild(QtWidgets.QLineEdit, 'lineEdit')
        seardata["searchterm"] = self.lineEdit.text()
        if self.AvatarNameRB.isChecked():
            seardata["name"] = True
        if self.AvatarAuthorRB.isChecked():
            seardata["author"] = True
        if self.AvatarIDRB.isChecked():
            seardata["avatarid"] = True
        headers = {
            'accept': 'application/json',
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.164 Safari/537.36',
            'Content-Type': 'application/json',
            "Bypass-Tunnel-Reminder": "bypass"
        }

        data = json.dumps(seardata)

        response = requests.post(f'http://{self.domain}/search', headers=headers, data=data)
        kk = json.loads(response.text)
        #print(response.text)
        self.Avatars = kk
        self.Avatars = sorted(self.Avatars, key=self.sortFunctionapi, reverse=True)
        #print(self.Avatars)
        self.filter()

    def filter(self):
        #print(json.dumps(self.Avatars))
        allowed = []
        if self.PrivateBox.isChecked():
            allowed.append("private")
        if self.PublicBox.isChecked():
            allowed.append("public")
        AvatarsS = []
        self.lineEdit = self.findChild(QtWidgets.QLineEdit, 'lineEdit')
        self.searched = self.lineEdit.text()
        if self.AvatarNameRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[2]).lower():
                    if x[9] in allowed:
                        AvatarsS.append(x)

        if self.AvatarAuthorRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[5]).lower():
                    if x[9] in allowed:
                        AvatarsS.append(x)

        if self.AvatarIDRB.isChecked():
            for x in self.Avatars:
                if str(self.searched).lower() in str(x[1]).lower():
                    if x[9] in allowed:
                        AvatarsS.append(x)
        try:
            #raise ValueError("TEST ERROR")
            if self.Tagscheckbox.isChecked():
                if self.NSFWcheckbox.isChecked() or self.Violencecheckbox.isChecked() or self.Gorecheckbox.isChecked() or self.Othernsfwcheckbox.isChecked():
                    newavis = []
                    if self.NSFWcheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_sex").lower() in str(x[11]).lower():
                                    newavis.append(x)
                            except:
                                self.ErrorLog(traceback.format_exc())
                    if self.Violencecheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_violence").lower() in str(x[11]).lower():
                                    newavis.append(x)
                            except:
                                self.ErrorLog(traceback.format_exc())
                    if self.Gorecheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_gore").lower() in str(x[11]).lower():
                                    newavis.append(x)
                            except:
                                self.ErrorLog(traceback.format_exc())
                    if self.Othernsfwcheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_other").lower() in str(x[11]).lower():
                                    newavis.append(x)
                            except:
                                self.ErrorLog(traceback.format_exc())
                    print(json.dumps(newavis))
                    AvatarsS = newavis
                    #AvatarsS = list(set(newavis))
                    print(json.dumps(AvatarsS))
        except:
            self.ErrorLog(traceback.format_exc())


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
        if self.HTMLBox.isChecked():
            threading.Thread(target=makehtml, args={json.dumps(self.Avatars),}).start()
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))

    def DownVRCAT(self, url, dir1):
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
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

    def UnityLauncher(self):
        os.system(rf'"{self.UDir}/Editor/Unity.exe" -ProjectPath HSB')
        self.UnityButton.setEnabled(True)

    def OpenUnity(self):
        self.UnityButton.setEnabled(False)
        if os.path.isdir(tempfile.gettempdir()+"\\DefaultCompany\\HSB"):
            shutil.rmtree(tempfile.gettempdir()+"\\DefaultCompany\\HSB", ignore_errors=True)
        if os.path.isdir("HSB"):
            try:
                shutil.rmtree("HSB", ignore_errors=True)
            except:
                pass
            try:
                os.rmdir("HSB")
            except:
                pass
        os.mkdir('HSB')
        os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
        threading.Thread(target=self.UnityLauncher, args={}).start()

    def HotSwap(self):
        try:
            self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'progressBar')
            self.ProgBar.setEnabled(True)
            #self.ProjName1 = self.findChild(QtWidgets.QLineEdit, 'ProjName')
            #self.ProjName = self.ProjName1.text()
            self.ProgBar.setValue(10)
            self.ProjPath = tempfile.gettempdir()+"\\DefaultCompany\\HSB\\custom.vrca"
            os.chdir("HOTSWAP")
            self.ProgBar.setValue(20)
            os.system("HOTSWAP.exe d "+self.ProjPath)
            with open("decompressedfile", "rb") as f:
                f = f.read()
            self.NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
            self.ProgBar.setValue(30)
            if os.path.exists("decompressedfile"):
                os.remove("decompressedfile")
            os.chdir("..")
            self.ProgBar.setValue(40)
            self.DownVRCAT(self.Avatars[self.AvatarIndex][6], "HOTSWAP/Avatar.vrca")
            os.chdir("HOTSWAP")
            if os.path.exists("custom.vrca"):
                os.remove("custom.vrca")
            self.ProgBar.setValue(50)
            os.system("HOTSWAP.exe d Avatar.vrca")
            self.ProgBar.setValue(60)
            self.oldid = self.Avatars[self.AvatarIndex][1]
            self.ReplaceID(self.oldid, self.NewID)
            self.ProgBar.setValue(70)
            os.system("HOTSWAP.exe c")
            self.ProgBar.setValue(80)
            if os.path.exists("Avatar.vrca"):
                os.remove("Avatar.vrca")
            if os.path.exists("decompressedfile"):
                os.remove("decompressedfile")
            if os.path.exists("decompressedfile1"):
                os.remove("decompressedfile1")
            if os.path.exists(self.ProjPath):
                os.remove(self.ProjPath)
            self.ProgBar.setValue(90)
            shutil.move("custom.vrca", self.ProjPath)
            self.ProgBar.setValue(100)
            os.chdir("..")
            self.ProgBar.setEnabled(False)
            #self.ProjName1.setText("COMPLETE")
            time.sleep(10)
            self.ProgBar.setValue(0)
            self.HotswapButton.setEnabled(True)
        except:
            traceback.print_exc()

    def DeleteLogs(self):
        if os.path.exists(self.LogFolder + "/Log.txt"):
            os.remove(self.LogFolder + "/Log.txt")

app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
#app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application