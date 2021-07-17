import qdarkstyle, os, sys, requests, urllib
from PyQt5 import QtWidgets, uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *

class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__() # Call the inherited classes __init__ method
        uic.loadUi('untitled.ui', self) # Load the .ui file
        self.show() # Show the GUI

        self.leftbox = self.findChild(QtWidgets.QLabel, 'PreviewImage')
        url = "https://api.vrchat.cloud/api/1/file/file_44e12631-8eca-4de7-9bce-9e7fe2a6e55d/1/file"
        payload = ""
        headers = {"User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"}
        data = requests.request("GET", url, data=payload, headers=headers)
        pixmap = QPixmap()
        pixmap.loadFromData(data.content)
        self.leftbox.setPixmap(pixmap)



app = QtWidgets.QApplication(sys.argv) # Create an instance of QtWidgets.QApplication
app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui() # Create an instance of our class
app.exec_() # Start the application