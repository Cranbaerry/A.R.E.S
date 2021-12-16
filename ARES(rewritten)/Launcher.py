import hashlib, requests

with open("ARES.exe", "rb") as f:
    bytes = f.read()
    InstalledHash = hashlib.sha256(bytes).hexdigest()
    print(f'Installed: {InstalledHash}')
try:
    LatestHash = requests.get("https://pastebin.com/raw/ZupUSf12", timeout=10).text
except:
    LatestHash = "Couldn't Connect!"
print(f'Latest: {LatestHash}')
if InstalledHash == LatestHash:
    print("Up to date!")
else:
    print("Out of date!")