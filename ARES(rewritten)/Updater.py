import hashlib, requests, subprocess, os, traceback
from clint.textui import progress
GUIP = "GUI"
if os.path.isfile("VRChat.exe"):
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
        os.system('taskkill /F /im "AssetRipperConsole.exe"')
    except:
        pass
    if os.path.isfile(GUIP + "\\ARES.exe"):
        print("Getting installed hash...")
        with open(f"{GUIP}\\ARES.exe", "rb") as f:
            ARESDATA = f.read()
            InstalledHash = hashlib.sha256(ARESDATA).hexdigest()
            print(f"Installed hash: {InstalledHash}")
        try:
            LatestHash = requests.get("https://raw.githubusercontent.com/LargestBoi/A.R.E.S/main/VersionHashes/ARESGUI.txt", timeout=10).text
            print(f"GitHub Hash: {LatestHash}")
        except:
            LatestHash = "Couldn't Connect!"
            print(f"Failed to connect to GitHub: \n{traceback.format_exc()}")
        if LatestHash == "Couldn't Connect!":
            os.chdir(GUIP)
            subprocess.Popen("ARES.exe")
            print(f"ARES couldn't verify version! Launching...")
        else:
            if InstalledHash == LatestHash:
                os.chdir(GUIP)
                subprocess.Popen("ARES.exe")
                print(f"ARES is up-to-date! Launching...")
            else:
                print(f"ARES is out-of-date! Updating...")
                if not os.path.isfile('UnRar.exe'):
                    print(f"Fetching UnRar.exe...")
                    r = requests.get("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/UnRAR.exe", stream=True)
                    with open("UnRar.exe", 'wb') as f:
                        total_length = int(r.headers.get('content-length'))
                        for chunk in progress.bar(r.iter_content(chunk_size=1024), expected_size=(total_length / 1024) + 1):
                            if chunk:
                                f.write(chunk)
                                f.flush()
                print(f"Fetching GUI.rar...")
                r = requests.get("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/GUI.rar", stream=True)
                with open("GUI.rar", 'wb') as f:
                    total_length = int(r.headers.get('content-length'))
                    for chunk in progress.bar(r.iter_content(chunk_size=1024), expected_size=(total_length / 1024) + 1):
                        if chunk:
                            f.write(chunk)
                            f.flush()
                os.system(f"UnRAR.exe x GUI.rar {GUIP} -id[c,d,n,p,q] -O+")
                os.chdir(GUIP)
                subprocess.Popen("ARES.exe")
                print("ARES updated! Opening...")
    else:
        print("ARES not installed! Installing...")
        if not os.path.isfile('UnRar.exe'):
            print(f"Fetching UnRar.exe...")
            r = requests.get("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/UnRAR.exe", stream=True)
            with open("UnRar.exe", 'wb') as f:
                total_length = int(r.headers.get('content-length'))
                for chunk in progress.bar(r.iter_content(chunk_size=1024), expected_size=(total_length / 1024) + 1):
                    if chunk:
                        f.write(chunk)
                        f.flush()
        print(f"Fetching GUI.rar...")
        r = requests.get("https://github.com/LargestBoi/A.R.E.S/releases/latest/download/GUI.rar", stream=True)
        with open("GUI.rar", 'wb') as f:
            total_length = int(r.headers.get('content-length'))
            for chunk in progress.bar(r.iter_content(chunk_size=1024), expected_size=(total_length / 1024) + 1):
                if chunk:
                    f.write(chunk)
                    f.flush()
        if not os.path.isdir(GUIP):
            os.mkdir(GUIP)
        os.system(f"UnRAR.exe x GUI.rar {GUIP} -id[c,d,n,p,q] -O+")
        os.chdir(GUIP)
        subprocess.Popen("ARES.exe")
        print("ARES installed! Opening...")
else:
    input("The updater is not currently in the VRChat folder, please place it\nalongside your 'VRChat.exe' file for optimal preformance!\nYou can just hit enter to close meh!")