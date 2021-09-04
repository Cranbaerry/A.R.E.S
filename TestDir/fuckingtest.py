import subprocess, os, shutil

def ExtractVRCA():
    output = subprocess.getoutput(f'HOTSWAP.exe d {cum}')
    print(output)
    if "2019" in output:
        print("Is 2019 using Asset Ripper")
        print("This will result in a broken prefab")
        print("Making a full rip is not possible")
        shutil.copy(cum, f'AssetRipperConsole_win64(ds5678)/{cum}')
        os.chdir("AssetRipperConsole_win64(ds5678)")
        output = subprocess.getoutput(f'AssetRipperConsole.exe {cum} -q')
        print(output)
        os.rename("Ripped", cum.replace(".vrca", ""))
        print("Extracted!")
    else:
        print("Is below 2019")

cum = "2019.vrca"
ExtractVRCA()
cum = "2018.vrca"
ExtractVRCA()