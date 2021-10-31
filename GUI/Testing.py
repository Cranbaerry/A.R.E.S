import pymsgbox
base = "Test"
MaxVer = "60"
def TrueTest():
    while True:
        SelVer = pymsgbox.prompt(f'What version would you like to use?(Between 1-{MaxVer})')
        MainList = range(1, int(MaxVer))
        if int(SelVer) in MainList:
            break
    return f'{base}/{SelVer}/file'
print(TrueTest())