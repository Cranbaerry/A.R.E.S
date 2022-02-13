import os, requests, threading, traceback
from CoreUtils import LoadLog,EventLog

lock = threading.Lock()

def UploadAvatar(avatarlist, key):
    try:
        for x in avatarlist:
            if x[1] not in uploaded:
                try:
                    headers = {
                        'accept': 'application/json',
                        'content-type': 'application/json',
                        'user-agent': key,
                    }
                    data = {"TimeDetected": x[0],
                            "AvatarID": x[1],
                            "AvatarName": x[2],
                            "AvatarDescription": x[3],
                            "AuthorID": x[4],
                            "AuthorName": x[5],
                            "PCAssetURL": x[6],
                            "QUESTAssetURL": x[7],
                            "ImageURL": x[8],
                            "ThumbnailURL": x[9],
                            "UnityVersion": x[10],
                            "Releasestatus": x[11],
                            "Tags": x[12]}

                    response = requests.post('http://avatarlogger.tk/records/Avatars', headers=headers, json=data)
                    if type(response.json()) == int:
                        lock.acquire()
                        with open(f"{bd}\\Uploaded.txt","a+", errors="ignore") as f:
                            f.writelines(x[1] + "\n")
                        lock.release()
                    elif response.json()["message"] == "Duplicate key exception":
                        lock.acquire()
                        with open(f"{bd}\\Uploaded.txt","a+", errors="ignore") as f:
                            f.writelines(x[1] + "\n")
                        lock.release()
                except:
                    EventLog("Error occured whilst uploading avatar:\n" + traceback.format_exc())
                    EventLog(data)
                    EventLog(response.json())
    except:
        EventLog("Error occured whilst uploading avatar:\n" + traceback.format_exc())

def StartUploads(key):
    global uploaded
    global bd
    try:
        avatarlist = LoadLog()
        avatarlist1 = avatarlist[0:len(avatarlist) // 5]
        avatarlist2 = avatarlist[len(avatarlist) // 5:len(avatarlist) // 5 * 2]
        avatarlist3 = avatarlist[len(avatarlist) // 5 * 2:len(avatarlist) // 5 * 3]
        avatarlist4 = avatarlist[len(avatarlist) // 5 * 3:len(avatarlist) // 5 * 4]
        avatarlist5 = avatarlist[len(avatarlist) // 5 * 4:len(avatarlist)]
        bd = os.getcwd()
        if not os.path.isfile(f"{bd}\\Uploaded.txt"):
            with open(f"{bd}\\Uploaded.txt", "a+"):
                pass
        with open(f"{bd}\\Uploaded.txt","r+", errors="ignore") as f:
            uploaded = f.readlines()
            uploaded = [x.strip() for x in uploaded]
        t1 = threading.Thread(target=UploadAvatar, args=(avatarlist1,key))
        t1.start()
        t2 = threading.Thread(target=UploadAvatar, args=(avatarlist2,key))
        t2.start()
        t3 = threading.Thread(target=UploadAvatar, args=(avatarlist3,key))
        t3.start()
        t4 = threading.Thread(target=UploadAvatar, args=(avatarlist4,key))
        t4.start()
        t5 = threading.Thread(target=UploadAvatar, args=(avatarlist5,key))
        t5.start()
    except:
        EventLog("Error occured whilst uploading avatar:\n" + traceback.format_exc())