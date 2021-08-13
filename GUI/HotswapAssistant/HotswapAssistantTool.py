import sys, os, re, subprocess
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__()  # Call the inherited classes __init__ method
        self.setFixedSize(364, 114)
        uic.loadUi('untitled.ui', self)  # Load the .ui file
        self.show()  # Show the GUI
        self.LoadVRCAB = self.findChild(QtWidgets.QPushButton, 'LoadVRCAB')
        self.LoadVRCAB.clicked.connect(self.LoadVRCA)
        self.OLDID = self.findChild(QtWidgets.QLabel, 'OLDID')
        self.OLDID.setText("")
        self.SwapIDB = self.findChild(QtWidgets.QPushButton, 'SwapIDB')
        self.SwapIDB.clicked.connect(self.SwapIds)
        self.SwapIDB.setEnabled(False)
    def LoadVRCA(self):
        self.lvrca = QFileDialog.getOpenFileName(self, 'Open file', '',"VRCA Files (*.vrca)")[0]
        res = subprocess.Popen(rf'HOTSWAP.exe d "{self.lvrca}"')
        if res.wait() != 0:
            print("Error")
            os.system(rf'HOTSWAP.exe c "{self.lvrca}"')
            os.system(rf'HOTSWAP.exe d decompressed.vrca')
            os.remove("compressed.vrca")
        with open("decompressed.vrca", "rb") as f:
            f = f.read()
        self.oldavtrid = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(f)).group(1)
        self.OLDID.setText(self.oldavtrid)
        self.SwapIDB.setEnabled(True)
    def SwapIds(self):
        self.NewIDBox = self.findChild(QtWidgets.QLineEdit, 'NewIDBox')
        self.NewID = self.NewIDBox.text()
        with open("decompressedfile", "rb") as f:
            DAF = f.read()
        DAF = DAF.replace(bytes(self.oldavtrid, 'utf-8'), bytes(self.NewID, 'utf-8'))
        with open("decompressedfile1", "wb") as f:
            f.write(DAF)
        os.system('HOTSWAP.exe c')
        os.remove("decompressedfile")
        os.remove("decompressedfile1")
app = QtWidgets.QApplication(sys.argv)  # Create an instance of QtWidgets.QApplication
#app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui()  # Create an instance of our class
app.exec_()  # Start the application