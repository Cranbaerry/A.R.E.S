#A file containing all the modules allowing the GUI to read from/interact with the log

#Importing reqired modules
import os, pymsgbox, requests, dateparser, threading, CoreUtils, traceback
import matplotlib.pyplot as plt
from datetime import datetime
from PyQt5.QtGui import *
#Importing ARES modules
from GenerateHTML import genhtml
#Allows the log sice to be retrieved and returned
def InitLogUtils():
    global BaseD
    BaseD = os.getcwd()
def LogSize():
    if os.path.exists("Log.txt"):
        return (str(round(os.path.getsize("Log.txt") / (1024 * 1024))) + "MB")
    else:
        return "No Log!"
#Function to delete the logged avis
def DeleteLog():
    Answer = pymsgbox.prompt('Are you sure you want to delete your log file? type (yes) to delete')
    if Answer == "yes":
        if os.path.exists("Log.txt"):
            os.remove("Log.txt")
def BrowserViewLoad(cls):
    try:
        genhtml(cls.Avatars)
        LoadToBrowser(cls)
        #with open(f'{BaseD}\\avatars.html','r+',errors='ignore') as av:
            #Page = av.read()
        #cls.BrowserWindow.setHtml(Page)
        #cls.MainTab.setTabVisible(1, True)
        #cls.MainTab.setCurrentIndex(1)
    except:
        traceback.print_exc()
def LoadToBrowser(cls):
    os.system("start avatars.html")
    #cls.MainTab.setCurrentIndex(0)

def CallUpdateStats(key,cls):
    threading.Thread(target=UpdateStats,args=(key,cls)).start()
def UpdateStats(key,cls):
    try:
        headers = {
            'user-agent': key,
        }
        response = requests.get('https://api.avataruploader.tk/userstats', headers=headers)
        data = response.json()
        cls.DBSL.setText(str(data['Total_database_size']))
        cls.UUSL.setText(str(data['total_users_avatars']))
        dates = data["upload_date_data"]
        datescleaned = []
        for x in dates:
            kk = dateparser.parse(x)
            datescleaned.append(str(kk).split(" ")[0])
        GraphValues = {i: datescleaned.count(i) for i in datescleaned}
        x = GraphValues.keys()
        y = GraphValues.values()
        plt.style.use('dark_background')
        plt.bar(x, y, tick_label=list(x), width=0.6, color=['grey', 'lightgrey'])
        plt.xlabel('Dates')
        plt.ylabel('Avatars Logged')
        plt.title('Weekly Logging Statistics')
        plt.savefig('Graph.png')
        with open(f"{BaseD}\\Graph.png","rb") as g:
            graphdata = g.read()
        pixmap = QPixmap()
        pixmap.loadFromData(graphdata)
        cls.GLabel.setPixmap(pixmap)
    except:
        CoreUtils.EventLog(f"Update API stats died:\n{traceback.format_exc()}")
#Takes raw data and allows the user to decide what to do with the asset URLs
def DecideAssetURL(PC,Q):
    if "None" in PC:
        PCV = False
    else:
        PCV = True
    if "None" in Q:
        QV = False
    else:
        QV = True
    # If both quest and PC options are valid ask the user what platform they want the action to involve
    if PCV is True and QV is True:
        selection = pymsgbox.confirm(text="Would you like to use the Quest or PC version of the avatar?",title="VRCA Platform Select", buttons=["PC", "Quest"])
        if selection == "PC":
            url = PC
        if selection == "Quest":
            url = Q
    # If only quest is available default to quest
    if PCV is False and QV is True:
        url = Q
    # If only PC is available default to PC
    if PCV is True and QV is False:
        url = PC
    # Creates variables to home template for base avatar url
    base = "/".join(url.split('/')[:7])
    # Lets vrc think me browser
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
        "Content-Type": "application/json",
        "Bypass-Tunnel-Reminder": "bypass"
    }
    # Returns json of all avatar versions
    versions = requests.get(url=base, headers=headers).json()
    # Gets the highest possible version
    MaxVer = str(len(versions["versions"]) - 1)
    # Allows user to select what version of the avatar they want
    selection = pymsgbox.confirm(text="Do you want to download the latest or custom version?",title="VRCA Version Select", buttons=["Latest", "Custom"])
    # Takes action depending on what was selected
    if selection == "Latest":
        return f'{base}/{MaxVer}/file'
    if selection == "Custom":
        # In a loop to allow multiple attempts
        while True:
            # Try in while loop because ahhhhhhhhhhhhhhhhhh
            try:
                # Prompts user for desired version of an avatrs
                SelVer = pymsgbox.prompt(
                    f'What version would you like to use?(Between 1-{MaxVer}), cancel will auto to latest version!')
                # What the fuck are you even doing here trying to select nothing? WHo cares you get latest version
                if SelVer == None:
                    SelVer = MaxVer
                    break
                # Ensure the selected number is within a possible range of avatar version
                MainList = range(1, int(MaxVer))
                # If its a valid version break
                if int(SelVer) in MainList:
                    break
            except:
                pass
        # Retruns end desired asset url
        return f'{base}/{SelVer}/file'
#Cleans text to appear in the RawData preview
def CleanText(data):
    try:
        Klean = f"""Time Detected:{datetime.utcfromtimestamp(int(data["TimeDetected"])).strftime('%Y-%m-%d %H:%M:%S')}\nAvatar ID:{data["AvatarID"]}\nAvatar Name:{data["AvatarName"]}\nAvatar Description:{data["AvatarDescription"]}\nAuthor ID:{data["AuthorID"]}\nAuthor Name:{data["AuthorName"]}\nPC Asset URL:{data["PCAssetURL"]}\nQuest Asset URL:{data["QUESTAssetURL"]}\nImage URL:{data["ImageURL"]}\nThumbnail URL:{data["ThumbnailURL"]}\nUnity Version:{data["UnityVersion"]}\nRelease Status:T\nTags:{data["Tags"]}"""
    except:
        Klean = f"""Time Detected:{data["TimeDetected"]}\nAvatar ID:{data["AvatarID"]}\nAvatar Name:{data["AvatarName"]}\nAvatar Description:{data["AvatarDescription"]}\nAuthor ID:{data["AuthorID"]}\nAuthor Name:{data["AuthorName"]}\nPC Asset URL:{data["PCAssetURL"]}\nQuest Asset URL:{data["QUESTAssetURL"]}\nImage URL:{data["ImageURL"]}\nThumbnail URL:{data["ThumbnailURL"]}\nUnity Version:{data["UnityVersion"]}\nRelease Status:TBC\nTags:{data["Tags"]}"""
    return Klean
#Gets data from SelectedAvi in a simpler way
def GetData(SelAvi,param):
    if param == "TimeDetected":
        return SelAvi["TimeDetected"]
    if param == "AvatarID":
        return SelAvi["AvatarID"]
    if param == "AvatarName":
        return SelAvi["AvatarName"]
    if param == "AvatarDescription":
        return SelAvi["AvatarDescription"]
    if param == "AuthorID":
        return SelAvi["AuthorID"]
    if param == "AuthorName":
        return SelAvi["AuthorName"]
    if param == "PCAsset":
        return SelAvi["PCAssetURL"]
    if param == "QAsset":
        return SelAvi["QUESTAssetURL"]
    if param == "IMGURL":
        return SelAvi["ImageURL"]
    if param == "ThumbURL":
        return SelAvi["ThumbnailURL"]
    if param == "UnityVer":
        return SelAvi["UnityVersion"]
    if param == "ReleaseStatus":
        return "TBC"
    if param == "Tags":
        return SelAvi["Tags"]

#Gets image of SelectedAvi in a simpler way
def GetImage(url):
    try:
        #Creates empty payload
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
        # Requests data and sets image
        data = requests.request("GET", url, data=payload, headers=headers)
        return data
    except:
        return ""