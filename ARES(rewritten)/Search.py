import requests
import re
from LogUtils import *

def check_quary_avatar_name(term, avatars):
    new = []
    for avatar in avatars:
        print("if term: "+term.lower()+" in avatar[2]: "+avatar[2].lower())
        if term.lower() in avatar[2].lower():
            new.append(avatar)
            print("new: "+str(new)[:10])
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
        if avatar[11] == "Private":
            new.append(avatar)
    return new

def check_public(avatars):
    new = []
    for avatar in avatars:
        if avatar[11] == "Public":
            new.append(avatar)
    return new

def check_pc_asset(avatars):
    new = []
    for avatar in avatars:
        if avatar[6] != None:
            new.append(avatar)
    return new

def check_quest_assets(avatars):
    new = []
    for avatar in avatars:
        if avatar[7] == None:
            new.append(avatar)
    return new

def check_tags(tags, avatars):
    new = []
    for avatar in avatars:
        for tag in tags:
            if tag.lower() in avatar[12].lower():
                new.append(avatar)
    return new


def filter(query, filters={}, avatars=[]):
    new_list = avatars
    if query != "":
        if filters["Avatar name"]:
            new_list = check_quary_avatar_name(query, new_list)
        if filters["Avatar author"]:
            new_list = check_quary_author_name(query, new_list)
        if filters["Avatar id"]:
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
        new_list = new_list
    elif filters["PCasseturl"] == True and filters["Questasseturl"] == False:
        new_list = check_pc_asset(new_list)
    elif filters["PCasseturl"] == False and filters["Questasseturl"] == True:
        new_list = check_quest_assets(new_list)
    if filters["NSFW"] == True or filters["Violonce"] == True or filters["Gore"] == True or filters["Othernsfw"] == True:
        tgs = []
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
    pass

def search(query, filters={}, api=False, Localavatars=None):
    # print("Searching for: " + query)
    # print("Filters: " + str(filters))
    # print("API: " + str(api))
    # print("Localavatars: " + str(Localavatars))
    if not api:
        avis = filter(query, filters, Localavatars)
        print("avis: "+str(len(avis)))
        return avis