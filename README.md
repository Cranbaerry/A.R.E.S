# Avatar Logger GUI

Thx to [KeafyIsHere](https://github.com/KeafyIsHere) for the original avatar logging script!

And special thanks to [cassell1337](https://github.com/cassell1337) for willingly being mindfucked with me while making the semi-auto hotswap and the new GUI!

If you have any feedback for us or would like to suggest a feature please use our [FeedbackForm](https://forms.gle/QifnS6ZSa8fse9yF7).

I have gone about validating the latest V6.8 package with Microsoft, if a false positive is detected please follow the insructions under GUI>Dist>Submission Details. <<This also works as proof that all files are safe as they were manually reviewed by a Microsoft employee!

Allows for easy searching of the VRChat cache folder for a particular avatar that has been previously encountered via the use of a simple GUI

Features:

	-Logs ALL avatars seen in game regardless of them being private or public
	
	-An easy to navigate and in my opinion extremely sexy GUI
	
	-Ability to search avatar logs by:
	
		-Avatar Name
		
		-Avatar ID
		
		-Avatar Author
		
		-Uploader Tags
		
	-Ability to delete logs and parses from within the GUI
	
	-Browse the logs with image previews of avatars
	
	-Mostly-Automatic Hotswaping
	
	-HTML Viewer for easy browsing of larger logs
	
	-Search by tags: These are the tags given during the upload of the avatar
	
	-Version detection: IF your copy is out of dste your application will now propmt you to upgrade

    -API support! If you are willing to share your logs with us in return you will gain access to a database containing avatars logged by other users

    -Ability to hotswap from a VRCA file downloaded elsewhere

Installation/Usage: Videos can be found in the "Tutorials" folder!
	
    1. Install melon loader from here: (https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.exe)

    2. Get the latest package from the releases secetion (Currently V6.8)

    3. Install the mod in the "Mods" folder

    4. Place the "GUI" folder wherever it is easiest to access

    5. (Optional) Only install if you want to hotswap, you need the latest supported VRChat unity version (https://download.unity3d.com/download_unity/008688490035/Windows64EditorInstaller/UnitySetup64-2018.4.20f1.exe)

    6. Launch the game and log some avatars before attempting to use the GUI

Common issues and fixes:

    -The GUI may incorrectly locate your unity/vrchat installations, to fix this simply open the "Settings.json" file in any text editor and manually change the direcories. The options should look like this if done correctly:
        {
        "Avatar_Folder": "F:\\SteamLibrary\\steamapps\\common\\VRChat\\AvatarLog",
        "Unity_Exe": "C:\\Program Files\\Unity",
        }
        The "Unity_Exe" should not be aimed directly at the exe but instead aimed at the folder containing the "Editor" folder!
Extras:

    -If you enable API in the settings you will have all logged avatars sent to our database, in exchange you will gain access to an entire collection of avatars logged by others!

Issues? Open an issue in the "Issues" tab, We will do our best to resolve your issue!

Licesnse:

[GNU GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html)
