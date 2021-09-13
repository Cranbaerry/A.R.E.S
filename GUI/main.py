#Importing all reqirements for the GUI and its functionality
import os, sys, requests, json, re, threading, queue, tempfile, shutil, time, hashlib, traceback, pymsgbox, subprocess
from pathlib import Path
from PyQt5 import QtWidgets, uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from datetime import datetime
from generatehtml import makehtml
from base64 import b64encode
#Toggle for debug mode, this will hide the large "OUTDATED" button
debugg = False
Lock = threading.Lock()
#Prep for multiple resolution support
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"
class Ui(QtWidgets.QMainWindow):
    #Initialises all buttons and base functions for the script
    def __init__(self):
        #Prepares for UI launch
        super(Ui, self).__init__()
        #Locks particualr window size
        self.setFixedSize(836, 602)
        #Load the .ui file
        uic.loadUi('GUI.ui', self)
        #Prepares to log what is done within the current user session
        with open("latest.log", "w+", errors="ignore") as k:
            k.write("")
        #Show the GUI
        self.show()
        #Close conflicting software
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
        #Sets version number to later be checked with the pastebin
        VERSION = "7.7"
        #Prepare the "Special Thanks" mox to contain text
        self.ST = self.findChild(QtWidgets.QPlainTextEdit, 'SpecialThanks')
        #Attempt to get latest "Special Thanks" from pastebin and populate box with a 10 second timeout
        try:
            SPTX = requests.get("https://pastebin.com/raw/vayK7gC2", timeout=10).text
        except:
            SPTX = "Couldn't Connect!"
        self.ST.appendPlainText(SPTX)
        #Declares update button
        self.UPDATEBUTTON = self.findChild(QtWidgets.QPushButton, 'UPDATEBUTTON')
        #Hides button by default
        self.UPDATEBUTTON.hide()
        #Run version check with pastebin
        try:
            ss = requests.get("https://pastebin.com/raw/w3f0jC9P", timeout=10).text
            if VERSION != ss:
                self.UPDATEBUTTON.show()
                self.UPDATEBUTTON.clicked.connect(self.UPDATEPUSHED)
                self.UPDATEBUTTON.setStyleSheet("background-color: red; border: 3px solid black;")
            if debugg:
                self.UPDATEBUTTON.hide()
        except:
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
            pass
        #Set default image preview
        self.updateimage("https://i.ibb.co/3pHS4wB/Default-Placeholder.png")
        #Initiate "Settings.json" file
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
        #Read from "Settings.json" file
        with open("Settings.json", "r+") as s:
            self.Settings = json.loads(s.read())
            self.VRCDir = self.Settings["Avatar_Folder"]
            self.UDir = self.Settings["Unity_Exe"]
        #Declares the LogDirectoy label and sets text
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
        #Loading the ModConfig file from the avatr logger mod
        self.LogFolder = self.Settings["Avatar_Folder"]+"\\AvatarLog"
        self.ModConfig = self.Settings["Avatar_Folder"]+"\\AvatarLog\\Config.json"
        try:
            with open(self.ModConfig, "r+") as s:
                self.ModSettings = json.loads(s.read())
        except:
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
            pass
        self.DirLabel = self.findChild(QtWidgets.QLabel, 'DirLabel')
        # Declares the API label and sets text
        self.APIL = self.findChild(QtWidgets.QLabel, 'APILabel')
        #Gets and sets LogSize of avatar logger
        self.LogSize = self.findChild(QtWidgets.QLabel, 'LogSize')
        if os.path.exists(self.LogFolder + "/Log.txt"):
            self.LSize = os.path.getsize(self.LogFolder + "/Log.txt")
            self.LogSize.setText(str(round(self.LSize/(1024*1024)))+"MB")
        #Declares search API button
        self.searchapibutton = self.findChild(QtWidgets.QPushButton, 'searchapibutton')
        self.searchapibutton.clicked.connect(self.callapiforavis)
        #Loads mod setting from VRC mod
        if self.ModSettings["SendToAPI"]:
            kk = requests.get(url="https://pastebin.com/raw/8DzGLek5").text
            self.domain = kk
            self.searchapibutton.setEnabled(True)
            threading.Thread(target=self.HWIDLaunch, args={}).start()
            #self.HWIDLaunch()
            self.APIL.setText("API Enabled")
            if os.path.exists(self.LogFolder + "/Log.txt"):
                threading.Thread(target=self.startuploads, args={}).start()
        #Declares all other buttons, checkboxes toggles, textboxes and alot more!
        self.DirLabel.setText("CurrentDirectory: " + self.Settings["Avatar_Folder"])
        self.LoadButton = self.findChild(QtWidgets.QPushButton, 'LoadButton')
        self.LoadButton.clicked.connect(self.loadavatar0)
        self.BackButton = self.findChild(QtWidgets.QPushButton, 'BackButton')
        self.BackButton.clicked.connect(self.Back)
        self.BackButton.setEnabled(False)
        self.browserview = self.findChild(QtWidgets.QPushButton, 'browserview')
        self.browserview.clicked.connect(self.browserview1)
        self.browserview.setEnabled(False)
        self.SearchButton = self.findChild(QtWidgets.QPushButton, 'SearchButton')
        self.SearchButton.clicked.connect(self.Search)
        self.SearchButton.setEnabled(False)
        self.CleanExitButton = self.findChild(QtWidgets.QPushButton, 'CleanExitButton')
        self.CleanExitButton.clicked.connect(self.CleanExit)
        self.NextButton = self.findChild(QtWidgets.QPushButton, 'NextButton')
        self.NextButton.clicked.connect(self.Next)
        self.NextButton.setEnabled(False)
        self.AvatarNameRB = self.findChild(QtWidgets.QRadioButton, 'AvatarNameRB')
        self.AvatarAuthorRB = self.findChild(QtWidgets.QRadioButton, 'AvatarAuthorRB')
        self.AvatarIDRB = self.findChild(QtWidgets.QRadioButton, 'AvatarIDRB')
        self.Status = self.findChild(QtWidgets.QLabel, 'Status')
        self.PrivateBox = self.findChild(QtWidgets.QCheckBox, 'PrivateBox')
        self.LCDPANEL = self.findChild(QtWidgets.QLCDNumber, 'lcdNumber')
        self.UsrTotal = self.findChild(QtWidgets.QLCDNumber, 'UsrTotal')
        self.PublicBox = self.findChild(QtWidgets.QCheckBox, 'PublicBox')
        self.DLVRCAButton = self.findChild(QtWidgets.QPushButton, 'DLVRCAButton')
        self.DLVRCAButton.clicked.connect(self.DownVRCA)
        self.DLVRCAButton.setEnabled(False)
        self.HotswapButton = self.findChild(QtWidgets.QPushButton, 'HotswapButton')
        self.HotswapButton.clicked.connect(self.Hotswap)
        self.HotswapButton.setEnabled(False)
        self.LoadVRCAButton = self.findChild(QtWidgets.QPushButton, 'LoadVRCAButton')
        self.LoadVRCAButton.clicked.connect(self.LoadVRCA)
        self.DeleteLogButton = self.findChild(QtWidgets.QPushButton, 'DeleteLogButton')
        self.DeleteLogButton.clicked.connect(self.DeleteLogs)
        self.SetUserButton = self.findChild(QtWidgets.QPushButton, 'SetUserButton')
        self.SetUserButton.clicked.connect(self.SetUser)
        self.VRCAExtractButton = self.findChild(QtWidgets.QPushButton, 'VRCAExtractButton')
        self.VRCAExtractButton.clicked.connect(self.VRCAExtract)
        self.VRCAExtractButton.setEnabled(False)
        self.SetUserBox = self.findChild(QtWidgets.QLineEdit, 'SetUserBox')
        self.SetUserBox.setText(self.ModSettings["Username"])
        self.UnityButton = self.findChild(QtWidgets.QPushButton, 'UnityButton')
        self.UnityButton.clicked.connect(self.OpenUnity)
        self.ToggleAPIButton = self.findChild(QtWidgets.QPushButton, 'ToggleAPIButton')
        self.ToggleAPIButton.clicked.connect(self.updateapi)
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
        self.Instructions = self.findChild(QtWidgets.QPlainTextEdit, 'Instructions')
        self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'progressBar')
        #Fetch user statistics from API
        try:
            kk = requests.get(url="https://api.avataruploader.tk/user/" + self.ModSettings["Username"]).text
            self.UsrTotal.display(int(kk))
        except:
            pass
    #Allows the program to commit die
    def CleanExit(self):
        #Attempts to kill all avatar logger gui instances and any other conflicting apps
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
    #Allows us to update the "Console Output" feild in the GUI
    def updateconsole(self, word):
        #Takes passed args and creates string to append to output box and txt files
        try:
            self.Instructions.appendPlainText(word)
            with open("outputlogs.txt", "a+", errors="ignore") as f:
                f.writelines(word+"\n")
        except Exception as e:
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
    #Calls thread to set user
    def SetUser(self):
        threading.Thread(target=self.SetUser1, args=()).start()
    #Allows the user to identify themselves when connecting to the API
    def SetUser1(self):
        #Disables the button to avoid button spam
        self.SetUserButton.setEnabled(False)
        #Gets text box contents and removes special chars
        self.UserText = self.SetUserBox.text().encode().decode("ascii", errors="ignore")
        #Writes to mod settings files
        self.ModSettings["Username"] = self.UserText
        with open(self.ModConfig, "w+") as s:
            s.write(json.dumps(self.ModSettings, indent=4))
        self.SetUserBox.setText("Username Set!")
        #Updates console
        self.updateconsole("Username Set")
        time.sleep(3)
        self.SetUserBox.setText(self.UserText)
        self.SetUserButton.setEnabled(True)
    #Creates user identifier for API
    def HWIDLaunch(self):
        #Gets username and sends to API via request
        self.HHWID = self.ModSettings["Username"]
        headers = {"Content-Type": "application/json",
                   "Bypass-Tunnel-Reminder": "bypass"}
        try:
            response = requests.get(f'https://{self.domain}/checkin/'+self.HHWID, headers=headers, timeout=5)
            self.updateconsole("SENT USERID :"+str(datetime.now()))
        except Exception as E:
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
    #Links url to discord server to outdated button
    def UPDATEPUSHED(self):
        #So uh... while your here JOIN THE DISCORD
        os.startfile("https://discord.gg/dhSdMsfgWe")
    #Indentifies all tags and functions to work within search
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
    #Function for extraction of a VRCA via asset ripper
    def VRCAExtract(self):
        #All in try statemnt incase I or the user fucks this up
        try:
            #Select folder to save extracted files
            self.APSF = QFileDialog.getExistingDirectory(self, "Select Directory")
            #If its a loaded vrca do the correct preperation
            if self.Avatars[self.AvatarIndex][2] == "VRCA":
                self.filepath = self.Avatars[self.AvatarIndex][6]
                self.keepvrca = True
                self.filepatha = len(self.filepath.split("/"))
                self.pathname = self.filepath.split("/")[self.filepatha - 1].replace(".vrca", "")
            #If it aint download the vrca in question
            else:
                self.filepath = f'{self.Avatars[self.AvatarIndex][2].encode().decode("ascii", errors="ignore")}.vrca'
                self.DownVRCAT(self.Avatars[self.AvatarIndex][6], f'AssetRipperConsole_win64(ds5678)/{self.filepath}')
                self.keepvrca = False
                self.pathname = self.Avatars[self.AvatarIndex][2].encode().decode("ascii", errors="ignore")
            #Enter the asset ripper
            os.chdir("AssetRipperConsole_win64(ds5678)")
            #Extract all assets
            os.system(f'AssetRipperConsole.exe "{self.filepath}" DLL -q')
            os.chdir("Ripped/Assets")
            #Remove redundant files
            shutil.rmtree("Scripts")
            #Disable shaders but retain names
            os.chdir("Shader")
            SF = Path(os.getcwd())
            for f in SF.iterdir():
                if f.is_file() and f.suffix in ['.meta']:
                    f.rename(f.with_suffix('.meta.txt'))
            for f in SF.iterdir():
                if f.is_file() and f.suffix in ['.shader']:
                    f.rename(f.with_suffix('.shader.txt'))
            os.chdir("..")
            os.chdir("..")
            os.chdir("..")
            #Rename folder
            os.rename("Ripped", self.pathname)
            #If a vrca was loaded DON'T delete it
            if not self.keepvrca:
                os.remove(self.filepath)
            #Move output files to desired directory
            shutil.move(self.pathname, self.APSF)
            #Exit asset ripper
            os.chdir("..")
        except:
            #If it fails let us know and log it
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
    #If the API is toggled via the GUI
    def updateapi(self):
        #If the API is on disable it and quit the app
        if self.ModSettings["SendToAPI"]:
            self.ModSettings["SendToAPI"] = False
            with open(self.ModConfig, "w+") as s:
                s.write(json.dumps(self.ModSettings, indent=4))
            try:
               os.system('taskkill /F /im "Avatar Logger GUI.exe"')
            except:
               pass
        #If the API was off turn it on and quit the app
        self.ModSettings["SendToAPI"] = True
        with open(self.ModConfig, "w+") as s:
            s.write(json.dumps(self.ModSettings, indent=4))
        try:
            os.system('taskkill /F /im "Avatar Logger GUI.exe"')
        except:
            pass
    #Uploads avis to the API
    def upload1(self):
        #Set global vars
        global avis
        global q
        #Create an avi queue
        q = queue.Queue()
        #Make the uploaded file if it doesnt exist
        if not os.path.exists("uploaded.txt"):
            with open("uploaded.txt", "a") as p:
                pass
        #Get log file
        pubpath = self.LogFolder + "/Log.txt"
        #read what has already been uploaded
        with open("uploaded.txt", "r+", errors="ignore") as k:
            avis = k.read()
        #Create upload json
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nUnity Version:(.*)\nPlatform:(.*)\nAPI Version:(.*)\nVersion:(.*)\nTags: (.*)"
        #Create array to upload
        with open(pubpath, "r+", errors="ignore") as g:
            kk = g.read()
            ho = re.findall(pat, kk)
            for x in ho:
                q.put(x)
    #Creates json to upload to our API
    def upload(self, data):
        #Set data var
        x = data
        #Get all necassacry information in json
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
            "UnityVersion": x[10],
            "Platform": x[11],
            "APIVersion": x[12],
            "Version": x[13],
            "Tags": x[14]
        }
        #Uploads to our domain
        url = "http://" + self.domain + "/upload"
        headers = {"Content-Type": "application/json",
                   "Bypass-Tunnel-Reminder": "bypass",
                   "User-Agent" : self.ModSettings["Username"]}
        if str(x[1]) not in avis:
            try:
                response = requests.request("POST", url, json=hooh, headers=headers)
            except:
                self.senderrorlogs(traceback.format_exc())
                with open("latest.log", "a+", errors="ignore") as k:
                    k.writelines(traceback.format_exc() + "\n\n")
                pass
            with open("uploaded.txt", "a+", errors="ignore") as k:
                k.writelines(x[1] + "\n")
            Lock.acquire()
            #Updates LCD panel on success
            ff = self.LCDPANEL.value()
            self.LCDPANEL.display(int(ff)+1)
            try:
                if '"UPLOADED"' in response.text:
                    ff = self.UsrTotal.value()
                    self.UsrTotal.display(int(ff)+1)
            except:
                pass
            Lock.release()
    #Begins avi uploading
    def startuploads(self):
        #Update the console
        self.updateconsole("Starting Upload")
        #Prepare headers
        headers = {
            'accept': 'application/json',
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
        #Try to upload while threaded
        try:
            response = requests.get(f'https://{self.domain}/status', headers=headers, timeout=5)
            if "ONLINE" in response.text:
                self.upload1()
                tt = 15
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
            #If error log it and send it to us
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
            print("SERVER OFFLINE")
    #Sets preview image
    def updateimage(self, url):
        #Locates image label
        self.leftbox = self.findChild(QtWidgets.QLabel, 'PreviewImage')
        #Creates empty payload
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
        #Requests data and sets image
        data = requests.request("GET", url, data=payload, headers=headers)
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.leftbox.setPixmap(pixmap)
        self.NextButton.show()
        self.BackButton.show()
    #Say 0 if allg
    def sortFunction(self, value):
        return value[0]
    #Moves back in avi list
    def Back(self):
        #Stop from breaking avi index with invalid value
        if self.AvatarIndex == 0:
            return
        self.BackButton.hide()
        self.NextButton.hide()
        self.AvatarIndex -= 1
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))
    #Moves forward in avi list
    def Next(self):
        # Stop from breaking avi index with invalid value
        if self.AvatarIndex + 1 >= self.MaxAvatar:
            return
        self.NextButton.hide()
        self.BackButton.hide()
        self.AvatarIndex += 1
        self.AvatarUpdate(self.AvatarIndex)
        self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))
    #Updates general avatr information
    def AvatarUpdate(self, AVIndex):
        #Unhides particular buttons as an avi has now been loaded
        self.SearchButton.setEnabled(True)
        self.DLVRCAButton.setEnabled(True)
        self.VRCAExtractButton.setEnabled(True)
        self.HotswapButton.setEnabled(True)
        self.browserview.setEnabled(True)
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
    #Sets load value
    def loadavatar0(self):
        self.loadavatars(True)
    #Loads avatr log file
    def loadavatars(self, makehtmll=False):
        #Unhides particular buttons as an avi has now been loaded
        self.SearchButton.setEnabled(True)
        self.DLVRCAButton.setEnabled(True)
        self.VRCAExtractButton.setEnabled(True)
        self.HotswapButton.setEnabled(True)
        self.browserview.setEnabled(True)
        self.BackButton.setEnabled(True)
        self.NextButton.setEnabled(True)
        self.leftbox.show()
        self.Status.show()
        #Sets json index
        pat = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nAsset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nRelease Status:(.*)\nUnity Version:(.*)\nPlatform:(.*)\nAPI Version:(.*)\nVersion:(.*)\nTags: (.*)"
        try:
            #Setup logs to be read
            with open(self.LogFolder + "\Log.txt", "r+", errors="ignore") as s:
                self.Logs = s.read()
                self.Avatars = re.findall(pat, self.Logs)
            #Parses log files
            self.Avatars = sorted(self.Avatars, key=self.sortFunction, reverse=True)
            self.MaxAvatar = len(self.Avatars)
            self.AvatarIndex = 0
            self.resultsbox = self.findChild(QtWidgets.QLabel, 'resultsbox')
            self.resultsbox.setText("LOADED: " + str(self.AvatarIndex + 1) + "/" + str(self.MaxAvatar))
            self.AvatarUpdate(0)
        except:
            #If somthing breaks log it and let us know
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
            self.RawData.setPlainText("INVALID LOG FOLDER")
    #Threads creation and display of the browser view
    def browserview1(self):
        threading.Thread(target=makehtml, args={json.dumps(self.Avatars), }).start()
    #Cleans text for preview
    def Cleantext(self, data):
        try:
            Klean = f"""Time Detected:{datetime.utcfromtimestamp(int(data[0])).strftime('%Y-%m-%d %H:%M:%S')}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nUnity Version:{data[10]}\nPlatform:{data[11]}\nAPI Version:{data[12]}\nVersion:{data[13]}\nTags:{data[14]}"""
        except:
            Klean = f"""Time Detected:{data[0]}\nAvatar ID:{data[1]}\nAvatar Name:{data[2]}\nAvatar Description:{data[3]}\nAuthor ID:{data[4]}\nAuthor Name:{data[5]}\nAsset URL:{data[6]}\nImage URL:{data[7]}\nThumbnail URL:{data[8]}\nRelease Status:{data[9]}\nUnity Version:{data[10]}\nPlatform:{data[11]}\nAPI Version:{data[12]}\nVersion:{data[13]}\nTags:{data[14]}"""
        return Klean
    #Some search reqirements
    def Search(self):
        self.loadavatars()
        self.filter()
    #Prepares to search
    def sortFunctionapi(self, value):
        kk = str(value[0])
        value[0] = kk
        return value[0]
    #Get avatars from API
    def callapiforavis(self):
        #Enable buttons
        self.SearchButton.setEnabled(True)
        self.DLVRCAButton.setEnabled(True)
        self.VRCAExtractButton.setEnabled(True)
        self.HotswapButton.setEnabled(True)
        self.browserview.setEnabled(True)
        self.BackButton.setEnabled(True)
        self.NextButton.setEnabled(True)
        #Set what is being searched for
        seardata = {
            "author": False,
            "avatarid": False,
            "name": False,
            "searchterm": "string"
        }
        #Get search term and other relevant info
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
        #Search api and fetch results
        response = requests.post(f'http://{self.domain}/search', headers=headers, data=data)
        kk = json.loads(response.text)
        #print(response.text)
        self.Avatars = kk
        self.Avatars = sorted(self.Avatars, key=self.sortFunctionapi, reverse=False)
        #print(self.Avatars)
        self.filter()
    #Filters searches
    def filter(self):
        #Checks what is checked and what isnt
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
                                if str("content_sex").lower() in str(x[14]).lower():
                                    newavis.append(x)
                            except:
                                self.senderrorlogs(traceback.format_exc())
                                with open("latest.log", "a+", errors="ignore") as k:
                                    k.writelines(traceback.format_exc() + "\n\n")
                    if self.Violencecheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_violence").lower() in str(x[14]).lower():
                                    newavis.append(x)
                            except:
                                self.senderrorlogs(traceback.format_exc())
                                with open("latest.log", "a+", errors="ignore") as k:
                                    k.writelines(traceback.format_exc() + "\n\n")
                    if self.Gorecheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_gore").lower() in str(x[14]).lower():
                                    newavis.append(x)
                            except:
                                self.senderrorlogs(traceback.format_exc())
                                with open("latest.log", "a+", errors="ignore") as k:
                                    k.writelines(traceback.format_exc() + "\n\n")
                    if self.Othernsfwcheckbox.isChecked():
                        for x in AvatarsS:
                            try:
                                if str("content_other").lower() in str(x[14]).lower():
                                    newavis.append(x)
                            except:
                                self.senderrorlogs(traceback.format_exc())
                                with open("latest.log", "a+", errors="ignore") as k:
                                    k.writelines(traceback.format_exc() + "\n\n")
                    print(json.dumps(newavis))
                    AvatarsS = newavis
                    #AvatarsS = list(set(newavis))
                    print(json.dumps(AvatarsS))
        except:
            #If somthing breaks log it and alert us
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")


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
    #Sends us your error logs
    def senderrorlogs(self, log):
        #Sets possible solution
        possiblesol = "Not found"
        #Checks pastebin for possible solution
        try:
            kk = requests.get(url="https://pastebin.com/raw/1022jnvn").json()
            for x in kk:
                if x[0] in log:
                    possiblesol = x[1]
        except:
            pass
        #Display error message
        pymsgbox.alert(log+"\nPossible Fix: "+possiblesol, 'ID10T')
        dtag = pymsgbox.prompt('What is your Discord Tag for better support?')
        #Encode in base64 before sending
        okk = b64encode(str(log+"\nPossible Fix: "+possiblesol+"\nUsername: "+dtag).encode()).decode()
        requests.get("https://api.avataruploader.tk/errors/" + okk)
    #Donloads vrca files
    def DownVRCAT(self, url, dir1):
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
        data = requests.request("GET", url, data=payload, headers=headers, stream=True)
        #Writes content to file
        with open(dir1, "wb") as v:
            v.write(data.content)
    #Initiates download of avatars
    def DownVRCA(self):
        #Gets desired save directory
        self.fileName = QFileDialog.getSaveFileName(self, 'Save VRCA', "Avatar", ".vrca")
        #Gets asset url
        self.DLLink = self.Avatars[self.AvatarIndex][6]
        #Joins directories
        self.SaveDir = "".join(self.fileName)
        #Begins avatar downloading
        os.system("curl -L " + self.DLLink + " > " + self.SaveDir)
    #Repace of avtr ids in file
    def ReplaceID(self, oldid, newid):
        #Read decompiled file
        with open("decompressed.vrca", "rb") as f:
            kok = f.read()
        #Replace strings
        kok = kok.replace(bytes(oldid, 'utf-8'), bytes(newid, 'utf-8'))
        #Write to new file
        with open("decompressed1.vrca", "wb") as f:
            f.write(kok)
    #Opens unity project
    def UnityLauncher(self):
        os.system(rf'"{self.UDir}" -ProjectPath HSB')
        self.UnityButton.setEnabled(True)
    #Prepares demo unity project
    def OpenUnity(self):
        #Disables button to avoid spam
        self.UnityButton.setEnabled(False)
        #Remove any traces of HSB if any are present
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
        #Create HSB directory
        os.mkdir('HSB')
        #Extracts HSB to folder
        os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
        #Threads opening of unity project
        threading.Thread(target=self.UnityLauncher, args={}).start()
    #Loads 3rd party vrca
    def LoadVRCA(self):
        #Prompts user to select their vrca file
        self.lvrca = QFileDialog.getOpenFileName(self, 'Open file', '',"VRCA Files (*.vrca)")[0]
        #If they fail to pick a file just cancel
        if self.lvrca == "":
            return
        #Enter hotswap
        os.chdir("HOTSWAP")
        #Decompress file
        os.system(rf'HOTSWAP.exe d "{self.lvrca}"')
        #Read decompressed file
        with open("decompressed.vrca", "rb") as f:
            f = f.read()
        #Find new avatr ID
        self.oldid = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        #Remove decompressed file
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        #Exit hotswap
        os.chdir("..")
        #Set values in avatr index
        self.Avatars = []
        self.Avatars1 = [
            "VRCA",
            "AvtrID",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA",
            "AssetURL",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA",
            "VRCA"
        ]
        self.Avatars1[1] = self.oldid
        self.Avatars1[6] = self.lvrca
        self.Avatars1[7] = "https://i.ibb.co/3pHS4wB/Default-Placeholder.png"
        self.Avatars.append(self.Avatars1)
        self.AvatarIndex = 0
        self.AvatarUpdate(0)
    #Semi-Automates the hotswapping procedure
    def Hotswap(self):
        self.HotswapButton.setEnabled(True)
        try:
            #Enables progress bar
            self.ProgBar.setEnabled(True)
            #Checks if vrca has been loaded from eslewhere
            if self.Avatars[self.AvatarIndex][7] != "VRCA":
                #If it has declare a demo image
                self.imgurl = self.Avatars[self.AvatarIndex][7]
                self.DownVRCAT(self.imgurl, "Logo.png")
                os.remove("HSB/Assets/Logo.png")
                shutil.move("Logo.png", "HSB/Assets/Logo.png")
            #Set pregress bar to 10%
            self.ProgBar.setValue(10)
            #Hotswap Started
            self.updateconsole("Hotswap Started...")
            #Getting temp dir
            self.ProjPath = tempfile.gettempdir()+"\\DefaultCompany\\HSB\\custom.vrca"
            os.chdir("HOTSWAP")
            self.ProgBar.setValue(20)
            self.updateconsole("Got Temp Dir...")
            #Starting decompression of hsb vrca
            shutil.copy(self.ProjPath, "custom.vrca")
            self.updateconsole("Decompress Started...")
            #Starting decompression of hotswaping vrca
            os.system("HOTSWAP.exe d custom.vrca")
            self.updateconsole("Decompressed...")
            #Read decompressed file
            with open("decompressed.vrca", "rb") as f:
                f = f.read()
            #Get new avtr id
            self.NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
            self.ProgBar.setValue(30)
            self.updateconsole("New ID Located...")
            #Clean up temp files
            if os.path.exists("decompressed.vrca"):
                os.remove("decompressed.vrca")
            #Exit hotswap
            os.chdir("..")
            self.ProgBar.setValue(40)
            self.updateconsole("Cleaned Temp FIles...")
            #If its an loaded vrca accomodate it
            if self.Avatars[0][0] != "VRCA":
                self.DownVRCAT(self.Avatars[self.AvatarIndex][6], "HOTSWAP/Avatar.vrca")
            else:
                shutil.copy(self.lvrca, "HOTSWAP/Avatar.vrca")
            #Re-enter hotswap
            os.chdir("HOTSWAP")
            #Cleans temp files
            if os.path.exists("custom.vrca"):
                os.remove("custom.vrca")
            self.ProgBar.setValue(50)
            self.updateconsole("Downloaded/Copied VRCA...")
            self.updateconsole("Decompression Of New Avatar Started...")
            #Decompresses new avi
            os.system("HOTSWAP.exe d Avatar.vrca")
            self.ProgBar.setValue(60)
            self.updateconsole("New Avatar Decompressed...")
            #Replaces avtr ids
            self.oldid = self.Avatars[self.AvatarIndex][1]
            self.ReplaceID(self.oldid, self.NewID)
            self.ProgBar.setValue(70)
            self.updateconsole("ID's Swapped...")
            self.updateconsole("New Avatar Compression Started...")
            #Creates compressed file
            os.system("HOTSWAP.exe c decompressed1.vrca")
            self.updateconsole("New Avatar Compressed...")
            self.ProgBar.setValue(80)
            #Cleans temp files
            if os.path.exists("Avatar.vrca"):
                os.remove("Avatar.vrca")
            if os.path.exists("decompressed.vrca"):
                os.remove("decompressed.vrca")
            if os.path.exists("decompressed1.vrca"):
                os.remove("decompressed1.vrca")
            if os.path.exists(self.ProjPath):
                os.remove(self.ProjPath)
            self.ProgBar.setValue(90)
            self.updateconsole("Temp Cleaned...")
            #Renames and moves new vrca
            os.rename("compressed.vrca", "custom.vrca")
            shutil.move("custom.vrca", self.ProjPath)
            self.updateconsole("VRCA Moved...")
            self.ProgBar.setValue(100)
            #States completeness in console
            self.updateconsole("DONE SWAPPING")
            #Exit hotswap folder
            os.chdir("..")
            self.ProgBar.setEnabled(False)
            time.sleep(10)
            self.ProgBar.setValue(0)
            #Re-enable button
            self.HotswapButton.setEnabled(True)
        except:
            #If somthing breaks log it and send it to us
            self.senderrorlogs(traceback.format_exc())
            with open("latest.log", "a+", errors="ignore") as k:
                k.writelines(traceback.format_exc() + "\n\n")
    #Deletes log file
    def DeleteLogs(self):
        #If there is a log file delete it
        if os.path.exists(self.LogFolder + "/Log.txt"):
            os.remove(self.LogFolder + "/Log.txt")
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling, True) #enable highdpi scaling
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_UseHighDpiPixmaps, True) #use highdpi icons
#app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application