import requests, re
from LogUtils import *

def check_quary_avatar_name(term, avatars):
    new = []
    for avatar in avatars:
        if term.lower() in avatar[2].lower():
            new.append(avatar)
    return new

def check_quary_author_name(term, avatars):
    new = []
    for avatar in avatars:
        if term.lower() in avatar[5].lower():
            new.append(avatar)
    return new

def check_quary_AvatarID_name(term, avatars):
    new = []
    for avatar in avatars:
        if term.lower() in avatar[1].lower():
            new.append(avatar)
    return new

def check_quary_AuthorID_name(term, avatars):
    new = []
    for avatar in avatars:
        if term.lower() in avatar[4].lower():
            new.append(avatar)
    return new

def check_private(avatars):
    new = []
    for avatar in avatars:
        if str(avatar[11]).lower() == "private":
            new.append(avatar)
    return new

def check_public(avatars):
    new = []
    for avatar in avatars:
        if str(avatar[11]).lower() == "public":
            new.append(avatar)
    return new

def check_pc_asset(avatars):
    new = []
    for avatar in avatars:
        if avatar[6] != "None":
            new.append(avatar)
    return new

def check_quest_assets(avatars):
    new = []
    for avatar in avatars:
        if avatar[7] != "None":
            new.append(avatar)
    return new

def check_both_assets(avatars):
    new = []
    for avatar in avatars:
        if avatar[7] != "None" and avatar[6] != "None":
            new.append(avatar)
    return new

def check_tags(tags, avatars):
    new = []
    for avatar in avatars:
        for tag in tags:
            if tag.lower() in avatar[12].lower():
                if avatar not in new:
                    new.append(avatar)
    return new


def filter(query, filters={}, avatars=[]):
    new_list = avatars

    if query != "":
        if filters["Avatar name"]:
            new_list = check_quary_avatar_name(query, new_list)
        if filters["Avatar author"]:
            usr = re.match("usr_........-....-....-....-............",str(query).replace(" ",""))
            if usr:
                query = str(query).replace(" ","")
                new_list = check_quary_AuthorID_name(query, new_list)
            else:
                new_list = check_quary_author_name(query, new_list)
        if filters["Avatar id"]:
            query = str(query).replace(" ","")
            new_list = check_quary_AvatarID_name(query, new_list)
    # checking if the filter is private or public
    if filters["private"] == True and filters["public"] == True:
        new_list = new_list
    elif filters["private"] == True and filters["public"] == False:
        new_list = check_private(new_list)
    elif filters["private"] == False and filters["public"] == True:
        new_list = check_public(new_list)
    #checking for pc assets and quest assets
    if filters["PCasseturl"] == True and filters["Questasseturl"] == True:
        new_list = check_both_assets(new_list)
    elif filters["PCasseturl"] == True and filters["Questasseturl"] == False:
        new_list = check_pc_asset(new_list)
    elif filters["PCasseturl"] == False and filters["Questasseturl"] == True:
        new_list = check_quest_assets(new_list)
    if filters["NSFW"] or filters["Violonce"] or filters["Gore"] or filters["Othernsfw"]:
        tgs = []
        if filters["NSFW"] and filters["Violonce"] and filters["Gore"] and filters["Othernsfw"]:
            tgs.append("None")
        if filters["NSFW"] == True:
            tgs.append("content_sex")
        if filters["Violonce"] == True:
            tgs.append("content_violence")
        if filters["Gore"] == True:
            tgs.append("content_gore")
        if filters["Othernsfw"] == True:
            tgs.append("content_other")
        new_list = check_tags(tgs, new_list)
    return new_list

def get_avatars_list_api(query, filters={}):
    headers = {
        'Connection': 'keep-alive',
        'Pragma': 'no-cache',
        'Cache-Control': 'no-cache',
        'accept': 'application/json',
        'User-Agent': filters["key"],
        'Content-Type': 'application/json',
        'Sec-GPC': '1',
        'Origin': 'http://127.0.0.1:8000',
        'Sec-Fetch-Site': 'same-origin',
        'Sec-Fetch-Mode': 'cors',
        'Sec-Fetch-Dest': 'empty',
        'Referer': 'http://127.0.0.1:8000/docs',
        'Accept-Language': 'en-US,en;q=0.9',
    }

    data = {"author": filters["Avatar author"], "avatarid": filters["Avatar id"], "name": filters["Avatar name"], "searchterm": query}

    response = requests.post('http://api.avataruploader.tk/search', headers=headers, json=data)
    IDList = []
    cleanarr = []
    for x in response.json():
        if x[1] not in IDList:
            IDList.append(x[1])
            cleanarr.append(x)
    return cleanarr

def search(query, filters={}, api=False, Localavatars=None):
    if not api:
        avis = filter(query, filters, Localavatars)
        return avis
    if api:
        if filters["Avatar id"]:
            query = str(query).replace(" ","")
        if filters["Avatar author"]:
            usr = re.match("usr_........-....-....-....-............",str(query).replace(" ",""))
            if usr:
                query = str(query).replace(" ","")
        avis = filter(query, filters, get_avatars_list_api(query, filters))
        return avis
