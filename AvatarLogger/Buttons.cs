//Importing reqired modules
using System;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using ReMod.Core.UI.QuickMenu;
using MelonLoader;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using VRC.Core;
using VRC.UI;
using ComfyUtils;
using static BaseFuncs.BaseFuncs;
using static AvatarLogger.Main;
//Contains all code responsible for creating buttons/in-game Ui elements
namespace Buttons
{
    internal static class Buttons
    {
        public static Sprite ButtonImage = LoadSpriteFromDisk((Environment.CurrentDirectory + "\\ARESLogo.png"));
        private static ConfigHelper<AvatarLogger.Config> Helper => AvatarLogger.Main.Helper;

        //Creation of buttons within the Ui
        public static void OnUiManagerInit()
        {
            MelonLogger.Msg("Ui initiating...");
            ReMenuPage TabPage = new ReMenuPage("ARES", true);
            ReTabButton ARESTAB = ReTabButton.Create("ARES", "Access ARES Menus", "ARES", ButtonImage);
            ReMenuPage LSMP = TabPage.AddMenuPage("Logging Settings", "Allows you to configure your ARES settings!");
            LSMP.AddToggle("Log Avatars", "Toggles the logging of avatars", delegate (bool b) { Config.LogAvatars = b; Helper.SaveConfig(); }, Config.LogAvatars);
            LSMP.AddToggle("Log Own Avatars", "Toggles the logging of own avatars", delegate (bool b) { Config.LogOwnAvatars = b; Helper.SaveConfig(); }, Config.LogOwnAvatars);
            LSMP.AddToggle("Log Friends Avatars", "Toggles the ability to log avatars uploaded to your friends accounts!", delegate (bool b) { Config.LogFriendsAvatars = b; Helper.SaveConfig(); }, Config.LogFriendsAvatars);
            LSMP.AddToggle("Log To Console", "Toggles the ability display logged avatars in console!", delegate (bool b) { Config.LogToConsole = b; Helper.SaveConfig(); }, Config.LogToConsole);
            LSMP.AddToggle("Log Errors To Console", "Toggles the ability display why avaatrs weren't logged in console!", delegate (bool b) { Config.ConsoleError = b; Helper.SaveConfig(); }, Config.ConsoleError);
            ReMenuPage FPage = TabPage.AddMenuPage("ARES Functions", "Use the other features within ARES");
            FPage.AddButton("Copy Instance ID", "Copies the current instance ID to your clipboard!", delegate { Clipboard.SetText(WorldInstanceID); });
            FPage.AddButton("Join Instance By ID", "Joins the instance currently within your clipboard!", delegate { JoinInstanceByID(); });
            FPage.AddButton("Wear Avatar ID", "Changes into avatar ID that is currently in clipboard!", delegate { ChangeAvatar(); });
            FPage.AddButton("Restart VRC (Persistent)", "Restarts VRChat and re-joins the room you were in!", delegate { RVRC(true); });
            FPage.AddButton("Restart VRC", "Restarts VRChat!", delegate { RVRC(false); });
            FPage.AddButton("Show Logging Statistics", "Displays session statistics within the console", delegate { ShowSessionStats(); });
            MelonLogger.Msg("Ui ready!");
        }

        //Restarts VRChat
        public static void RVRC(bool persistence)
        {
            new Thread(() =>
            {
                //Kills VRChat
                UnityEngine.Application.Quit();
                Thread.Sleep(2500);
                try
                {
                    //Opens VRChat
                    string cl = Environment.CommandLine;
                    if (cl.Contains("vrchat://launch"))
                    {
                        string launch = cl.Substring(cl.IndexOf("vrchat://launch"));
                        cl = cl.Remove(cl.IndexOf("vrchat://launch"), launch.Contains(" ") ? launch.IndexOf(" ") : launch.Length);
                    }
                    if (persistence) { cl = $"{cl} vrchat://launch?id={RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}"; }
                    Process.Start($"{Environment.CurrentDirectory}\\VRChat.exe", cl);
                }
                catch (Exception) { new Exception(); }
                Process.GetCurrentProcess().Kill();
            })
            {
                IsBackground = true,
                Name = "RestartVRC Thread"
            }.Start();
        }
        //Shows the current logging session statistics
        private static void ShowSessionStats()
        {
            MelonLogger.Msg("-------------------------------------");
            MelonLogger.Msg($"Current Logging Session Stats:");
            MelonLogger.Msg("-------------------------------------");
            MelonLogger.Msg($"Total Logged Avatars: {Pub + Pri}");
            MelonLogger.Msg($"Logged PC Avatars: {PC}");
            MelonLogger.Msg($"Logged Quest Avatars: {Q}");
            MelonLogger.Msg($"Logged Private Avatars: {Pri}");
            MelonLogger.Msg($"Logged Public Avatars: {Pub}");
            MelonLogger.Msg("-------------------------------------");
        }
        //Takes the insatnce ID attached to the clipboard and joins the world!
        private static void JoinInstanceByID()
        {
            string[] ID = Clipboard.GetText().Split(':');
            if (Clipboard.GetText().Contains("wrld"))
            {
                JoinInstance(ID[0], ID[1]);
                MelonLogger.Msg($"Instance joined: {Clipboard.GetText()}");
            }
            else
            {
                MelonLogger.Msg($"Invalid instance ID!");
            }
        }
        //Changes into the avatar based on an avatar ID within the clipboard
        public static void ChangeAvatar()
        {
            Regex Avatar = new Regex("avtr_[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}");
            if (Avatar.IsMatch(Clipboard.GetText()))
            {
                try
                {
                    PageAvatar Changer = GameObject.Find("UserInterface/MenuContent/Screens/Avatar").GetComponent<PageAvatar>();
                    Changer.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar { id = Avatar.Matches(Clipboard.GetText())[0].Value };
                    Changer.ChangeToSelectedAvatar();
                    MelonLogger.Msg($"Avatar switched: {Avatar.Matches(Clipboard.GetText())[0].Value}");
                }
                catch
                {
                    MelonLogger.Msg($"Invalid Avatar ID!");
                }
            }
            else
            {
                MelonLogger.Msg($"Invalid Avatar ID!");
            }
        }
    }
}
