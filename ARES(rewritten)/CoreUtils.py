#A file containing all the core modules in relation to the GUI itself

#Importing reqired modules
import requests, os, json, pymsgbox, datetime, shutil
from base64 import b64encode
#Importing custom ARES modules
from LogUtils import DecideAssetURL
#Returns weather or not ARES is up to date
# will be removed in the future
def VersionCheck(ver):
    ss = requests.get("https://pastebin.com/raw/w3f0jC9P", timeout=10).text
    if ver != ss:
        return False
    else:
        return True
#Pushes the user to the update portal on our GitHub and close any app that may conflit with the update about to occur
# will be removed in the future
def UpdatePushed():
    os.startfile("https://github.com/LargestBoi/A.R.E.S/releases")
    try:
        os.system('taskkill /F /im "ARES.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "HOTSWAP.exe"')
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
#Fetches special thanks from our pastebin
def GetSpecialThanks():
    try:
        SPTX = requests.get("https://pastebin.com/raw/vayK7gC2", timeout=10).text
    except:
        SPTX = "Couldn't Connect!"
    return SPTX
#Retruns true if the application has completed its first time setup
def IsSetup():
    if not os.path.exists("Logs"):
        os.mkdir("Logs")
    if os.path.exists("Latest.log"):
        shutil.move("Latest.log",f'Logs\\{str(datetime.datetime.now()).replace(":","-")}.txt')
    if os.path.exists("Settings.json"):
        return True
    return False
#Gets the settings information
def GetSettings():
    with open("Settings.json", "r+") as s:
        return json.loads(s.read())
#Saves the settings provided to the settings file
def SaveSettings(settings):
    with open("Settings.json", "w+") as s:
        s.write(json.dumps(settings, indent=4))
#A simple error log handiling system
def SendErrorLogs(error):
    possiblesol = "Not found"
    try:
        kk = requests.get(url="https://pastebin.com/raw/1022jnvn").json()
        for x in kk:
            if x[0] in error:
                possiblesol = x[1]
    except:
        pass
    try:
        pymsgbox.alert(error + "\nPossible Fix: " + possiblesol, 'ID10T')
        dtag = pymsgbox.prompt('What is your Discord Tag for better support?')
        okk = b64encode(str(error + "\nPossible Fix: " + possiblesol + "\nUsername: " + dtag).encode()).decode()
        requests.get("https://api.avataruploader.tk/errors/" + okk)
    except:
        pass
#Logs events
def EventLog(BaseDir, Data):
    CurrentDir = os.getcwd()
    os.chdir(BaseDir)
    log = f'{str(datetime.datetime.now())} | {Data}'
    with open("Latest.log", "a+") as l:
        l.write(f'{log}\n')
    os.chdir(CurrentDir)
    return log
#Function to download VRCAs
def DownloadVRCA(PC,Q):
    os.startfile(DecideAssetURL(PC,Q))
#Function to download VRCAs for internal use
def DownloadVRCAFL(PC,Q):
    payload = ""
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
        "Content-Type": "application/json",
        "Bypass-Tunnel-Reminder": "bypass"
    }
    data = requests.request("GET", DecideAssetURL(PC,Q), data=payload, headers=headers, stream=True)
    # Writes content to file
    with open("HOTSWAP\\Avatar.vrca", "wb") as v:
        v.write(data.content)

#Cleanly exits ARES and any other possibly conflicting software
def CleanExit():
    try:
        os.system('taskkill /F /im "ARES.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "HOTSWAP.exe"')
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