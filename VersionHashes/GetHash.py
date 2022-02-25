import hashlib, os

base = os.getcwd()
os.chdir('..')
with open(f"ARES_C_UPDATER\\ARES.UPDATER\\ARES.UPDATER\\bin\\Release\\ARES.Updater.exe", "rb") as f:
    ARESDATA = f.read()
    UpdaterHash = hashlib.sha256(ARESDATA).hexdigest()
    print('Updater Hash: ' + UpdaterHash)
    with open(f'{base}\\ARESUPDATER.txt', 'w+')as f:
        f.write(UpdaterHash)
with open(f"ARES_C\\ARES\\ARES\\bin\\Release\\ARES.exe", "rb") as f:
    ARESDATA = f.read()
    GUIHash = hashlib.sha256(ARESDATA).hexdigest()
    print('GUI Hash: ' + GUIHash)
    with open(f'{base}\\ARESGUI.txt', 'w+')as f:
        f.write(GUIHash)