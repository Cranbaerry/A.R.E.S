#A file containing functions that rely on external processes such as hotswap.exe,
#Unity.exe and 3rd party mods such as EMM

#Imports reqired modules
import os, shutil, tempfile, pymsgbox, requests, re, textwrap, time, traceback
#Importing ARES modules
from CoreUtils import *
#Allows for the creation and execution of the HSB package
def CreateHSB(key,UnityPath):
    try:
        if os.path.isfile("HSBC.rar"):
            return True
        try:
            os.system('taskkill /F /im "Unity Hub.exe"')
        except:
            pass
        try:
            os.system('taskkill /F /im "Unity.exe"')
        except:
            pass
        if os.path.isdir(f"{os.path.expanduser('~')}\\AppData\\Local\\Temp\\DefaultCompany\\HSB"):
            shutil.rmtree(f"{os.path.expanduser('~')}\\AppData\\Local\\Temp\\DefaultCompany\\HSB", ignore_errors=True)
        if os.path.isdir(f"{os.path.expanduser('~')}\\AppData\\LocalLow\\DefaultCompany\\HSB"):
            shutil.rmtree(f"{os.path.expanduser('~')}\\AppData\\LocalLow\\DefaultCompany\\HSB", ignore_errors=True)
        if os.path.isdir("HSB"):
            try:
                shutil.rmtree("HSB", ignore_errors=True)
            except:
                os.rmdir("HSB")
        # Create HSB directory
        os.mkdir('HSB')
        # Extracts HSB to folder
        os.system("UnRAR.exe x HSB.rar HSB -id[c,d,n,p,q]")
        # Launches unity
        os.system(f'"{UnityPath}" -ProjectPath HSB')
        shutil.move("HSB\\Assets\\ARES SMART\\Resources\\.CurrentLayout-default.dwlt","HSB\\Library\\CurrentLayout-default.dwlt")
        os.system("Rar.exe a HSBC.rar HSB")
        if os.path.isdir("HSB"):
            try:
                shutil.rmtree("HSB", ignore_errors=True)
            except:
                os.rmdir("HSB")
        pymsgbox.alert("Succesfully created HSB!")
        return True
    except:
        EventLog("Error in HSB creation:\n{traceback.format_exc()}")
        ErrorLog(key,f"Error in HSB creation:\n{traceback.format_exc()}")
        return False
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
        if os.path.isdir(f"{os.path.expanduser('~')}\\AppData\\Local\\Temp\\DefaultCompany\\HSB"):
            shutil.rmtree(f"{os.path.expanduser('~')}\\AppData\\Local\\Temp\\DefaultCompany\\HSB", ignore_errors=True)
            EventLog("Deleted TMP HSB")
        if os.path.isdir(f"{os.path.expanduser('~')}\\AppData\\LocalLow\\DefaultCompany\\HSB"):
            shutil.rmtree(f"{os.path.expanduser('~')}\\AppData\\LocalLow\\DefaultCompany\\HSB", ignore_errors=True)
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
        os.system("UnRAR.exe x HSBC.rar HSB -id[c,d,n,p,q]")
        EventLog("Extracted HSB")
        #Launches unity
        os.system(f'"{UnityPath}" -ProjectPath HSB\\HSB')
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
    base = os.getcwd()
    print(base)
    try:
        cla.StatusL.setText(f"Status: Hotswap started!")
        if os.path.exists(f"{base}\\HOTSWAP\\decompressed.vrca"):
            os.remove(f"{base}\\HOTSWAP\\decompressed.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\decompressed1.vrca"):
            os.remove(f"{base}\\HOTSWAP\\decompressed1.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\dummy.vrca"):
            os.remove(f"{base}\\HOTSWAP\\dummy.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\target.vrca"):
            os.remove(f"{base}\\HOTSWAP\\target.vrca")
        cla.StatusL.setText(f"Status: Cleaned working enviroment!")
        dummyvrcapath = f"{os.path.expanduser('~')}\\AppData\\Local\\Temp\\DefaultCompany\\HSB\\custom.vrca"
        shutil.copy(dummyvrcapath, f"{base}\\HOTSWAP\\dummy.vrca")
        print("Copied dummy")
        cla.StatusL.setText(f"Status: Decompressing dummy VRCA...")
        os.chdir("HOTSWAP")
        os.system(f"HOTSWAP.exe d dummy.vrca decompressed.vrca")
        EventLog("Decompressed dummy vrca!")
        with open(f"{base}\\HOTSWAP\\decompressed.vrca", "rb") as f:
            DummyData = f.read()
        #Extracts avatar ID and CAB
        NewID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(DummyData)).group(1)
        cla.StatusL.setText(f"Status: New avatar ID found!")
        print(f"Got new ID: {NewID}")
        NewCAB = re.search("(CAB-[\w\d]{32})", str(DummyData)).group(1)
        cla.StatusL.setText(f"Status: New CAB found!")
        print(f"Got new CAB: {NewCAB}")
        EventLog("New Info: " + NewCAB + " | " + NewID)
        cla.StatusL.setText(f"Status: Decompressing target avatar...")
        os.system(f"HOTSWAP.exe d Avatar.vrca decompressed.vrca")
        EventLog("Decompressed avatar!")
        with open(f"{base}\\HOTSWAP\\decompressed.vrca", "rb") as f:
            AviData = f.read()
        # Extracts avatar ID and CAB
        OldID = re.search("(avtr_[\w\d]{8}-[\w\d]{4}-[\w\d]{4}-[\w\d]{4}-[\w\d]{12})", str(AviData)).group(1)
        cla.StatusL.setText(f"Status: Old avatar ID found!")
        print(f"Old ID: {OldID}")
        OldCAB = re.search("(CAB-[\w\d]{32})", str(AviData)).group(1)
        cla.StatusL.setText(f"Status: Old CAB found!")
        print(f"Old CAB: {OldCAB}")
        EventLog("Old Info: " + OldCAB + " | " + OldID)
        #Replaces old avatar ID and CAB
        AviData = AviData.replace(bytes(OldID, 'utf-8'), bytes(NewID, 'utf-8'))
        AviData = AviData.replace(bytes(OldCAB, 'utf-8'), bytes(NewCAB, 'utf-8'))
        EventLog("Data replaced!")
        #Write to new file
        with open(f"{base}\\HOTSWAP\\decompressed1.vrca", "wb") as f:
            f.write(AviData)
        cla.StatusL.setText(f"Status: Data replaced and written!")
        #Compresses final avatar
        cla.StatusL.setText(f"Status: Compressing final avatar...")
        os.system(f"HOTSWAP.exe c decompressed1.vrca target.vrca")
        EventLog("Final avatar compressed!")
        cla.StatusL.setText(f"Status: Getting file sizes...")
        decompsize = textwrap.shorten(str(os.path.getsize(f'{base}\\HOTSWAP\\decompressed1.vrca') / (1024 * 1024)), width=5,placeholder="")
        cla.StatusL.setText(f"Status: Mopping the floor...")
        if os.path.exists(f"{base}\\HOTSWAP\\decompressed.vrca"):
            os.remove(f"{base}\\HOTSWAP\\decompressed.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\Avatar.vrca"):
            os.remove(f"{base}\\HOTSWAP\\Avatar.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\decompressed1.vrca"):
            os.remove(f"{base}\\HOTSWAP\\decompressed1.vrca")
        if os.path.exists(f"{base}\\HOTSWAP\\dummy.vrca"):
            os.remove(f"{base}\\HOTSWAP\\dummy.vrca")
        EventLog("Cleaned!")
        cla.StatusL.setText(f"Status: VRCA Renamed!")
        shutil.move(f"{base}\\HOTSWAP\\target.vrca", dummyvrcapath)
        compsize = textwrap.shorten(str(os.path.getsize(dummyvrcapath) / (1024 * 1024)), width=5, placeholder="")
        print(f"Got file sizes, comp:{compsize}MB, decomp:{decompsize}MB")
        os.chdir(base)
        pymsgbox.alert(f'Hotswap complete!\nSizes:\nCompressed:{compsize}MB|Decompressed:{decompsize}MB')
        EventLog("Thread closed and hotswap complete!")
        cla.Hotswap.setEnabled(True)
        cla.StatusL.setText(f"Status: Idle")
    except:
        traceback.print_exc()
        os.chdir(base)
        EventLog(f"An error occured during hotswaping {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during hotswap: {traceback.format_exc()}")
        cla.Hotswap.setEnabled(True)
#Function to repair a cached VRCA
def RepairVRCA(cla):
    base = os.getcwd()
    try:
        cla.StatusL.setText(f"Status: Starting repair...")
        #Enters the HOTSWAP directory
        os.chdir("HOTSWAP")
        #Decompresses the VRCA fully
        cla.StatusL.setText(f"Status: Decompressing VRCA...")
        os.system('HOTSWAP.exe d bad.vrca baddecomp.vrca')
        cla.StatusL.setText(f"Status: Compressing VRCA...")
        os.system('HOTSWAP.exe c baddecomp.vrca Repaired.vrca')
        cla.StatusL.setText(f"Status: Cleaning files...")
        # Rename file/remove old variants
        if os.path.exists("bad.vrca"):
            os.remove("bad.vrca")
        if os.path.exists("baddecomp.vrca"):
            os.remove("baddecomp.vrca")
        if not os.path.exists("Repaired"):
            os.mkdir("Repaired")
        if os.path.isfile("Repaired\\Repaired.vrca"):
            os.remove("Repaired\\Repaired.vrca")
        #Move the repaired file into the "Repaired" folder
        shutil.move("Repaired.vrca", "Repaired\\Repaired.vrca")
        #Opens the "Repaired" folder
        os.system(f'explorer Repaired')
        #Returns to main file directory and informs the user their VRCA has been repaired
        os.chdir(base)
        pymsgbox.alert("VRCA Repaired!")
        time.sleep(10)
        cla.StatusL.setText(f"Status: Idle")
        EventLog("Thread closed and repair complete!")
        cla.RepairVRCA.setEnabled(True)
    except:
        os.chdir(base)
        EventLog(f"An error occured during repair: {traceback.format_exc()}")
        pymsgbox.alert(f"An error occured during repair: {traceback.format_exc()}")
        cla.RepairVRCA.setEnabled(True)
#Function to extract VRCAs
def ExtractVRCA(ExtValue,cla):
    Base = os.getcwd()
    try:
        if os.path.isdir("AssetRipperConsole_win64(ds5678)\\Ripped"):
            EventLog("Old rip found!")
            try:
                shutil.rmtree("AssetRipperConsole_win64(ds5678)\\Ripped", ignore_errors=True)
                EventLog("Old rip deleted!")
            except:
                os.rmdir("AssetRipperConsole_win64(ds5678)\\Ripped")
                EventLog("Old rip deleted!")
        os.chdir("AssetRipperConsole_win64(ds5678)")
        EventLog("In asset ripper folder!")
        os.system(f'AssetRipperConsole.exe Avatar.vrca {ExtValue} -q')
        EventLog("Extract complete! Cleaning...")
        if os.path.isdir(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\Assets\\Shader'):
            os.rename(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\Assets\\Shader',f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\Assets\\.Shader')
        if os.path.isdir(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\Assets\\Scripts'):
            shutil.rmtree(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\Assets\\Scripts')
        if os.path.isdir(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\AssetRipper'):
            if os.path.isdir(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\AssetRipper\\GameAssemblies'):
                shutil.rmtree(f'{Base}\\AssetRipperConsole_win64(ds5678)\\Ripped\\AssetRipper\\GameAssemblies', ignore_errors=True)
        os.chdir(Base + "\\AssetRipperConsole_win64(ds5678)")
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