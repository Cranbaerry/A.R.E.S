import os
print("This is the Avatar Logger Locator")
print("V1 By LargestBoi")
print("This tool can be used to search the logs created by the avatar logger tool (Also on this GitHub!)")
print("The keyword can be anything such as:")
print("The Avatar ID (avtr_xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)")
print("Avatar Name")
print("Avatar Author ID (usr_xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)")
print("Avatar Author Name")
print("In return you will be given the line where your keyword appears in the logs, thi can then be used to open the .txt log file and locate all information pulled from the avatar!")
def Search():
    Key = input("Enter your keword search term:")
    line_number = 0
    with open("Private.txt", 'r', encoding="latin-1") as read_obj:
        print("Reading private log...")
        for line in read_obj:
            line_number += 1
            if Key in line:
                print(f'Line Number (Private): {line_number}')
    line_number = 0
    with open("Public.txt", 'r', encoding="latin-1") as read_obj:
        print("Reading public log...")
        for line in read_obj:
            line_number += 1
            if Key in line:
                print(f'Line Number (Public): {line_number}')
    print("Search Complete!")
    Search()
Search()