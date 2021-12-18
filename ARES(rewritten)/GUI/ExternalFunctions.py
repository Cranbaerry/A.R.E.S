#A file containing functions that rely on external processes such as hotswap.exe,
#Unity.exe and 3rd party mods such as EMM

#Imports reqired modules
import os, shutil, tempfile, pymsgbox, requests, re, textwrap, time, subprocess, traceback
#Importing ARES modules
from CoreUtils import *
#Allows for the creation and execution of the HSB package
def OpenUnity(UnityPath,cla):
    Base = os.getcwd()
    try:
        # Kills unity to avoid issues
        try:
            os.system('taskkill /F /im "Unity Hub.exe"')
            EventLog(Base,"Killed UnityHub!")
        except:
            pass
        try:
            os.system('taskkill /F /im "Unity.exe"')
            EventLog(Base,"Killed Unity!")
        except:
            pass
        # Remove any traces of HSB if any are present
        if os.path.isdir(tempfile.gettempdir() + "\\DefaultCompany\\HSB"):
            shutil.rmtree(tempfile.gettempdir() + "\\DefaultCompany\\HSB", ignore_errors=True)
            EventLog(Base, "Deleted TMP HSB")
        if os.path.isdir("HSB"):
            try:
                shutil.rmtree("HSB", ignore_errors=True)
                EventLog(Base, "Deleted HSB project!")
            except:
                os.rmdir("HSB")
                EventLog(Base, "Deleted HSB project!")
        # Create HSB directory
        os.mkdir('HSB')
        # Extracts HSB to folder
        os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
        EventLog(Base, "Extracted HSB")
        #Launches unity
        subprocess.Popen(f'"{UnityPath}" -ProjectPath HSB')
        EventLog(Base, "HSB opened!")
        EventLog(Base,"Thread closed and unity opened!")
        # Enables button again
        cla.OpenUnity.setEnabled(True)
        cla.Hotswap.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(Base, f"An error occured during unity launch: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during unity launch: {traceback.format_exc()}")
        cla.OpenUnity.setEnabled(True)
        cla.Hotswap.setEnabled(False)
#Patches EMMVRC
def PatchEMM(cla):
    Base = os.getcwd()
    try:
        cla.ProgBar.setValue(0)
        #Gets URLs from pastebin
        EMMURLS = requests.get("https://pastebin.com/raw/ahNAhVFB", timeout=10).text
        loaderurl = EMMURLS.split("|")[0]
        emmurl = EMMURLS.split("|")[1]
        os.chdir("..")
        #Removes any potential conflicitng files
        cla.ProgBar.setValue(20)
        if os.path.exists("Plugins"):
            os.chdir("Plugins")
            if os.path.exists("emmVRC.dll"):
                os.remove("emmVRC.dll")
            os.chdir("..")
        os.chdir("..")
        os.chdir("Mods")
        if os.path.exists("emmVRCLoader.dll"):
            os.remove("emmVRCLoader.dll")
        os.chdir("..")
        if os.path.exists("Dependencies"):
            os.chdir("Dependencies")
            if os.path.exists("emmVRC.dll"):
                os.remove("emmVRC.dll")
            os.chdir("..")
        #Enters correct folders and downloads reqired files
        cla.ProgBar.setValue(40)
        os.chdir("Mods")
        payload = ""
        headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
            "Content-Type": "application/json",
            "Bypass-Tunnel-Reminder": "bypass"
        }
        data = requests.request("GET", loaderurl, data=payload, headers=headers, stream=True)
        with open("emmVRCLoader.dll", "wb") as v:
            v.write(data.content)
        cla.ProgBar.setValue(60)
        os.chdir("..")
        os.chdir("Dependencies")
        data = requests.request("GET", emmurl, data=payload, headers=headers, stream=True)
        with open("emmVRC.dll", "wb") as v:
            v.write(data.content)
        cla.ProgBar.setValue(80)
        os.chdir("..")
        os.chdir("GUI")
        cla.ProgBar.setValue(100)
        time.sleep(10)
        #Infroms the user the process is complete
        pymsgbox.alert("Patched EMM has been installed!")
        cla.ProgBar.setValue(0)
        EventLog(Base,"Thread closed and EMM patched!")
        cla.PatchEMM.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(Base, f"An error occured during EMM patching: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during patch: {traceback.format_exc()}")
        cla.PatchEMM.setEnabled(True)
#Allows VRCA files to be hotswapped withing the HSB
def Hotswap(cla):
    Base = os.getcwd()
    try:
        #Enables progress bar
        cla.ProgBar.setEnabled(True)
        cla.ProgBar.setValue(0)
        base = os.getcwd()
        #Ensures hotswap enviroment is clean
        os.chdir("HOTSWAP")
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        if os.path.exists("decompressed1.vrca"):
            os.remove("decompressed1.vrca")
        cla.ProgBar.setValue(9)
        dummyvrcapath = tempfile.gettempdir() + "\\DefaultCompany\\HSB\\custom.vrca"
        os.system(f"HOTSWAP.exe d {dummyvrcapath}")
        EventLog(Base, "Decompressed dummy vrca!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        print("DIR: " + os.getcwd())
        with open("decompressed.vrca", "rb") as f:
            DummyData = f.read()
        cla.ProgBar.setValue(18)
        #Extracts avatar ID and CAB
        NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(DummyData)).group(1)
        NewCAB = re.search("(CAB-[\w\d]{32})", str(DummyData)).group(1)
        EventLog(Base, "New Info: " + NewCAB + " | " + NewID)
        cla.ProgBar.setValue(27)
        os.system("HOTSWAP.exe d Avatar.vrca")
        EventLog(Base, "Decompressed avatar!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        cla.ProgBar.setValue(36)
        with open("decompressed.vrca", "rb") as f:
            AviData = f.read()
        cla.ProgBar.setValue(45)
        # Extracts avatar ID and CAB
        OldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(AviData)).group(1)
        OldCAB = re.search("(CAB-[\w\d]{32})", str(AviData)).group(1)
        EventLog(Base,"Old Info: " + OldCAB + " | " + OldID)
        cla.ProgBar.setValue(54)
        #Replaces old avatar ID and CAB
        AviData = AviData.replace(bytes(OldID, 'utf-8'), bytes(NewID, 'utf-8'))
        AviData = AviData.replace(bytes(OldCAB, 'utf-8'), bytes(NewCAB, 'utf-8'))
        EventLog(Base, "Data replaced!")
        cla.ProgBar.setValue(63)
        #Write to new file
        with open("decompressed1.vrca", "wb") as f:
            f.write(AviData)
        cla.ProgBar.setValue(72)
        #Compresses final avatar
        os.system("HOTSWAP.exe c decompressed1.vrca")
        EventLog(Base, "Final avatar compressed!")
        os.chdir(base)
        os.chdir("HOTSWAP")
        compsize = textwrap.shorten(str(os.path.getsize("compressed.vrca") / (1024 * 1024)), width=5, placeholder="")
        decompsize = textwrap.shorten(str(os.path.getsize("decompressed1.vrca") / (1024 * 1024)), width=5,placeholder="")
        cla.ProgBar.setValue(81)
        if os.path.exists("decompressed.vrca"):
            os.remove("decompressed.vrca")
        if os.path.exists("decompressed1.vrca"):
            os.remove("decompressed1.vrca")
        if os.path.exists("Avatar.vrca"):
            os.remove("Avatar.vrca")
        EventLog(Base, "Cleaned!")
        cla.ProgBar.setValue(90)
        os.rename("compressed.vrca", "custom.vrca")
        shutil.move("custom.vrca", dummyvrcapath)
        os.chdir(base)
        cla.ProgBar.setValue(100)
        cla.ProgBar.setEnabled(False)
        pymsgbox.alert(f'Hotswap complete!\nSizes:\nCompressed:{compsize}MB|Decompressed:{decompsize}MB')
        cla.ProgBar.setValue(0)
        EventLog(Base,"Thread closed and hotswap complete!")
        cla.Hotswap.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(Base, f"An error occured during hotswaping {traceback.format_exc()}")
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
        subprocess.Popen(f'explorer Repaired')
        #Returns to main file directory and informs the user their VRCA has been repaired
        os.chdir("..")
        pymsgbox.alert("VRCA Repaired!")
        time.sleep(10)
        cla.ProgBar.setEnabled(False)
        cla.ProgBar.setValue(0)
        EventLog(Base,"Thread closed and repair complete!")
        cla.RepairVRCA.setEnabled(True)
    except:
        os.chdir(Base)
        EventLog(Base, f"An error occured during repair: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during repair: {traceback.format_exc()}")
        cla.RepairVRCA.setEnabled(True)
#Function to extract VRCAs
def ExtractVRCA(ExtValue,cla):
    Base = os.getcwd()
    try:
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog(Base, "In asset ripper folder")
        if os.path.isdir("Ripped"):
            EventLog(Base, "Old rip found!")
            try:
                shutil.rmtree("Ripped", ignore_errors=True)
                EventLog(Base, "Old rip deleted!")
            except:
                os.rmdir("Ripped")
                EventLog(Base, "Old rip deleted!")
        os.chdir(Base)
        EventLog(Base, "Returned to base!")
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog(Base, "In asset ripper folder!")
        os.system(f'AssetRipperConsole.exe Avatar.vrca {ExtValue} -q')
        EventLog(Base, "Extract complete! Cleaning...")
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
        EventLog(Base, "Cleaned! Opening now...")
        subprocess.Popen(f'explorer Ripped')
        os.chdir(Base)
        EventLog(Base, "Thread code ended!")
        cla.ExtVRCA.setEnabled(True)
        pymsgbox.alert("Extract complete!")
    except:
        os.chdir(Base)
        EventLog(Base, f"An error occured during extraction: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during extraction: {traceback.format_exc()}")
        cla.ExtVRCA.setEnabled(True)