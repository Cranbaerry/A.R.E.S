import pymsgbox
#PC = "https://api.vrchat.cloud/api/1/file/file_2f681ec2-462b-4e8b-913b-e61cb733c034/1/file"
#Q = "https://api.vrchat.cloud/api/1/file/file_ab68ef48-c47b-459c-bd17-b07e6fa04803/2/file"
PC = "cxdffhcggyjNonedfghjfdgyyj"
Q = "fgdhdfhdfffhNonefggnfcghmvgg"
if "None" in PC:
    PC = False
    print(PC)
if "None" in Q:
    Q = False
    print(Q)
response = ''
if PC == True and Q == True:
    selection = pymsgbox.confirm(text="Would you like to hotswap the Quest or PC version of the avatar?", title="VRCA Platform Select", buttons=["PC","Quest"])
    if selection == "PC":
        print(selection)
    if selection == "Quest":
        print(selection)
if PC == False and Q == True:
    print("Q")
if PC == True and Q == False:
    print("PC")