import sys, os, re, threading
from PyQt5 import QtWidgets, uic, QtCore
from PyQt5.QtWidgets import *
os.environ["QT_AUTO_SCREEN_SCALE_FACTOR"] = "1"
class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()
        self.setFixedSize(533, 256)
        uic.loadUi('GUI.ui', self)
        self.show()
        self.LoadB = self.findChild(QtWidgets.QPushButton, 'LoadBB')
        self.LoadB.clicked.connect(self.LoadBundleFunc)
        self.LoadDB = self.findChild(QtWidgets.QPushButton, 'LoadDBB')
        self.LoadDB.clicked.connect(self.LoadDBFunc)
        self.LoadDB.setEnabled(False)
        self.Build = self.findChild(QtWidgets.QPushButton, 'BuildB')
        self.Build.clicked.connect(self.BuildFunc)
        self.Build.setEnabled(False)
        self.AV = self.findChild(QtWidgets.QRadioButton, 'AVRB')
        self.W = self.findChild(QtWidgets.QRadioButton, 'WRB')
        self.oldidl = self.findChild(QtWidgets.QLabel, 'OldIDLabel')
        self.oldcabl = self.findChild(QtWidgets.QLabel, 'OldCABLabel')
        self.newidl = self.findChild(QtWidgets.QLabel, 'NewIDLabel')
        self.newcabl = self.findChild(QtWidgets.QLabel, 'NewCABLabel')
        self.statusL = self.findChild(QtWidgets.QLabel, 'StatusLabel')
        if os.path.exists("decompressed.LargestBundle"):
            self.statusL.setText("Status: Old 'decompressed.LargestBundle' found!")
            os.remove("decompressed.LargestBundle")
            self.statusL.setText("Status: Old 'decompressed.LargestBundle' deleted!")
        if os.path.exists("compressed.LargestBundle"):
            self.statusL.setText("Status: Old 'compressed.LargestBundle' found!")
            os.remove("compressed.LargestBundle")
            self.statusL.setText("Status: Old 'compressed.LargestBundle' deleted!")
        if os.path.exists("custom.vrca"):
            self.statusL.setText("Status: Old 'custom.vrca' found!")
            os.remove("custom.vrca")
            self.statusL.setText("Status: Old 'custom.vrca' deleted!")
        if os.path.exists("custom.vrcw"):
            self.statusL.setText("Status: Old 'custom.vrcw' found!")
            os.remove("custom.vrcw")
            self.statusL.setText("Status: Old 'custom.vrcw' deleted!")
        if os.path.exists("selected.LargestBundle"):
            self.statusL.setText("Status: Old 'selected.LargestBundle' found!")
            os.remove("selected.LargestBundle")
            self.statusL.setText("Status: Old 'selected.LargestBundle' deleted!")
        if os.path.exists("dummy.LargestBundle"):
            self.statusL.setText("Status: Old 'dummy.LargestBundle' found!")
            os.remove("dummy.LargestBundle")
            self.statusL.setText("Status: Old 'dummy.LargestBundle' deleted!")
        if os.path.exists("built.LargestBundle"):
            self.statusL.setText("Status: Old 'built.LargestBundle' found!")
            os.remove("built.LargestBundle")
            self.statusL.setText("Status: Old 'built.LargestBundle' deleted!")
        self.statusL.setText("Status: Idle")
    def LoadBundleFunc(self):
        threading.Thread(target=self.LoadBundleFuncT(), args={}).start()
    def LoadBundleFuncT(self):
        self.lb = QFileDialog.getOpenFileName(self, 'Open file', '', "VRC Files (*.vrca , *.vrcw)")[0]
        if self.lb == "":
            self.statusL.setText("Status: No VRC file selected!")
            return
        self.statusL.setText("Status: Decompressing loaded bundle...")
        os.system(rf'HOTSWAP.exe d "{self.lb}"')
        self.statusL.setText("Status: Loaded bundle decompressed!")
        self.statusL.setText("Status: Reading decompressed bundle...")
        with open("decompressed.LargestBundle", "rb") as f:
            f = f.read()
            self.statusL.setText("Status: Decompressed bundle read!")
        self.statusL.setText("Status: Searching for old ID...")
        if self.AV.isChecked():
            self.oldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
            self.statusL.setText("Status: Searching for old CAB...")
            self.oldCAB = re.search("(CAB-[\w\d]{32})", str(f)).group(1)
            self.oldcabl.setText(self.oldCAB)
            self.statusL.setText("Status: Old CAB found!")
        if self.W.isChecked():
            self.oldID = re.search("(wrld_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.oldidl.setText(self.oldID)
        self.statusL.setText("Status: Old bundle ID found!")
        self.statusL.setText("Status: Renaming file...")
        os.rename('decompressed.LargestBundle', 'selected.LargestBundle')
        self.statusL.setText("Status: File renamed!")
        self.LoadDVRCA.setEnabled(True)
        self.statusL.setText("Status: Ready for dummy bundle!")
    def LoadDBFunc(self):
        threading.Thread(target=self.LoadDBFuncT, args={}).start()
    def LoadDBFuncT(self):
        self.ldb = QFileDialog.getOpenFileName(self, 'Open file', '', "VRC Files (*.vrca , *.vrcw)")[0]
        if self.ldb == "":
            self.statusL.setText("Status: No dummy bundle selected!")
            return
        self.statusL.setText("Status: Decompressing dummy bundle...")
        os.system(rf'HOTSWAP.exe d "{self.ldb}"')
        self.statusL.setText("Status: Dummy bundle decompressed!")
        self.statusL.setText("Status: Reading decompressed dummy bundle...")
        with open("decompressed.LargestBundle", "rb") as f:
            f = f.read()
            self.statusL.setText("Status: Decompressed dummy bundle read!")
        self.statusL.setText("Status: Searching for new bundle ID...")
        if self.AV.isChecked():
            self.newID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
            self.statusL.setText("Status: Searching for new CAB...")
            self.newCAB = re.search("(CAB-[\w\d]{32})", str(f)).group(1)
            self.newcabl.setText(self.newCAB)
            self.statusL.setText("Status: New CAB found!")
        if self.W.isChecked():
            self.newID = re.search("(wrld_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.newidl.setText(self.newID)
        self.statusL.setText("Status: New bundle ID found!")
        self.statusL.setText("Status: Renaming file...")
        os.rename('decompressed.LargestBundle', 'dummy.LargestBundle')
        self.statusL.setText("Status: File renamed!")
        self.Hotswap.setEnabled(True)
        self.statusL.setText("Status: Ready to build!")
    def BuildFunc(self):
        threading.Thread(target=self.BuildFuncT, args={}).start()
    def BuildFuncT(self):
        self.statusL.setText("Status: Reading data...")
        with open("selected.LargestBundle", "rb") as f:
            bundledata = f.read()
            self.statusL.setText("Status: Data read!")
        self.statusL.setText("Status: Replacing ID...")
        bundledata = bundledata.replace(bytes(self.oldID, 'utf-8'), bytes(self.newID, 'utf-8'))
        self.statusL.setText("Status: ID replaced!")
        if self.W.isChecked():
            self.statusL.setText("Status: Replacing CAB...")
            bundledata = bundledata.replace(bytes(self.oldCAB, 'utf-8'), bytes(self.newCAB, 'utf-8'))
            self.statusL.setText("Status: CAB replaced!")
            self.statusL.setText("Status: Writing new data...")
        with open("built.LargestBundle", "wb") as f:
            f.write(bundledata)
            self.statusL.setText("Status: Data written!")
        self.statusL.setText("Status: Compressing...")
        os.system('HOTSWAP.exe c built.LargestBundle')
        self.statusL.setText("Status: Compressed!")
        self.statusL.setText("Status: Renaming files...")
        if self.AV.isChecked():
            os.rename('compressed.LargestBundle', 'custom.vrca')
        if self.W.isChecked():
            os.rename('compressed.LargestBundle', 'custom.vrcw')
        self.statusL.setText("Status: Renamed!")
        self.statusL.setText("Status: Cleaning...")
        if os.path.exists("decompressed.LargestBundle"):
            self.statusL.setText("Status: Old 'decompressed.LargestBundle' found!")
            os.remove("decompressed.LargestBundle")
            self.statusL.setText("Status: Old 'decompressed.LargestBundle' deleted!")
        if os.path.exists("compressed.LargestBundle"):
            self.statusL.setText("Status: Old 'compressed.LargestBundle' found!")
            os.remove("compressed.LargestBundle")
            self.statusL.setText("Status: Old 'compressed.LargestBundle' deleted!")
        if os.path.exists("selected.LargestBundle"):
            self.statusL.setText("Status: Old 'selected.LargestBundle' found!")
            os.remove("selected.LargestBundle")
            self.statusL.setText("Status: Old 'selected.LargestBundle' deleted!")
        if os.path.exists("dummy.LargestBundle"):
            self.statusL.setText("Status: Old 'dummy.LargestBundle' found!")
            os.remove("dummy.LargestBundle")
            self.statusL.setText("Status: Old 'dummy.LargestBundle' deleted!")
        if os.path.exists("built.LargestBundle"):
            self.statusL.setText("Status: Old 'built.LargestBundle' found!")
            os.remove("built.LargestBundle")
            self.statusL.setText("Status: Old 'built.LargestBundle' deleted!")
        self.statusL.setText("Status: Clean!")
        if self.AV.isChecked():
            self.statusL.setText("You can find your bundle in my folder! 'custom.vrca'")
        if self.W.isChecked():
            self.statusL.setText("You can find your bundle in my folder! 'custom.vrcw'")
app = QtWidgets.QApplication(sys.argv)
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_EnableHighDpiScaling, True)
QtWidgets.QApplication.setAttribute(QtCore.Qt.AA_UseHighDpiPixmaps, True)
window = Ui()
app.exec_()