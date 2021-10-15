import sys, os, re, threading
from PyQt5 import QtWidgets, uic, QtCore
from PyQt5.QtWidgets import *
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"
class Ui(QtWidgets.QMainWindow):
    #Initialises all buttons and base functions for the script
    def __init__(self):
        # Loads UI file and initiates buttons
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(432, 256)
        uic.loadUi('GUI.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        self.LoadVRCA = self.findChild(QtWidgets.QPushButton, 'LoadVRCAB')
        self.LoadVRCA.clicked.connect(self.LoadVRCAFunc)
        self.LoadDVRCA = self.findChild(QtWidgets.QPushButton, 'LoadDVRCAB')
        self.LoadDVRCA.clicked.connect(self.LoadDVRCAFunc)
        self.LoadDVRCA.setEnabled(False)
        self.Hotswap = self.findChild(QtWidgets.QPushButton, 'HotswapB')
        self.Hotswap.clicked.connect(self.HotswapFunc)
        self.Hotswap.setEnabled(False)
        self.oldidl = self.findChild(QtWidgets.QLabel, 'OldIDLabel')
        self.oldcabl = self.findChild(QtWidgets.QLabel, 'OldCABLabel')
        self.newidl = self.findChild(QtWidgets.QLabel, 'NewIDLabel')
        self.newcabl = self.findChild(QtWidgets.QLabel, 'NewCABLabel')
        self.statusL = self.findChild(QtWidgets.QLabel, 'StatusLabel')
        try:
            self.statusL.setText("Status: Old 'decompressed.vrca' found!")
            os.remove('decompressed.vrca')
            self.statusL.setText("Status: Old 'decompressed.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'compressed.vrca' found!")
            os.remove('compressed.vrca')
            self.statusL.setText("Status: Old 'compressed.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'Real.vrca' found!")
            os.remove('Real.vrca')
            self.statusL.setText("Status: Old 'Real.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'Dummy.vrca' found!")
            os.remove('Dummy.vrca')
            self.statusL.setText("Status: Old 'Dummy.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'DecompHS.vrca' found!")
            os.remove('DecompHS.vrca')
            self.statusL.setText("Status: Old 'DecompHS.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'custom.vrca' found!")
            os.remove('custom.vrca')
            self.statusL.setText("Status: Old 'custom.vrca' deleted!")
        except:
            pass
        self.statusL.setText("Status: Idle")
    def LoadVRCAFunc(self):
        threading.Thread(target=self.LoadVRCAFuncT, args={}).start()
    def LoadVRCAFuncT(self):
        self.lvrca = QFileDialog.getOpenFileName(self, 'Open file', '', "VRCA Files (*.vrca)")[0]
        if self.lvrca == "":
            self.statusL.setText("Status: No VRCA selected!")
            return
        self.statusL.setText("Status: Decompressing loaded VRCA...")
        os.system(rf'HOTSWAP.exe d "{self.lvrca}"')
        self.statusL.setText("Status: Loaded VRCA decompressed!")
        self.statusL.setText("Status: Reading decompressed loaded VRCA...")
        with open("decompressed.vrca", "rb") as f:
            f = f.read()
            self.statusL.setText("Status: Decompressed loaded VRCA read!")
        self.statusL.setText("Status: Searching for old avatar ID...")
        self.oldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.oldidl.setText(self.oldID)
        self.statusL.setText("Status: Old avatar ID found!")
        self.statusL.setText("Status: Searching for old CAB...")
        self.oldCAB = re.search("(CAB-[\w\d]{32})", str(f)).group(1)
        self.oldcabl.setText(self.oldCAB)
        self.statusL.setText("Status: Old CAB found!")
        self.statusL.setText("Status: Renaming file...")
        os.rename('decompressed.vrca', 'Real.vrca')
        self.statusL.setText("Status: File renamed!")
        self.LoadDVRCA.setEnabled(True)
        self.statusL.setText("Status: Ready for dummy VRCA!")
    def LoadDVRCAFunc(self):
        threading.Thread(target=self.LoadDVRCAFuncT, args={}).start()
    def LoadDVRCAFuncT(self):
        self.ldvrca = QFileDialog.getOpenFileName(self, 'Open file', '', "VRCA Files (*.vrca)")[0]
        if self.ldvrca == "":
            self.statusL.setText("Status: No VRCA selected!")
            return
        self.statusL.setText("Status: Decompressing dummy VRCA...")
        os.system(rf'HOTSWAP.exe d "{self.ldvrca}"')
        self.statusL.setText("Status: Dummy VRCA decompressed!")
        self.statusL.setText("Status: Reading decompressed dummy VRCA...")
        with open("decompressed.vrca", "rb") as f:
            f = f.read()
            self.statusL.setText("Status: Decompressed dummy VRCA read!")
        self.statusL.setText("Status: Searching for new avatar ID...")
        self.newID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.newidl.setText(self.newID)
        self.statusL.setText("Status: New avatar ID found!")
        self.statusL.setText("Status: Searching for new CAB...")
        self.newCAB = re.search("(CAB-[\w\d]{32})", str(f)).group(1)
        self.newcabl.setText(self.newCAB)
        self.statusL.setText("Status: New CAB found!")
        self.statusL.setText("Status: Renaming file...")
        os.rename('decompressed.vrca', 'Dummy.vrca')
        self.statusL.setText("Status: File renamed!")
        self.Hotswap.setEnabled(True)
        self.statusL.setText("Status: Ready to hotswap!")
    def HotswapFunc(self):
        threading.Thread(target=self.HotswapFuncT, args={}).start()
    def HotswapFuncT(self):
        self.statusL.setText("Status: Reading data...")
        with open("Real.vrca", "rb") as f:
            avidata = f.read()
            self.statusL.setText("Status: Data read!")
        self.statusL.setText("Status: Replacing ID...")
        avidata = avidata.replace(bytes(self.oldID, 'utf-8'), bytes(self.newID, 'utf-8'))
        self.statusL.setText("Status: ID replaced!")
        self.statusL.setText("Status: Replacing CAB...")
        avidata = avidata.replace(bytes(self.oldCAB, 'utf-8'), bytes(self.newCAB, 'utf-8'))
        self.statusL.setText("Status: CAB replaced!")
        self.statusL.setText("Status: Writing new data...")
        with open("DecompHS.vrca", "wb") as f:
            f.write(avidata)
            self.statusL.setText("Status: Data written!")
        self.statusL.setText("Status: Compressing...")
        os.system('HOTSWAP.exe c DecompHS.vrca')
        self.statusL.setText("Status: Compressed!")
        self.statusL.setText("Status: Renaming files...")
        os.rename('compressed.vrca', 'custom.vrca')
        self.statusL.setText("Status: Renamed!")
        self.statusL.setText("Status: Cleaning...")
        try:
            self.statusL.setText("Status: Old 'decompressed.vrca' found!")
            os.remove('decompressed.vrca')
            self.statusL.setText("Status: Old 'decompressed.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'compressed.vrca' found!")
            os.remove('compressed.vrca')
            self.statusL.setText("Status: Old 'compressed.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'Real.vrca' found!")
            os.remove('Real.vrca')
            self.statusL.setText("Status: Old 'Real.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'Dummy.vrca' found!")
            os.remove('Dummy.vrca')
            self.statusL.setText("Status: Old 'Dummy.vrca' deleted!")
        except:
            pass
        try:
            self.statusL.setText("Status: Old 'DecompHS.vrca' found!")
            os.remove('DecompHS.vrca')
            self.statusL.setText("Status: Old 'DecompHS.vrca' deleted!")
        except:
            pass
        self.statusL.setText("Status: Clean!")
        self.statusL.setText("You can find your VRCA in my folder! 'custom.vrca'")
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling, True) #enable highdpi scaling
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_UseHighDpiPixmaps, True) #use highdpi icons
#app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application