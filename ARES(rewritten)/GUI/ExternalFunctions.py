#A file containing functions that rely on external processes such as hotswap.exe,
#Unity.exe and 3rd party mods such as EMM

#Imports reqired modules
import os, shutil, tempfile, pymsgbox, requests, re, textwrap, time, traceback, getpass
#Importing ARES modules
from CoreUtils import *
#Allows for the creation and execution of the HSB package
def OpenUnity(UnityPath,cla):
    Base = os.getcwd()
    try:
        # Kills unity to avoid issues
        try:
            os.system('taskkill /F /im "Unity Hub.exe"')
            EventLog("Killed UnityHub!")
        except:
            pass
        try:
            os.system('taskkill /F /im "Unity.exe"')
            EventLog("Killed Unity!")
        except:
            pass
        # Remove any traces of HSB if any are present
        if os.path.isdir(f"C:\\Users\\{getpass.getuser()}\\AppData\\Local\\Temp\\DefaultCompany\\HSB"):
            shutil.rmtree(f"C:\\Users\\{getpass.getuser()}\\AppData\\Local\\Temp\\DefaultCompany\\HSB", ignore_errors=True)
            EventLog("Deleted TMP HSB")
        if os.path.isdir("HSB"):
            try:
                shutil.rmtree("HSB", ignore_errors=True)
                EventLog("Deleted HSB project!")
            except:
                os.rmdir("HSB")
                EventLog("Deleted HSB project!")
        # Create HSB directory
        os.mkdir('HSB')
        # Extracts HSB to folder
        os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
        EventLog("Extracted HSB")
        #Launches unity
        os.system(f'"{UnityPath}" -ProjectPath HSB')
        EventLog("HSB opened!")
        EventLog("Thread closed and unity opened!")
        # Enables button again
        cla.OpenUnity.setEnabled(True)
        cla.Hotswap.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(f"An error occured during unity launch: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during unity launch: {traceback.format_exc()}")
        cla.OpenUnity.setEnabled(True)
        cla.Hotswap.setEnabled(False)
#Allows VRCA files to be hotswapped withing the HSB
def Hotswap(cla):
    Base = os.getcwd()
    try:
        #Enables progress bar
        base = os.getcwd()
        #Ensures hotswap enviroment is clean
        os.chdir("HOTSWAP")
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        if os.path.exists("decompressed1.vrca"):
            os.remove("decompressed1.vrca")
        dummyvrcapath = f"C:\\Users\\{getpass.getuser()}\\AppData\\Local\\Temp\\DefaultCompany\\HSB\\custom.vrca"
        os.system(f"HOTSWAP.exe d {dummyvrcapath}")
        EventLog("Decompressed dummy vrca!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        print("DIR: " + os.getcwd())
        with open("decompressed.vrca", "rb") as f:
            DummyData = f.read()
        #Extracts avatar ID and CAB
        NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(DummyData)).group(1)
        NewCAB = re.search("(CAB-[\w\d]{32})", str(DummyData)).group(1)
        EventLog("New Info: " + NewCAB + " | " + NewID)
        os.system("HOTSWAP.exe d Avatar.vrca")
        EventLog("Decompressed avatar!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        with open("decompressed.vrca", "rb") as f:
            AviData = f.read()
        # Extracts avatar ID and CAB
        OldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(AviData)).group(1)
        OldCAB = re.search("(CAB-[\w\d]{32})", str(AviData)).group(1)
        EventLog("Old Info: " + OldCAB + " | " + OldID)
        #Replaces old avatar ID and CAB
        AviData = AviData.replace(bytes(OldID, 'utf-8'), bytes(NewID, 'utf-8'))
        AviData = AviData.replace(bytes(OldCAB, 'utf-8'), bytes(NewCAB, 'utf-8'))
        EventLog("Data replaced!")
        #Write to new file
        with open("decompressed1.vrca", "wb") as f:
            f.write(AviData)
        #Compresses final avatar
        os.system("HOTSWAP.exe c decompressed1.vrca")
        EventLog("Final avatar compressed!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        compsize = textwrap.shorten(str(os.path.getsize("compressed.vrca") / (1024 * 1024)), width=5, placeholder="")
        decompsize = textwrap.shorten(str(os.path.getsize("decompressed1.vrca") / (1024 * 1024)), width=5,placeholder="")
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        if os.path.exists("decompressed1.vrca"):
            os.remove("decompressed1.vrca")
        if os.path.exists("Avatar.vrca"):
            os.remove("Avatar.vrca")
        EventLog("Cleaned!")
        os.rename("compressed.vrca", "custom.vrca")
        shutil.move("custom.vrca", dummyvrcapath)
        os.chdir(base)
        pymsgbox.alert(f'Hotswap complete!\nSizes:\nCompressed:{compsize}MB|Decompressed:{decompsize}MB')
        EventLog("Thread closed and hotswap complete!")
        cla.Hotswap.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(f"An error occured during hotswaping {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during hotswa[: {traceback.format_exc()}")
        cla.Hotswap.setEnabled(True)
#Function to repair a cached VRCA
def RepairVRCA(cla):
    Base = os.getcwd()
    try:
        cla.ProgBar.setEnabled(True)
        cla.ProgBar.setValue(0)
        #Enters the HOTSWAP directory
        os.chdir("HOTSWAP")
        #Decompresses the VRCA fully
        os.system('HOTSWAP.exe d bad.vrca')
        cla.ProgBar.setValue(33)
        os.chdir(Base)
        #Compresses the VRCA fully
        os.chdir("HOTSWAP")
        os.system('HOTSWAP.exe c decompressed.vrca')
        cla.ProgBar.setValue(66)
        # Rename file/remove old variants
        if os.path.exists("bad.vrca"):
            os.remove("bad.vrca")
        if os.path.exists("Repaired.vrca"):
            os.remove("Repaired.vrca")
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        os.rename("compressed.vrca", "Repaired.vrca")
        if not os.path.exists("Repaired"):
            os.mkdir("Repaired")
        os.chdir("Repaired")
        if os.path.exists("Repaired.vrca"):
            os.remove("Repaired.vrca")
        os.chdir("..")
        #Move the repaired file into the "Repaired" folder
        shutil.move("Repaired.vrca", "Repaired/Repaired.vrca")
        cla.ProgBar.setValue(100)
        #Opens the "Repaired" folder
        os.system(f'explorer Repaired')
        #Returns to main file directory and informs the user their VRCA has been repaired
        os.chdir("..")
        pymsgbox.alert("VRCA Repaired!")
        time.sleep(10)
        cla.ProgBar.setEnabled(False)
        cla.ProgBar.setValue(0)
        EventLog("Thread closed and repair complete!")
        cla.RepairVRCA.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(f"An error occured during repair: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during repair: {traceback.format_exc()}")
        cla.RepairVRCA.setEnabled(True)
#Function to extract VRCAs
def ExtractVRCA(ExtValue,cla):
    Base = os.getcwd()
    try:
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog("In asset ripper folder")
        if os.path.isdir("Ripped"):
            EventLog("Old rip found!")
            try:
                shutil.rmtree("Ripped", ignore_errors=True)
                EventLog("Old rip deleted!")
            except:
                os.rmdir("Ripped")
                EventLog("Old rip deleted!")
        os.chdir(Base)
        EventLog("Returned to base!")
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog("In asset ripper folder!")
        os.system(f'AssetRipperConsole.exe Avatar.vrca {ExtValue} -q')
        EventLog("Extract complete! Cleaning...")
        os.chdir(Base)
        os.chdir("AssetRipperConsole_win64(ds5678)")
        os.chdir("Ripped")
        os.chdir("Assets")
        os.rename("Shader",".Shader")
        shutil.rmtree("Scripts")
        os.chdir(Base)
        os.chdir("AssetRipperConsole_win64(ds5678)")
        os.chdir("Ripped")
        os.chdir("AssetRipper")
        shutil.rmtree("GameAssemblies", ignore_errors=True)
        os.chdir(Base)
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog("Cleaned! Opening now...")
        os.system(f'explorer Ripped')
        os.chdir(Base)
        EventLog("Thread code ended!")
        cla.ExtVRCA.setEnabled(True)
        pymsgbox.alert("Extract complete!")
    except:
        os.chdir(Base)
        EventLog(f"An error occured during extraction: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during extraction: {traceback.format_exc()}")
        cla.ExtVRCA.setEnabled(True)