# CachedAvatarLocator
Allows for easy searching of the VRChat cache folder for a particular avatar!
Installation:

	1. Install Python from here: https://www.python.org/downloads/ (Latest)
	
	2. Download the "Avatar Scanner.py" an place it in your VRChat cache folder: (C:\Users\WINDOWSUSERNAME\AppData\LocalLow\VRChat\VRChat\Cache-WindowsPlayer)
	
	3. Create a shortcut to your VRChat cache folder on your desktop or anywhere convenient!
	
	4. Install complete!
	
Usage:

	1. Open the shortcut to your cache folder
	
	2. Shift+RightClick and open a powershell/cmd window in the directory
	
	3. Enter the following command: python 'Avatar Scanner.py'
	
	4. When propted for the search term enter it This can be a part of the name or even the avatar ID itself!
	
	5. You will get the data objets directory, itll look somthing like this: "B1489EB51AA62D85\00000000000000000000000002000000\__data" That data file is your vrca so just rename it to AVATAR.vrca and boom you can continue with a hotswap or begin tearing down the vrca with utinyripper or asset bundle extractor!
		
	WARNING: Odd avatar names may bug out the program and they will not be scanned correctly, you can only enter single strings, this means you cannot use spaces! If the avatar contains spaces in its name the try entering key words like the first word in the name in an attempt to find the data object file!
	 
	Issues? Open an issue in the "Issues" tab and I will do my best to resolve your issue!
How does it work?

It scans all files in cache for a string relvant to the avatar you are looking for then prints its findings including its directory and the string where the name appears!