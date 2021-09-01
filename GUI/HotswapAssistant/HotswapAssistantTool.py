import sys, os, re, subprocess
from PyQt5 import QtWidgets, uic, QtCore, QtGui
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"
class Ui(QtWidgets.QMainWindow):
    #Initialises all buttons and base functions for the script
    def __init__(self):
        # Loads UI file and initiates buttons
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(364, 114)
        uic.loadUi('GUI.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        self.LoadVRCAB = self.findChild(QtWidgets.QPushButton, 'LoadVRCAB')
        self.LoadVRCAB.clicked.connect(self.LoadVRCA)
        self.OLDID = self.findChild(QtWidgets.QLabel, 'OLDID')
        self.OLDID.setText("")
        self.SwapIDB = self.findChild(QtWidgets.QPushButton, 'SwapIDB')
        self.SwapIDB.clicked.connect(self.SwapIds)
        self.SwapIDB.setEnabled(False)
    #Allows the user to load a vrca/vrcw
    def LoadVRCA(self):
        #Prompts user to select a vrca or vrcw
        self.lvrca = QFileDialog.getOpenFileName(self, 'Open file', '',"Bundle Files (*.vrca *.vrcw)")[0]
        #If they fail to pick a file just cancel
        if self.lvrca == "":
            return
        #Decompress file
        res = subprocess.Popen(rf'HOTSWAP.exe d "{self.lvrca}"')
        if res.wait() != 0:
            print("Error")
            os.system(rf'HOTSWAP.exe c "{self.lvrca}"')
            os.system(rf'HOTSWAP.exe d decompressed.vrca')
            os.remove("compressed.vrca")
        #Open/read decompressed file
        with open("decompressed.vrca", "rb") as f:
            f = f.read()
        #Assign VRCA or VRCW depending on weather or not a world id or an avatar id is within the bundle
        if "vrca" in self.lvrca:
            self.oldavtrid = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        if "vrcw" in self.lvrca:
            self.oldavtrid = re.search("(wrld_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.OLDID.setText(self.oldavtrid)
        #Allow them to swap ID's
        self.SwapIDB.setEnabled(True)
    #Swap the ids
    def SwapIds(self):
        #Get the new if
        self.NewIDBox = self.findChild(QtWidgets.QLineEdit, 'NewIDBox')
        print("Declared new id box")
        self.NewID = self.NewIDBox.text()
        print("Got text from new id box")
        #Read asset bundle
        with open("decompressed.vrca", "rb") as f:
            DAF = f.read()
        print("Opened and read decompressed")
        #Replace ids within the file
        DAF = DAF.replace(bytes(self.oldavtrid, 'utf-8'), bytes(self.NewID, 'utf-8'))
        print("Replaced Id's")
        with open("decompressedfile1", "wb") as f:
            f.write(DAF)
        #Remove temp files
        print("Opened and read decompressefile1")
        os.system('HOTSWAP.exe c decompressedfile1')
        print("Compressed")
        os.remove("decompressed.vrca")
        print("Removed decompressedfile")
        os.remove("decompressedfile1")
        print("Removed decompressedfile1")
        #Change extension depending id found
        if "vrcw" in self.lvrca:
            os.rename("compressed.vrca", "custom.vrcw")
            print("Renamed to custom.vrcw")
        if "vrca" in self.lvrca:
            os.rename("compressed.vrca", "custom.vrca")
            print("Renamed to custom.vrca")
        print("END")
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling, True) #enable highdpi scaling
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_UseHighDpiPixmaps, True) #use highdpi icons
#app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application