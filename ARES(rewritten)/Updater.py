import hashlib, requests, subprocess, os, traceback
GUIP = "GUI"
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
if os.path.isdir(GUIP):
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
        print(f"ARES is couldn't verify version! Launching...")
    else:
        if InstalledHash == LatestHash:
            os.chdir(GUIP)
            subprocess.Popen("ARES.exe")
            print(f"ARES is up-to-date! Launching...")
        else:
            print(f"ARES is out-of-date! Updating...")
            payload = ""
            headers = {
                "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
                "Content-Type": "application/json",
                "Bypass-Tunnel-Reminder": "bypass"
            }
            data = requests.request("GET", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/UnRAR.exe",data=payload, headers=headers, stream=True)
            with open("UnRar.exe", "wb") as v:
                v.write(data.content)
            data = requests.request("GET", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/GUI.rar",data=payload, headers=headers, stream=True)
            with open("GUI.rar", "wb") as v:
                v.write(data.content)
            os.system(f"UnRAR.exe x GUI.rar {GUIP} -id[c,d,n,p,q] -O+")
            os.chdir(GUIP)
            subprocess.Popen("ARES.exe")
            print("ARES updated! Opening...")
else:
    print("ARES not installed! Installing...")
    payload = ""
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
        "Content-Type": "application/json",
        "Bypass-Tunnel-Reminder": "bypass"
    }
    data = requests.request("GET", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/UnRAR.exe", data=payload, headers=headers, stream=True)
    with open("UnRar.exe", "wb") as v:
        v.write(data.content)
    data = requests.request("GET", "https://github.com/LargestBoi/A.R.E.S/releases/latest/download/GUI.rar", data=payload, headers=headers, stream=True)
    with open("GUI.rar", "wb") as v:
        v.write(data.content)
    os.mkdir(GUIP)
    os.system(f"UnRAR.exe x GUI.rar {GUIP} -id[c,d,n,p,q]")
    os.chdir(GUIP)
    subprocess.Popen("ARES.exe")
    print("ARES installed! Opening...")