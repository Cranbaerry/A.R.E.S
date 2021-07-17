import sys
import qdarkstyle
import os
from PyQt5 import QtWidgets, uic
import sys
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *
import sys


class Ui(QtWidgets.QMainWindow):
    def __init__(self):
        super(Ui, self).__init__() # Call the inherited classes __init__ method
        uic.loadUi('untitled.ui', self) # Load the .ui file
        self.show() # Show the GUI



app = QtWidgets.QApplication(sys.argv) # Create an instance of QtWidgets.QApplication
app.setStyleSheet(qdarkstyle.load_stylesheet())
window = Ui() # Create an instance of our class
app.exec_() # Start the application