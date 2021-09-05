import subprocess, os, shutil

def ExtractVRCA():
    output = subprocess.getoutput(f'HOTSWAP.exe d {cum}')
    print(output)
    if "2019" in output:
        print("Is 2019 using Asset Ripper")
        print("This will result in a broken prefab")
        print("thus making a full rip is not possible")
        shutil.copy(cum, f'AssetRipperConsole_win64(ds5678)/{cum}')
        os.chdir("AssetRipperConsole_win64(ds5678)")
        output = subprocess.getoutput(f'AssetRipperConsole.exe {cum} -q')
        print(output)
        os.rename("Ripped", cum.replace(".vrca", ""))
        os.chdir("..")
        print("Extracted!")
    else:
        print("Is below 2019 using uTinyRipper")
        print("This will result in a working prefab")
        print("thus making a full rip possible")
        shutil.copy(cum, f'uTinyRipper_x64(mafaca)/{cum}')
        os.chdir("uTinyRipper_x64(mafaca)")
        output = subprocess.getoutput(f'uTinyRipper.exe {cum}')
        print(output)
        os.rename("Ripped", cum.replace(".vrca", ""))
        os.chdir("..")
        print("Extracted!")
cum = "2019.vrca"
ExtractVRCA()
cum = "2018.vrca"
ExtractVRCA()