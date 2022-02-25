# A.R.E.S

ARES is an avatar recovery tool! It is only for educational uses! We do not condone theft of avatars, the tool soley exists to recover avatars from within VRChat back onto new accounts and into their unity packages keeping as much of the avatar in-tact as possible!

Thx to Yui, they have chosen not to be marked as a developer on the project when they might as well be, they taught me everything I know about photon and it is an important part of the project as a whole. If you see him around thank him for me! The same thanks go out to Blaze for showing me a better/more stable way to create buttons ingame!

Thx to [Blaze](https://github.com/WTFBlaze) for helping me fix a few bugs within the wear avatar by ID button added in V10!

Thx to Afton (He doesn't have a GitHub ;-;) for helping me create the unlimited avatar favorites!

Thx to [nesrak1](https://github.com/nesrak1/AssetsTools.NET) for the original "AssetTools.NET.dll" file that was edited by me and then packed into our HOTSWAP allowing for VRCA files to be compressed and decompressed!

Thx to [Requi](https://github.com/RequiDev) for [ReMod.Core](https://github.com/RequiDev/ReMod.Core), we use this as our button API within ARES!

Thx to [Kirai](https://github.com/xKiraiChan) for the code behind our [force clone](https://github.com/Astrum-Project/AstralClone) patch!

Thx to [FACS01](https://github.com/FACS01-01/FACS_Utilities) for the bledshape and animation repairs only compatable with VRCA files extracted with ARES!

And special thanks to [cassell1337](https://github.com/cassell1337) for development assisted development of the GUI and total development of the API!

If you have any feedback for us or would like to suggest a feature please use our [FeedbackForm](https://forms.gle/QifnS6ZSa8fse9yF7).

[Permanent Invite Link To Support Discord Server](https://discord.gg/dhSdMsfgWe)

Virus Totals:

[ARESPlugin.dll](https://www.virustotal.com/gui/file/dfe89e62fcc820adf64e96c8707991fc99320374e4cda4e192eaa99837bfaa26/summary)

[AvatarLogger.dll](https://www.virustotal.com/gui/file/4cf54e694e100c02c54e3f20ca8465b62a5f26695365d15eb5dcbe041c30bba0/summary)

[UnRar.exe](https://www.virustotal.com/gui/file/f706f001e14f2c505de572ef095cd0cdcb8701bd9f2068a7048e4edb6f81b2d0/summary)

[ARES.Updater.exe](https://www.virustotal.com/gui/file/fc26bad74bb215928132bb2bc56e03df135628d57941e959b3a2e82bd026d1c3/summary)

[V11.0.1 GUI RAR](https://www.virustotal.com/gui/file/ad82c95614d052e8ed1eced3d556dddc256a40188b091b45ed4690a7d74f4769/summary)

[DRAG.INTO.VRChat.FOLDER.RAR](https://www.virustotal.com/gui/file/10e60fcb04a0e0c75652cb54c356a923b183fcf47950a585743b70a4624c12ee/summary)

Features:

    -Unlimted avatar favorites (Can be disabled)

    -Stealth mode to ensure all in-game buttons are hidden!

    -Copy instance IDs and join by them by clipboard

    -Repair avatar VRCAs retreived from the cache

	-Logs ALL avatars seen in game regardless of them being private or public(Including Quest Versions)
	
	-An easy to navigate and in my opinion extremely sexy GUI
	
	-Ability to search avatar logs by:
	
		-Avatar Name
		
		-Avatar ID
		
		-Avatar Author
		
		-Uploader Tags

            -Author ID
	
	-Browse the logs with image previews of avatars
	
	-Mostly-Automatic HotswapingV2 + Set images on upload
	
	-HTML Viewer for easy browsing of larger logs

    -Search by release platforms: search for avatars that have PC or Quest compatability!
	
	-Search by tags: These are the tags given during the upload of the avatar
	
	-Version detection: IF your copy is out of dste your application will now propmt you to upgrade

    -API support! If you are willing to share your logs with us in return you will gain access to a database containing avatars logged by other users

    -Ability to hotswap from a vaid VRCA file downloaded elsewhere

    -Ability to almost perfectly extract vrca files for repairs with FACS

Installation:
	
Step One: [DownloadMe](https://github.com/Dean2k/A.R.E.S/releases/latest/download/DRAG.CONTENTS.INTO.VRChat.FOLDER.rar).

Step Two: [Copy the files into your VRChat folder](https://i.imgur.com/izsyjz8.gif).

Step Three: Run VRChat and allow the plugin to fully install the mod, you can now configure your settings by clicking [here](https://i.imgur.com/iXi8VXv.png) then [here](https://i.imgur.com/3y0XZeJ.png) brining you to the [settings screen](https://i.imgur.com/nyV5Sse.png).

Step Four: [Run the "ARES.Updater.exe"](https://i.imgur.com/XfHDP2Z.gif), this will install the GUI and launch into it for a first time setup where you must select your Unity 2019.4.31f1 exe, follow the steps [here](https://rentry.org/LargestGithubSupportUnityInst) to ensure you have the correct unity installation!

Step Five: After pressing okay [here](https://i.imgur.com/LgHbHJQ.png) find your exe and [select it](https://i.imgur.com/BydfbV8.png)!

Step Six: Press "OK" on [this](https://i.imgur.com/gqKcHNn.png) screen, this will begin creating your HSB, if you don't know what you are doing press [original](https://i.imgur.com/NpcDrMu.png) here to use the regular VRCSDK! If you know what you are doing and want to use a custom SDK just select custom and choose your SDK of choice!

Info: From here onwards you can open ARES via the [ARES.exe](https://i.imgur.com/F3NDgCb.png) in your [GUI Folder](https://i.imgur.com/ovleFKV.png)!

ARES is now setup, working and ready to use, however there are some extra steps to gain API access to see a databse of avatars logged by all ARES users!

API Step One: Head to the [ARES GUI "Setings" tab](https://i.imgur.com/kLLdPzq.png).

~~API Step Two: Head to our [Discord Server](https://discord.gg/dhSdMsfgWe), verify and [get a key](https://i.imgur.com/YtzzQOf.png) by typing !key in the #keys channel.~~

~~API Step Three: Paste the [key](https://i.imgur.com/WsEMH0z.png) in your [key tab](https://i.imgur.com/qQEJKk2.png) and press ["Set Key"](https://i.imgur.com/DmSBggW.png).~~

API Step Two ~~Four~~: You may now press the [Toggle API](https://i.imgur.com/xAnJGrG.png) button to activate the API, this will quit ARES, just open it again!

All done! Can now access [API Features](https://i.imgur.com/kklkouA.png)

Issues? Open an issue in the "Issues" tab, We will do our best to resolve your issue!

License:

For other software used within the project please refer to the LICENSE folder in our release binaries and GitHub repos.

[GNU GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html)
