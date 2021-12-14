#A file containing functions that rely on external processes such as hotswap.exe,
#Unity.exe and 3rd party mods such as EMM

#Imports reqired modules
import os, shutil, tempfile, pymsgbox, requests, re, textwrap, time, subprocess
#Allows for the creation and execution of the HSB package
def OpenUnity(UnityPath):
    # Kills unity to avoid issues
    try:
        os.system('taskkill /F /im "Unity Hub.exe"')
    except:
        pass
    try:
        os.system('taskkill /F /im "Unity.exe"')
    except:
        pass
    # Remove any traces of HSB if any are present
    if os.path.isdir(tempfile.gettempdir() + "\\DefaultCompany\\HSB"):
        shutil.rmtree(tempfile.gettempdir() + "\\DefaultCompany\\HSB", ignore_errors=True)
    if os.path.isdir("HSB"):
        try:
            shutil.rmtree("HSB", ignore_errors=True)
        except:
            pass
        try:
            os.rmdir("HSB")
        except:
            pass
    # Create HSB directory
    os.mkdir('HSB')
    # Extracts HSB to folder
    os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
    #Launches unity
    os.system(rf'"{UnityPath}" -ProjectPath HSB')
#Patches EMMVRC
def PatchEMM(progbar):
    progbar.setValue(0)
    #Gets URLs from pastebin
    EMMURLS = requests.get("https://pastebin.com/raw/ahNAhVFB", timeout=10).text
    loaderurl = EMMURLS.split("|")[0]
    emmurl = EMMURLS.split("|")[1]
    os.chdir("..")
    #Removes any potential conflicitng files
    progbar.setValue(20)
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
    progbar.setValue(40)
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
    progbar.setValue(60)
    os.chdir("..")
    os.chdir("Dependencies")
    data = requests.request("GET", emmurl, data=payload, headers=headers, stream=True)
    with open("emmVRC.dll", "wb") as v:
        v.write(data.content)
    progbar.setValue(80)
    os.chdir("..")
    os.chdir("GUI")
    progbar.setValue(100)
    time.sleep(10)
    #Infroms the user the process is complete
    pymsgbox.alert("Patched EMM has been installed!")
    progbar.setValue(0)
#Allows VRCA files to be hotswapped withing the HSB
def Hotswap(progbar):
    #Enables progress bar
    progbar.setEnabled(True)
    progbar.setValue(0)
    base = os.getcwd()
    #Ensures hotswap enviroment is clean
    os.chdir("HOTSWAP")
    if os.path.exists("decompressed.vrca"):
        os.remove("decompressed.vrca")
    if os.path.exists("decompressed1.vrca"):
        os.remove("decompressed1.vrca")
    progbar.setValue(9)
    dummyvrcapath = tempfile.gettempdir() + "\\DefaultCompany\\HSB\\custom.vrca"
    os.system(f"HOTSWAP.exe d {dummyvrcapath}")
    os.chdir(base)
    os.chdir("HOTSWAP")
    print("DIR: " + os.getcwd())
    with open("decompressed.vrca", "rb") as f:
        DummyData = f.read()
    progbar.setValue(18)
    #Extracts avatar ID and CAB
    NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(DummyData)).group(1)
    NewCAB = re.search("(CAB-[\w\d]{32})", str(DummyData)).group(1)
    print("New Info: " + NewCAB + " | " + NewID)
    progbar.setValue(27)
    os.system("HOTSWAP.exe d Avatar.vrca")
    os.chdir(base)
    os.chdir("HOTSWAP")
    progbar.setValue(36)
    with open("decompressed.vrca", "rb") as f:
        AviData = f.read()
    progbar.setValue(45)
    # Extracts avatar ID and CAB
    OldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(AviData)).group(1)
    OldCAB = re.search("(CAB-[\w\d]{32})", str(AviData)).group(1)
    print("Old Info: " + OldCAB + " | " + OldID)
    progbar.setValue(54)
    #Replaces old avatar ID and CAB
    AviData = AviData.replace(bytes(OldID, 'utf-8'), bytes(NewID, 'utf-8'))
    AviData = AviData.replace(bytes(OldCAB, 'utf-8'), bytes(NewCAB, 'utf-8'))
    progbar.setValue(63)
    #Write to new file
    with open("decompressed1.vrca", "wb") as f:
        f.write(AviData)
    progbar.setValue(72)
    #Compresses final avatar
    os.system("HOTSWAP.exe c decompressed1.vrca")
    os.chdir(base)
    os.chdir("HOTSWAP")
    compsize = textwrap.shorten(str(os.path.getsize("compressed.vrca") / (1024 * 1024)), width=5, placeholder="")
    decompsize = textwrap.shorten(str(os.path.getsize("decompressed1.vrca") / (1024 * 1024)), width=5,placeholder="")
    progbar.setValue(81)
    if os.path.exists("decompressed.vrca"):
        os.remove("decompressed.vrca")
    if os.path.exists("decompressed1.vrca"):
        os.remove("decompressed1.vrca")
    if os.path.exists("Avatar.vrca"):
        os.remove("Avatar.vrca")
    progbar.setValue(90)
    os.rename("compressed.vrca", "custom.vrca")
    shutil.move("custom.vrca", dummyvrcapath)
    os.chdir(base)
    progbar.setValue(100)
    progbar.setEnabled(False)
    pymsgbox.alert(f'Hotswap complete!\nSizes:\nCompressed:{compsize}MB|Decompressed:{decompsize}MB')
    progbar.setValue(0)
#Function to repair a cached VRCA
def RepairVRCA(progbar):
    progbar.setEnabled(True)
    progbar.setValue(0)
    #Enters the HOTSWAP directory
    os.chdir("HOTSWAP")
    #Decompresses the VRCA fully
    os.system('HOTSWAP.exe d bad.vrca')
    progbar.setValue(33)
    #Compresses the VRCA fully
    os.chdir("HOTSWAP")
    os.system('HOTSWAP.exe c decompressed.vrca')
    progbar.setValue(66)
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
    progbar.setValue(100)
    #Opens the "Repaired" folder
    subprocess.Popen(f'explorer Repaired')
    #Returns to main file directory and informs the user their VRCA has been repaired
    os.chdir("..")
    pymsgbox.alert("VRCA Repaired!")
    time.sleep(10)
    progbar.setEnabled(False)
    progbar.setValue(0)
#Function to extract VRCAs
def ExtractVRCA(ExtValue):
    os.chdir("AssetRipperConsole_win64(ds5678)")
    os.system(f'AssetRipperConsole.exe Avatar.vrca {ExtValue} -q')
    subprocess.Popen(f'explorer Ripped')
    os.chdir("..")