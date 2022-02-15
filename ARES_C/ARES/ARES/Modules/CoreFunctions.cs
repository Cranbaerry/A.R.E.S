using ARES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARES.Modules
{
    public class CoreFunctions
    {
        public string SetAvatarInfo(Records avatar)
        {
            string avatarString = string.Format("Time Dectected: {0} {13} Avatar ID: {1} {13} Avatar Name: {2} {13} Avatar Description {3} {13} Author ID: {4} {13} Author Name: {5} {13} PC Asset URL: {6} {13} Quest Asset URL: {7} {13} Image URL: {8} {13} Thumbnail URL: {9} {13} Unity Version: {10} {13} Release Status: {11} {13} Tags: {12}", avatar.TimeDetected, avatar.AvatarID, avatar.AvatarName, avatar.AvatarDescription, avatar.AuthorID, avatar.AuthorName, avatar.PCAssetURL, avatar.QuestAssetURL, avatar.ImageURL, avatar.ThumbnailURL, avatar.UnityVersion, avatar.ReleaseStatus, avatar.Tags, Environment.NewLine);
            return avatarString;
        }
    }
}
