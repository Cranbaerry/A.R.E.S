import hashlib, os
base = os.getcwd()
os.chdir('..')
with open(f"ARES(rewritten)\\dist\\ARES.Updater.exe", "rb") as f:
    ARESDATA = f.read()
    UpdaterHash = hashlib.sha256(ARESDATA).hexdigest()
    print('Updater Hash: ' + UpdaterHash)
    with open(f'{base}\\ARESUPDATER.txt', 'w+')as f:
        f.write(UpdaterHash)
with open(f"ARES(rewritten)\\GUI\\dist\\ARES.exe", "rb") as f:
    ARESDATA = f.read()
    GUIHash = hashlib.sha256(ARESDATA).hexdigest()
    print('GUI Hash: ' + GUIHash)
    with open(f'{base}\\ARESGUI.txt', 'w+')as f:
        f.write(GUIHash)