import hashlib
with open(f"ARES.exe", "rb") as f:
    ARESDATA = f.read()
    InstalledHash = hashlib.sha256(ARESDATA).hexdigest()
    print(InstalledHash)