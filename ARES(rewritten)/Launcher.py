import hashlib, requests, pymsgbox, subprocess, os
GUIP = "GUIL"
if os.path.isdir(GUIP):
    with open(f"{GUIP}\\ARES.exe", "rb") as f:
        ARESDATA = f.read()
        InstalledHash = hashlib.sha256(ARESDATA).hexdigest()
    try:
        LatestHash = requests.get("https://pastebin.com/raw/ZupUSf12", timeout=10).text
    except:
        LatestHash = "Couldn't Connect!"
    if LatestHash == "Couldn't Connect!":
        os.chdir(GUIP)
        subprocess.Popen("ARES.exe")
        pymsgbox.alert(f"ARES is couldn't verify version! Launching...")
    else:
        if InstalledHash == LatestHash:
            os.chdir(GUIP)
            subprocess.Popen("ARES.exe")
            pymsgbox.alert(f"ARES is up-to-date! Launching...")
        else:
            pymsgbox.alert(f"ARES is out-of-date! Updating...")
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
            pymsgbox.alert("ARES updated! Opening...")
else:
    pymsgbox.alert("ARES not installed! Installing...")
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
    pymsgbox.alert("ARES installed! Opening...")