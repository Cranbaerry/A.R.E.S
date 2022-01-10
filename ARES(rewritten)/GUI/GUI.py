#The main python file reading from all modules and allowing them to work together
#in conjunction with the GUI

#Importing reqired modules
import sys, pymsgbox, json, os, threading, traceback, shutil,re
from PyQt5 import QtWidgets, uic, QtCore
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
#Importing custom ARES modules
from CoreUtils import *
from APIUpload import StartUploads
from LogUtils import *
from ExternalFunctions import *
import Search
#Enable/Disable debug mode
class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        #Initialising GUI
        super(Ui, self).__init__()
        uic.loadUi('GUI.ui', self)
        self.setFixedSize(780, 440)
        self.BaseDir = os.getcwd()
        os.chdir(self.BaseDir)
        self.show()
        #Prepares Tabs
        self.MainTab = self.findChild(QtWidgets.QTabWidget, 'MainTabs')
        #Prepares push buttons
        self.CleanExit = self.findChild(QtWidgets.QPushButton, 'CleanExitButton')
        self.CleanExit.clicked.connect(CleanExit)
        self.DeleteLog = self.findChild(QtWidgets.QPushButton, 'DeleteLogButton')
        self.DeleteLog.clicked.connect(DeleteLog)
        self.ChangeUnity = self.findChild(QtWidgets.QPushButton, 'ChangeUnityButton')
        self.ChangeUnity.clicked.connect(self.ChangeUnityF)
        self.ToggleAPI = self.findChild(QtWidgets.QPushButton, 'ToggleAPIButton')
        self.ToggleAPI.clicked.connect(self.ToggleAPIF)
        self.SetKey = self.findChild(QtWidgets.QPushButton, 'SetKeyButton')
        self.SetKey.clicked.connect(self.SetKeyF)
        self.OpenUnity = self.findChild(QtWidgets.QPushButton, 'OpenUnityButton')
        self.OpenUnity.clicked.connect(self.OpenUnityWrapper)
        self.Hotswap = self.findChild(QtWidgets.QPushButton, 'HotswapButton')
        self.Hotswap.clicked.connect(self.HotswapWrapper)
        self.RepairVRCA = self.findChild(QtWidgets.QPushButton, 'RepairCacheVRCAButton')
        self.RepairVRCA.clicked.connect(self.RepairVRCAWrapper)
        self.LoadAvatars = self.findChild(QtWidgets.QPushButton, 'LoadAvatarsButton')
        self.LoadAvatars.clicked.connect(self.LoadAvatarsWrapper)
        self.DownloadVRCA = self.findChild(QtWidgets.QPushButton, 'DownloadVRCAButton')
        self.DownloadVRCA.clicked.connect(self.DownloadVRCAWrapper)
        self.NextB = self.findChild(QtWidgets.QPushButton, 'NextButton')
        self.NextB.clicked.connect(self.NextAvi)
        self.PrevB = self.findChild(QtWidgets.QPushButton, 'PreviousButton')
        self.PrevB.clicked.connect(self.PrevAvi)
        self.LoadVRCA = self.findChild(QtWidgets.QPushButton, 'LoadVRCAButton')
        self.LoadVRCA.clicked.connect(self.LoadVRCAWrapper)
        self.ExtVRCA = self.findChild(QtWidgets.QPushButton, 'ExtractVRCAButton')
        self.ExtVRCA.clicked.connect(self.ExtractVRCAWrapper)
        self.SearchL = self.findChild(QtWidgets.QPushButton, 'SearchLocalButton')
        self.SearchL.clicked.connect(self.SearchLocalWrapper)
        self.SearchA = self.findChild(QtWidgets.QPushButton, 'SearchAPIButton')
        self.SearchA.clicked.connect(self.SearchApiWrapper)
        self.RefreshB = self.findChild(QtWidgets.QPushButton, 'RefreshButton')
        self.RefreshB.clicked.connect(lambda: CallUpdateStats(self.Settings["Username"], self))
        #Prepares text boxes
        self.SpecialThanks = self.findChild(QtWidgets.QPlainTextEdit, 'SpecialThanksBox')
        self.Console = self.findChild(QtWidgets.QPlainTextEdit, 'ConsoleBox')
        self.Data = self.findChild(QtWidgets.QTextEdit, 'RawData')
        #Prepares all the line edits
        self.KeyBox = self.findChild(QtWidgets.QLineEdit, 'SetKeyBox')
        self.SearchTerm = self.findChild(QtWidgets.QLineEdit, 'SearchTermLineEdit')
        #Prepares labels
        self.LogWrapperSize = self.findChild(QtWidgets.QLabel, 'LogSizeLabel')
        self.ResultsCount = self.findChild(QtWidgets.QLabel, 'resultsbox')
        self.AviStat =self.findChild(QtWidgets.QLabel, 'Status')
        self.PrevIMG = self.findChild(QtWidgets.QLabel, 'PreviewImage')
        self.APIStatus = self.findChild(QtWidgets.QLabel, 'APILabel')
        self.GLabel = self.findChild(QtWidgets.QLabel, 'GraphLabel')
        self.DBSL = self.findChild(QtWidgets.QLabel, 'DatabaseSizeL')
        self.UUSL = self.findChild(QtWidgets.QLabel, 'UserUploadsL')
        #Prepares progress bar
        self.ProgBar = self.findChild(QtWidgets.QProgressBar, 'ProgressBar')
        #Prepares radio buttons
        self.AvatarNameRB = self.findChild(QtWidgets.QRadioButton, 'AvatarNameRB')
        self.AvatarAuthorRB = self.findChild(QtWidgets.QRadioButton, 'AvatarAuthorRB')
        self.AvatarIDRB = self.findChild(QtWidgets.QRadioButton, 'AvatarIDRB')
        self.Algo1RB = self.findChild(QtWidgets.QRadioButton, 'Algo1RB')
        self.Algo2RB = self.findChild(QtWidgets.QRadioButton, 'Algo2RB')
        #Prepares check boxes
        self.PrivateCB = self.findChild(QtWidgets.QCheckBox, 'PrivateBox')
        self.PublicCB = self.findChild(QtWidgets.QCheckBox, 'PublicBox')
        self.PCCB = self.findChild(QtWidgets.QCheckBox, 'PCBox')
        self.QCB = self.findChild(QtWidgets.QCheckBox, 'QuestBox')
        self.NSFWCB = self.findChild(QtWidgets.QCheckBox, 'NSFWBox')
        self.VioCB = self.findChild(QtWidgets.QCheckBox, 'VioBox')
        self.GoreCB = self.findChild(QtWidgets.QCheckBox, 'GoreBox')
        self.ONSFWCB = self.findChild(QtWidgets.QCheckBox, 'ONSFWBox')
        #Getting special thanks from our pastebin and displaying it
        self.SpecialThanks.appendPlainText(GetSpecialThanks())
        #Checks if the app is set up correctly, if not run first time setup
        InitCore()
        InitLogUtils()
        if IsSetup() == False:
            self.LogWrapper("GUI started!")
            self.LogWrapper("First time setup begun!")
            pymsgbox.alert("Select Unity 2019.4.31f1 Exe")
            self.UPath = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
            with open("Settings.json", "a+") as s:
                dd = {
                    "Unity_Exe": self.UPath,
                    "Username": "Default",
                    "SendToAPI": False
                }
                self.LogWrapper(f"Unity selected: {self.UPath}")
                s.write(json.dumps(dd, indent=4))
            EventLog("Settings saved!")
        #Loads the settings into the application
        self.Settings = GetSettings()
        #Sets API status label and logs its status to the console
        if self.Settings["SendToAPI"] == True:
            self.APIStatus.setText("API Enabled!")
            self.SearchA.setEnabled(True)
            threading.Thread(target=UpdateStats,args=(self.Settings["Username"], self)).start()
            if os.path.isdir("Log.txt"):
                StartUploads(self.Settings["Username"])
            self.LogWrapper("API is enabled on startup!")

        else:
            self.APIStatus.setText("API Disabled!")
            self.LogWrapper("API is disabled on startup")
            self.MainTab.setTabVisible(2, False)
        #Gets the log size and displays it within a label
        self.LogWrapperSize.setText(LogSize())
        self.LogWrapper("Settings loaded!")
    #Allows the stored unity directory to be changed
    def ChangeUnityF(self):
        self.Settings["Unity_Exe"] = QFileDialog.getOpenFileName(self, 'Select Unity.exe', 'Unity', "EXE Files (*.exe)")[0]
        SaveSettings(self.Settings)
        self.LogWrapper(f"Unity selected: {self.UPath}")
    #Allows for an API key to be entered and saved
    def SetKeyF(self):
        key = str(self.KeyBox.text().encode().decode("ascii", errors="ignore")).replace("\n","").replace("\\n", "")
        self.Settings["Username"] = key
        SaveSettings(self.Settings)
        self.KeyBox.setText("Key Set!")
        self.LogWrapper(f"Key set: {key}")
    #Second logging function for ease of logging
    def LogWrapper(self, Data):
        self.Console.appendPlainText(EventLog(Data))
    #Allows the API to be enabled and disabled, then quits the application
    # add this: make it start function upload avatars instead of starting
    def ToggleAPIF(self):
        if self.Settings["SendToAPI"] == True:
            self.Settings["SendToAPI"] = False
            SaveSettings(self.Settings)
            self.LogWrapper("API toggled off")
        else:
            self.Settings["SendToAPI"] = True
            SaveSettings(self.Settings)
            self.LogWrapper("API toggled on")
        os.system('taskkill /F /im "ARES.exe"')
    #Wrapper to open unity
    def OpenUnityWrapper(self):
        try:
            self.LogWrapper("HSB preperation has begun...")
            #Disables button to avoid spam
            self.OpenUnity.setEnabled(False)
            self.LogWrapper("Unity opening on new thread...")
            threading.Thread(target=OpenUnity, args=(self.Settings["Unity_Exe"],self,)).start()
        except:
            self.LogWrapper(f"Error occured during open unity process!\n{traceback.format_exc()}")
            SendErrorLogs(traceback.format_exc())
            os.chdir(self.BaseDir)
            self.OpenUnity.setEnabled(True)
            self.Hotswap.setEnabled(False)
    #Wrapper to correctly preform a hotswap
    def HotswapWrapper(self,vrca):
        try:
            self.LogWrapper("Started hotswap process...")
            #Disables button to avoid spam
            self.Hotswap.setEnabled(False)
            self.ProgBar.setValue(0)
            self.LogWrapper("Deciding asset URL...")
            if GetData(self.SelectedAvi, "TimeDetected") == "VRCA":
                self.LogWrapper("Hotswaping from loaded VRCA...")
                shutil.copy(GetData(self.SelectedAvi, "PCAsset"),"HOTSWAP\\Avatar.vrca")
                os.chdir(self.BaseDir)
            else:
                self.LogWrapper("Hotswaping from log! Downloading avatar...")
                DownloadVRCAFL(GetData(self.SelectedAvi,"PCAsset"),GetData(self.SelectedAvi,"QAsset"))
                self.LogWrapper("VRCA downloaded, continuing hotswap...")
                os.chdir(self.BaseDir)
            self.LogWrapper("Starting hotswap on new thread...")
            try:
                threading.Thread(target=Hotswap, args=(self,)).start()
            except:
                print(traceback.format_exc())
        except:
            self.LogWrapper(f"Error occured during hotswap process!\n{traceback.format_exc()}")
            SendErrorLogs(traceback.format_exc())
            os.chdir(self.BaseDir)
            self.Hotswap.setEnabled(True)
            self.ProgBar.setEnabled(False)
            self.ProgBar.setValue(0)
    #Wrapper to correctly repair a VRCA
    def RepairVRCAWrapper(self):
        try:
            self.LogWrapper("Started repair process...")
            # Disables button to avoid spam
            self.RepairVRCA.setEnabled(False)
            self.ProgBar.setValue(0)
            self.LogWrapper("Propting user to select VRCA...")
            vrca = QFileDialog.getOpenFileName(self, 'Open file', '', "VRCA Files (*.vrca)")[0]
            #If they fail to pick a file just cancel
            if vrca == "":
                self.LogWrapper("No VRCA selected, repair cancelled!")
                os.chdir(self.BaseDir)
                self.RepairVRCA.setEnabled(True)
                return
            shutil.copy(vrca, "HOTSWAP\\bad.vrca")
            os.chdir(self.BaseDir)
            self.LogWrapper(f"VRCA selected: {vrca}")
            self.LogWrapper("Starting repair on new thread...")
            threading.Thread(target=RepairVRCA, args=(self,)).start()
        except:
            self.LogWrapper(f"Error occured during repair process!\n{traceback.format_exc()}")
            SendErrorLogs(traceback.format_exc())
            os.chdir(self.BaseDir)
            self.RepairVRCA.setEnabled(True)
            self.ProgBar.setEnabled(False)
            self.ProgBar.setValue(0)
    #Wrapper to load external VRCAs
    def LoadVRCAWrapper(self):
        try:
            self.LogWrapper("Started VRCA loading process...")
            # Disables button to avoid spam
            self.LoadVRCA.setEnabled(False)
            self.LogWrapper("Propting user to select VRCA...")
            vrca = QFileDialog.getOpenFileName(self, 'Open file', '', "VRCA Files (*.vrca)")[0]
            # If they fail to pick a file just cancel
            if vrca == "":
                self.LogWrapper("No VRCA selected, load cancelled!")
                os.chdir(self.BaseDir)
                self.LoadVRCA.setEnabled(True)
                return
            self.Avatars = []
            self.SelectedAvi = [
                "VRCA",
                "VRCA",
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
            ]
            self.SelectedAvi[6] = vrca
            self.Avatars.append(self.SelectedAvi)
            self.ResultsCount.setText("Loaded")
            self.Data.setPlainText(CleanText(self.SelectedAvi))
            self.LogWrapper("Loaded!")
            #Enables button again
            self.LoadVRCA.setEnabled(True)
            self.NextB.setEnabled(False)
            self.PrevB.setEnabled(False)
            self.ExtVRCA.setEnabled(True)
        except:
            self.LogWrapper(f"Error occured during VRCA loading process!\n{traceback.format_exc()}")
            SendErrorLogs(traceback.format_exc())
            print(f'Error {traceback.format_exc()}')
            os.chdir(self.BaseDir)
            self.LoadVRCA.setEnabled(True)
    #Wrapper to load avatars
    def LoadAvatarsWrapper(self):
        try:
            self.LogWrapper("Attempting to load avatars...")
            self.LoadAvatars.setEnabled(False)
            self.LogWrapper("Loading avatars...")
            Pattern = "Time Detected:(.*)\nAvatar ID:(.*)\nAvatar Name:(.*)\nAvatar Description:(.*)\nAuthor ID:(.*)\nAuthor Name:(.*)\nPC Asset URL:(.*)\nQuest Asset URL:(.*)\nImage URL:(.*)\nThumbnail URL:(.*)\nUnity Version:(.*)\nRelease Status:(.*)\nTags:(.*)"
            # Setup logs to be read
            with open("Log.txt", "r+", errors="ignore") as lf:
                Logs = lf.read()
                #Find all logs via pattern
                self.Avatars = re.findall(Pattern, Logs)
                self.Avatars = sorted(self.Avatars, reverse=True)
                self.MaxAvatar = len(self.Avatars)
                self.AvatarIndex = 0
                #Select first avi and present data
                self.SelectedAvi = self.Avatars[self.AvatarIndex]
                self.UpdateAvi()
            self.LoadAvatars.setEnabled(True)
            self.UpdateBut(True)
            self.LogWrapper("Avatars loaded!")
        except:
            self.LogWrapper(f"Error occured during the avatar loading process!\n{traceback.format_exc()}")
            SendErrorLogs(traceback.format_exc())
            self.LoadAvatars.setEnabled(True)
            self.UpdateBut(False)
    #Wrapper to extract VRCA files
    def ExtractVRCAWrapper(self):
        try:
            self.ExtVRCA.setEnabled(False)
            self.LogWrapper("Extract VRCA started...")
            self.LogWrapper("Deciding asset URL...")
            if GetData(self.SelectedAvi, "TimeDetected") == "VRCA":
                shutil.copy(GetData(self.SelectedAvi, "PCAsset"),"AssetRipperConsole_win64(ds5678)\\Avatar.vrca")
                os.chdir(self.BaseDir)
                self.LogWrapper("Extracting loaded VRCA...")
            else:
                self.LogWrapper("Extracting from log! Downloading avatar...")
                DownloadVRCAFL(GetData(self.SelectedAvi, "PCAsset"), GetData(self.SelectedAvi, "QAsset"))
                os.chdir("HOTSWAP")
                shutil.move("Avatar.vrca",self.BaseDir + "\\Avatar.vrca")
                os.chdir(self.BaseDir)
                shutil.move("Avatar.vrca", "AssetRipperConsole_win64(ds5678)\\Avatar.vrca")
                os.chdir(self.BaseDir)
                self.LogWrapper("VRCA downloaded, continuing extract...")
            if self.Algo1RB.isChecked():
                ExtValue = "2019DLL"
            if self.Algo2RB.isChecked():
                ExtValue = "2018DLL"
            self.LogWrapper("Starting extract on new thread...")
            threading.Thread(target=ExtractVRCA, args=(ExtValue,self,)).start()
        except:
            self.ExtVRCA.setEnabled(True)
            self.LogWrapper(f"An error occured while extracting a VRCA: {traceback.format_exc()}")
    def SearchLocalWrapper(self):
        self.searchavis(Localss=True)
    def SearchApiWrapper(self):
        self.searchavis(Localss=False)
    #Wrapper to regulate downloading of avatars via the download VRCA button
    def DownloadVRCAWrapper(self):
        try:
            self.LogWrapper("Attempting to download vrca...")
            self.DownloadVRCA.setEnabled(False)
            DownloadVRCA(GetData(self.SelectedAvi,"PCAsset"),GetData(self.SelectedAvi,"QAsset"))
            self.LogWrapper("VRCA download begun!")
            self.DownloadVRCA.setEnabled(True)
        except:
            self.DownloadVRCA.setEnabled(True)
            self.LogWrapper(f"An error occured while initiating the download of a VRCA: {traceback.format_exc()}")
    #Goes to the next avatar in a loaded list
    def NextAvi(self):
        if self.AvatarIndex + 1 >= self.MaxAvatar:
            return
        self.AvatarIndex += 1
        self.SelectedAvi = self.Avatars[self.AvatarIndex]
        self.UpdateAvi()
    #Goes to the previous avatar in a loaded list
    def PrevAvi(self):
        if self.AvatarIndex == 0:
            return
        self.AvatarIndex -= 1
        self.SelectedAvi = self.Avatars[self.AvatarIndex]
        self.UpdateAvi()
    #Function to update GUI to currently selected avatar
    def UpdateAvi(self):
        self.Data.setPlainText(CleanText(self.SelectedAvi))
        self.ResultsCount.setText(f'{self.AvatarIndex + 1}/{self.MaxAvatar + 1}')
        if GetData(self.SelectedAvi,"ReleaseStatus") == "private":
            self.AviStat.setStyleSheet("background-color: red; border: 3px solid black;")
            self.AviStat.setText("Private")
        if GetData(self.SelectedAvi,"ReleaseStatus") == "public":
            self.AviStat.setStyleSheet("background-color: blue; border: 3px solid black;")
            self.AviStat.setText("Public")
        data = GetImage(GetData(self.SelectedAvi, "ThumbURL"))
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.PrevIMG.setPixmap(pixmap)
    # Function to update GUI buttons to currently selected avatar
    def UpdateBut(self, value):
        self.DownloadVRCA.setEnabled(value)
        self.NextB.setEnabled(value)
        self.PrevB.setEnabled(value)
        self.SearchL.setEnabled(value)
        self.ExtVRCA.setEnabled(value)
        self.Hotswap.setEnabled(value)
    def searchavis(self, Localss=True):
        filterss = {
            "private": self.PrivateCB.isChecked(),
            "public": self.PublicCB.isChecked(),
            "PCasseturl": self.PCCB.isChecked(),
            "Questasseturl": self.QCB.isChecked(),
            "NSFW": self.NSFWCB.isChecked(),
            "Violonce": self.VioCB.isChecked(),
            "Gore": self.GoreCB.isChecked(),
            "Othernsfw": self.ONSFWCB.isChecked(),
            "Avatar name": self.AvatarNameRB.isChecked(),
            "Avatar author": self.AvatarAuthorRB.isChecked(),
            "Avatar id": self.AvatarIDRB.isChecked(),
            "key": self.Settings["Username"]
        }
        if Localss:
            try:
                quary = self.SearchTerm.text()
                filtered = Search.search(query=quary, api=False, Localavatars=self.Avatars, filters=filterss)
                self.Avatars = filtered
                if len(filtered) == 0:
                    self.LogWrapper("No avatars found!")
                    self.Data.setPlainText("No avatars found!\ntry something else!")
                    data = GetImage("https://image.freepik.com/free-vector/glitch-error-404-page_23-2148105404.jpg")
                    pixmap = QPixmap()
                    pixmap.loadFromData(data.content)
                    self.PrevIMG.setPixmap(pixmap)
                    self.UpdateBut(False)
                else:
                    self.LogWrapper(f"{len(filtered)} avatars found!")
                    self.MaxAvatar = len(filtered) - 1
                    self.AvatarIndex = 0
                    self.SelectedAvi = self.Avatars[self.AvatarIndex]
                    self.UpdateAvi()
                    self.UpdateBut(True)
            except:
                self.LogWrapper(f"Error occured during search: {traceback.format_exc()}")
        if not Localss:
            try:
                quary = self.SearchTerm.text()
                self.Avatars = Search.search(query=quary, api=True, Localavatars=None, filters=filterss)
                if len(self.Avatars) == 0:
                    self.LogWrapper("No avatars found!")
                    self.Data.setPlainText("No avatars found!\ntry something else!")
                    data = GetImage("https://image.freepik.com/free-vector/glitch-error-404-page_23-2148105404.jpg")
                    pixmap = QPixmap()
                    pixmap.loadFromData(data.content)
                    self.PrevIMG.setPixmap(pixmap)
                    self.UpdateBut(False)
                else:
                    self.LogWrapper(f"{len(self.Avatars)} avatars found!")
                    self.MaxAvatar = len(self.Avatars) - 1
                    self.AvatarIndex = 0
                    self.SelectedAvi = self.Avatars[self.AvatarIndex]
                    self.UpdateAvi()
                    self.UpdateBut(True)
            except:
                self.LogWrapper(f"Error occured during search: {traceback.format_exc()}")


#Extra GUI stuffs
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling, True)
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_UseHighDpiPixmaps, True)
app = QtWidgets.QApplication(sys.argv)
window = Ui()
app.exec_()