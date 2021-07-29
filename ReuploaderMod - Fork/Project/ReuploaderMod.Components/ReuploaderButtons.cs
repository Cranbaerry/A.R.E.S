using FastUI;
using FastUI.QuickMenu;
using FastUI.UserInterface;
using MelonLoader;
using ReuploaderMod.Components;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.UI;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppSystem.Collections.Generic;
using UnhollowerBaseLib.Attributes;
using static ExtensionMethods;

namespace ReuploaderMod.Components
{
    public class ReuploaderButtons : MonoBehaviour
    {


        public string string_8;


        public Delegate ReferencedDelegate;

        public IntPtr MethodInfo;

        public Il2CppSystem.Collections.Generic.List<MonoBehaviour> AntiGcList;

        private QuickMenuPaginatedButton quickMenuPaginatedButton_0;

        private QuickMenuToggleButton quickMenuToggleButton_0;

        private QuickMenuToggleButton quickMenuToggleButton_1;

        private QuickMenuButton quickMenuButton_0;
        private UserInterfaceAvatarButton userInterfaceAvatarButton_0;

        private UserInterfaceAvatarButton userInterfaceAvatarButton_1;

        private UserInterfaceAvatarButton userInterfaceAvatarButton_2;

        private UserInterfaceAvatarButton userInterfaceAvatarButton_3;

        private UserInterfaceUserInfoButton userInterfaceUserInfoButton_0;

        private UserInterfaceWorldInfoButton userInterfaceWorldInfoButton_0;

        public static bool isPrivate = true;

        public volatile bool ActionsBool;

        public System.Collections.Generic.List<Action> Actions = new System.Collections.Generic.List<Action>(10);

        public System.Collections.Generic.List<Action> Actions2 = new System.Collections.Generic.List<Action>(10);

        private string NewAvatarID = string.Empty;

        private Player player_0;

        private VRCPlayer vrcplayer_0;

        private APIUser apiuser_0;

        private VRCAvatarManager vrcavatarManager_0;

        private ApiAvatar SelectedAvatar;

        private ApiAvatar apiAvatar_1;

        private ApiWorld ReuploadedWorld;

        private string NewWorldID = string.Empty;

        private ApiWorld SelectedWorld;

        private VRC.Core.ApiFile WorldAssetBundle;

        private VRC.Core.ApiFile AvatarAssetBundle;

        private static string AssetBundlePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "AssetBundles");

        private static string VrcaStorePath = Path.Combine(AssetBundlePath, "VrcaStore");

        private static string UBPUPProgram = "UBPU.exe";

        private static string UBUPPath = Path.Combine(AssetBundlePath, UBPUPProgram);

        private static string ReuploaderModDataPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "ReuploaderModData");

        private static string NameTxtFile = "name.txt";

        private static string NameTxtPath = Path.Combine(ReuploaderModDataPath, NameTxtFile);

        private Assembly assembly_0;

        private Type GetUnpackerType;

        private MethodInfo GetUnpackerMethod;

        public ReuploaderButtons(IntPtr intptr_1)
            : base(intptr_1)
        {
            AntiGcList = new Il2CppSystem.Collections.Generic.List<MonoBehaviour>(1);
            AntiGcList.Add(this);
        }

        public ReuploaderButtons(Delegate delegate_1, IntPtr intptr_1)
            : base(ClassInjector.DerivedConstructorPointer<ReuploaderButtons>())
        {
            ClassInjector.DerivedConstructorBody(this);
            ReferencedDelegate = delegate_1;
            MethodInfo = intptr_1;
        }

        ~ReuploaderButtons()
        {
            Marshal.FreeHGlobal(MethodInfo);
            MethodInfo = IntPtr.Zero;
            ReferencedDelegate = null;
            AntiGcList.Remove(this);
            AntiGcList = null;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr intptr_1);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string string_21, string string_22);

        private void FocusConsole()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

        private void ForceConsolefocus()
        {
            IntPtr intPtr = FindWindow(null, "VRChat");
            if (intPtr != IntPtr.Zero)
            {
                SetForegroundWindow(intPtr);
            }
        }
        

        //TODO  : TRACK ERROR
        public void Start()
      {
	MelonLogger.LogWarning("=======================================================");
	MelonLogger.LogWarning("Patched Reuploader by xAstroBoy#6969");
	MelonLogger.LogWarning("=======================================================");
    MelonLogger.LogWarning("=======================================================");
    MelonLogger.LogWarning("Fixed & Maintained by LoudestBoi");
    MelonLogger.LogWarning("=======================================================");
    MelonLogger.LogWarning("You can now customize buttons!");
	MelonLogger.LogWarning("=======================================================");
	try
	{
		//FindUserDetailsBtn();
		quickMenuPaginatedButton_0 = new QuickMenuPaginatedButton
		{
			Name = "ReuploaderModSettingsButton",
			ButtonText = "Reupl.\nMod",
			ParentTransform = ParentStore.ShortcutMenu,
			UiTooltipText = "Reuploader Mod Settings",
			Smart = true,
			PageType = PageType.BackOnly,
			IsActive = true,
			IsInteractable = true,
			Indented = true
		}.Create();
		quickMenuButton_0 = new QuickMenuButton
		{
			Name = "UserInteractMenuReuploadButton",
			ButtonText = "Reupload\nAvatar",
			Smart = true,
			UiTooltipText = "Reupload Avatar",
			ParentTransform = ParentStore.UserInteractMenu,
			OnClick = (Action)ReuploadSelectedAvatar,
			IsActive = true,
			IsInteractable = true
		}.Create();
		quickMenuToggleButton_0 = new QuickMenuToggleButton
		{
			Name = "ToggleReleaseStatusButton",
			ToggledOnText = "Public",
			ToggledOffText = "Private",
			UiTooltipText = "Toggle release status of future uploaded avatars",
			ParentTransform = quickMenuPaginatedButton_0.PageTransform,
			ToggledOn = (Action)SetPublic,
			ToggledOff = (Action)SetPrivate,
			IsInteractable = true,
			IsActive = true
		}.Create();

                quickMenuPaginatedButton_0.AutoPosition(quickMenuToggleButton_0);
		userInterfaceUserInfoButton_0 = new UserInterfaceUserInfoButton
		{
			Name = "UserInfoReuploadButton",
			ButtonText = "Reupload Avi",
			OnClick = (Action)GetApiAvatar,
			Position = Utils.GetUserInfoButtonTemplate().transform.localPosition + new Vector3(0f, 150f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();
                userInterfaceUserInfoButton_0.Transform.SetAsLastSibling();
		userInterfaceAvatarButton_0 = new UserInterfaceAvatarButton
		{
			Name = "AvatarNameChangeButton",
			ButtonText = "Name",
			OnClick = (Action)ChangeAvatarName,
			Position = Utils.GetAvatarButtonTemplate().transform.localPosition + new Vector3(-99.75f, 682f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();
                userInterfaceAvatarButton_0.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, userInterfaceAvatarButton_0.RectTransform.rect.width / 2f);
		userInterfaceAvatarButton_1 = new UserInterfaceAvatarButton
		{
			Name = "AvatarImageChangeButton",
			ButtonText = "Image",
			OnClick = (Action)ChangeAvatarImage,
			Position = Utils.GetAvatarButtonTemplate().transform.localPosition + new Vector3(69.75f, 682f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();

        userInterfaceAvatarButton_1.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, userInterfaceAvatarButton_1.RectTransform.rect.width / 2f);
		userInterfaceAvatarButton_3 = new UserInterfaceAvatarButton
		{
			Name = "AvatarDescChangeButton",
			ButtonText = "Description",
			OnClick = (Action)ChangeAvatarDescription,
			Position = Utils.GetAvatarButtonTemplate().transform.localPosition + new Vector3(239.25f, 682f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();
                userInterfaceAvatarButton_3.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, userInterfaceAvatarButton_3.RectTransform.rect.width / 2f);
		userInterfaceAvatarButton_2 = new UserInterfaceAvatarButton
		{
			Name = "AvatarDeleteButton",
			ButtonText = "Delete",
			OnClick = (Action)DeleteAvatar,
			Position = Utils.GetAvatarButtonTemplate().transform.localPosition + new Vector3(408.75f, 682f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();
                userInterfaceAvatarButton_2.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, userInterfaceAvatarButton_2.RectTransform.rect.width / 2f);
		userInterfaceWorldInfoButton_0 = new UserInterfaceWorldInfoButton
		{
			Name = "WorldInfoReuploadButton",
			ButtonText = "Reupload World",
			OnClick = (Action)ReuploadWorldAction,
			Position = Utils.GetWorldInfoButtonTemplate().transform.localPosition + new Vector3(270f, 0f, 0f),
			IsInteractable = true,
			IsActive = true
		}.Create();
                quickMenuToggleButton_1 = new QuickMenuToggleButton
		{
			Name = "ToggleUIButtons",
			ToggledOnText = "UI Off",
			ToggledOffText = "UI On",
			UiTooltipText = "Toggle buttons on the UI",
			ParentTransform = quickMenuPaginatedButton_0.PageTransform,
			ToggledOn = (Action)DisableButtons,
			ToggledOff = (Action)EnableButtons,
			IsInteractable = true,
			IsActive = true
		}.Create();
                quickMenuPaginatedButton_0.AutoPosition(quickMenuToggleButton_1);
	}
	catch (Exception ex)
	{
		MelonLogger.LogError(ex.ToString());
		MelonLogger.LogError("=======================================================");
		MelonLogger.LogError("An error occured while creating buttons for RMod. The mod may not work correctly.");
		MelonLogger.LogError("Any other button errors may be related to the client you are running but compatibility is improving soon!");
		MelonLogger.LogError("=======================================================");
	}

            //TODO : FIND HOW TO USE THIS CODE TO AVOID CONSOLE SPAM


            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ReuploaderMod.Resources.UBPU.exe"))
                {
                    using MemoryStream memoryStream = new MemoryStream((int)stream.Length);
                    stream.CopyTo(memoryStream);
                    assembly_0 = Assembly.Load(memoryStream.ToArray());
                }
                GetUnpackerType = assembly_0.GetTypes().First((Type type_0) => type_0.Name.Equals("Program"));
                GetUnpackerMethod = GetUnpackerType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).First((MethodInfo methodInfo_0) => methodInfo_0.Name.Equals("Main"));
            }
            catch (Exception ex2)
            {
                MelonLogger.LogError(ex2.ToString());
            }
            if (Directory.Exists(AssetBundlePath))
	        {
		Directory.EnumerateDirectories(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				if (string_0.EndsWith("_dump"))
				{
					Directory.Delete(string_0, recursive: true);
				}
			}
			catch (Exception)
			{
			}
		});
		Directory.EnumerateFiles(AssetBundlePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				File.Delete(string_0);
			}
			catch (Exception)
			{
			}
		});
		Directory.EnumerateFiles(VrcaStorePath).ToList().ForEach(delegate(string string_0)
		{
			try
			{
				File.Delete(string_0);
			}
			catch (Exception)
			{
			}
		});
	}
	else
	{
		Directory.CreateDirectory(AssetBundlePath);
		Directory.CreateDirectory(VrcaStorePath);
	}
}

        public void Debug(string text)
        {
            if(true)
            {
                MelonLogger.LogWarning(text);
            }
        }


        public void Update()
        {
            if (ActionsBool)
            {
                lock (Actions)
                {
                    System.Collections.Generic.List<Action> list = Actions2;
                    Actions2 = Actions;
                    Actions = list;
                    ActionsBool = false;
                }
                foreach (Action item in Actions2)
                {
                    item();
                    Actions.Remove(item);
                }
                Actions2.Clear();
            }
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    ReuploadSelectedAvatar();
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    FocusConsole();
                    MelonLogger.Log("Input avatarId:");
                    string string_ = Console.ReadLine();
                    ForceConsolefocus();
                    ReuploadAvatar(string_);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    ReuploadWorld(RoomManager.field_Internal_Static_ApiWorld_0.id);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    FocusConsole();
                    MelonLogger.Log("Input worldId:");
                    string string_2 = Console.ReadLine();
                    ForceConsolefocus();
                    ReuploadWorld(string_2);
                }
            }
        }

        public void RegisterAction(Action action_0)
        {
            lock (Actions)
            {
                Actions.Add(action_0);
                ActionsBool = true;
            }
        }

        private void SetPrivate()
        {
            isPrivate = true;
        }

        private void SetPublic()
        {
            isPrivate = false;
        }

        private void EnableButtons()
        {
            userInterfaceUserInfoButton_0.IsActive = true;
            userInterfaceAvatarButton_0.IsActive = true;
            userInterfaceAvatarButton_1.IsActive = true;
            userInterfaceAvatarButton_3.IsActive = true;
            userInterfaceAvatarButton_2.IsActive = true;
            userInterfaceWorldInfoButton_0.IsActive = true;
        }

        private void DisableButtons()
        {
            userInterfaceUserInfoButton_0.IsActive = false;
            userInterfaceAvatarButton_0.IsActive = false;
            userInterfaceAvatarButton_1.IsActive = false;
            userInterfaceAvatarButton_3.IsActive = false;
            userInterfaceAvatarButton_2.IsActive = false;
            userInterfaceWorldInfoButton_0.IsActive = false;
        }

        private void ReuploadWorldAction()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/WorldInfo").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageWorldInfo component = gameObject.GetComponent<PageWorldInfo>();
            if ((bool)component)
            {
                ApiWorld field_Private_ApiWorld_ = component.field_Private_ApiWorld_0;
                if (field_Private_ApiWorld_ != null)
                {
                    ReuploadWorld(field_Private_ApiWorld_.id);
                }
                else
                {
                    MelonLogger.LogError("Couldn't fetch APIWorld");
                }
            }
        }

        private void GetApiAvatar()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/UserInfo").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageUserInfo component = gameObject.GetComponent<PageUserInfo>();
            if (!component)
            {
                return;
            }
            APIUser apiuser_0 = component.user;
            if (apiuser_0 == null)
            {
                return;
            }
            player_0 = PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0.ToList().FirstOrDefault((Player player_0) => (player_0.prop_APIUser_0 != null && player_0.prop_APIUser_0.id.Equals(apiuser_0.id)) ? true : false);
            if (!player_0)
            {
                MelonLogger.LogError("Couldn't fetch Player");
                return;
            }
            this.apiuser_0 = player_0.prop_APIUser_0;
            if (this.apiuser_0 != null)
            {
                vrcplayer_0 = player_0.prop_VRCPlayer_0;
                if (!vrcplayer_0)
                {
                    MelonLogger.LogError("Couldn't fetch VRCPlayer");
                    return;
                }
                vrcavatarManager_0 = vrcplayer_0.prop_VRCAvatarManager_0;
                if (!vrcavatarManager_0)
                {
                    MelonLogger.LogError("Couldn't fetch AvatarManager");
                    return;
                }
                SelectedAvatar = vrcavatarManager_0.prop_ApiAvatar_0;
                if (SelectedAvatar != null)
                {
                    ReuploadAvatar(SelectedAvatar.id);
                }
                else
                {
                    MelonLogger.LogError("Couldn't fetch ApiAvatar");
                }
            }
            else
            {
                MelonLogger.LogError("Couldn't fetch APIUser");
            }
        }

        private void ReuploadSelectedAvatar()
        {
            player_0 = QuickMenu.prop_QuickMenu_0.field_Private_Player_0;
            if (!player_0)
            {
                player_0 = Player.prop_Player_0;
                if (!player_0)
                {
                    MelonLogger.LogError("Couldn't fetch player");
                    return;
                }
            }
            apiuser_0 = player_0.prop_APIUser_0;
            if (apiuser_0 == null)
            {
                MelonLogger.LogError("Couldn't fetch APIUser");
                return;
            }
            vrcplayer_0 = player_0.prop_VRCPlayer_0;
            if (!vrcplayer_0)
            {
                MelonLogger.LogError("Couldn't fetch VRCPlayer");
                return;
            }
            vrcavatarManager_0 = vrcplayer_0.prop_VRCAvatarManager_0;
            if (!vrcavatarManager_0)
            {
                MelonLogger.LogError("Couldn't fetch AvatarManager");
                return;
            }
            SelectedAvatar = vrcavatarManager_0.prop_ApiAvatar_0;
            if (SelectedAvatar != null)
            {
                ReuploadAvatar(SelectedAvatar.id);
            }
            else
            {
                MelonLogger.LogError("Couldn't fetch ApiAvatar");
            }
        }

        private void ChangeAvatarName()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.avatar;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ != null && !(field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id))
            {
                MelonLogger.Log("Enter new avatar name:");
                FocusConsole();
                string name = Console.ReadLine();
                ForceConsolefocus();
                field_Internal_ApiAvatar_.name = name;
                field_Internal_ApiAvatar_.Save((Action<ApiContainer>)delegate
                {
                    MelonLogger.Log("Successfully changed avatar name");
                }, (Action<ApiContainer>)delegate
                {
                    MelonLogger.LogError("Couldn't change avatar name");
                });
            }
        }

        private void ChangeAvatarImage()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.avatar;
            if (!avatar)
            {
                return;
            }
            ApiAvatar apiAvatar_0 = avatar.field_Internal_ApiAvatar_0;
            if (apiAvatar_0 == null || apiAvatar_0.authorId != APIUser.CurrentUser.id)
            {
                return;
            }
            CursorLockMode lockState = Cursor.lockState;
            bool visible = Cursor.visible;
            MelonLogger.Log("Select new image:");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            string text = OpenFileWindows.OpenfileDialog();
            if (!string.IsNullOrEmpty(text))
            {
                Cursor.lockState = lockState;
                Cursor.visible = visible;
                MelonLogger.Log(text);
                string name = apiAvatar_0.name;
                string text2 = apiAvatar_0.unityVersion.ToLower();
                string text3 = apiAvatar_0.platform.ToLower();
                string text4 = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                if (string.IsNullOrEmpty(text4))
                {
                    text4 = "4";
                }
                string text5 = "Avatar - " + name + " - Image - " + text2 + "_" + text4 + "_" + text3 + "_Release";
                string value = VRC.Core.ApiFile.ParseFileIdFromFileAPIUrl(apiAvatar_0.imageUrl);
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                ApiFileUtils.UploadFileAsync(text, value, text5, delegate (VRC.Core.ApiFile apiFile_0, string string_0)
                {
                    apiAvatar_0.imageUrl = apiFile_0.GetFileURL();
                    apiAvatar_0.Save((Action<ApiContainer>)delegate
                    {
                        MelonLogger.Log("Successfully changed avatar image");
                    }, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("Couldn't change avatar image (POST)");
                    });
                }, delegate (VRC.Core.ApiFile apiFile_0, string string_0)
                {
                    MelonLogger.LogError("Couldn't change avatar image (UPLOAD), " + string_0);
                }, delegate
                {
                }, (VRC.Core.ApiFile apiFile_0) => false);
            }
            else
            {
                MelonLogger.LogError("Couldn't open filedialog or path was null");
            }
        }

        private void ChangeAvatarDescription()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.avatar;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ != null && !(field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id))
            {
                MelonLogger.Log("Enter new avatar description:");
                FocusConsole();
                string description = Console.ReadLine();
                ForceConsolefocus();
                field_Internal_ApiAvatar_.description = description;
                field_Internal_ApiAvatar_.Save((Action<ApiContainer>)delegate
                {
                    MelonLogger.Log("Successfully changed avatar description");
                }, (Action<ApiContainer>)delegate
                {
                    MelonLogger.LogError("Couldn't change avatar description");
                });
            }
        }

        private void DeleteAvatar()
        {
            VRCUiManager vRCUiManager = VRCUiManager.prop_VRCUiManager_0;
            if (!vRCUiManager)
            {
                return;
            }
            GameObject gameObject = vRCUiManager.menuContent.transform.Find("Screens/Avatar").gameObject;
            if (!gameObject)
            {
                return;
            }
            PageAvatar component = gameObject.GetComponent<PageAvatar>();
            if (!component)
            {
                return;
            }
            SimpleAvatarPedestal avatar = component.avatar;
            if (!avatar)
            {
                return;
            }
            ApiAvatar field_Internal_ApiAvatar_ = avatar.field_Internal_ApiAvatar_0;
            if (field_Internal_ApiAvatar_ == null || field_Internal_ApiAvatar_.authorId != APIUser.CurrentUser.id)
            {
                return;
            }
            field_Internal_ApiAvatar_.Delete((Action<ApiContainer>)delegate
            {
                MelonLogger.Log("Successfully deleted avatar");
            }, (Action<ApiContainer>)delegate
            {
                while (true)
                {
                    int num = -1146411568;
                    while (true)
                    {
                        int num2 = num;
                        uint num3;
                        switch ((num3 = (uint)(~(-(~num2) - 465534149))) % 3u)
                        {
                            case 1u:
                                goto IL_0003;
                            default:
                                return;

                            case 0u:
                                break;

                            case 2u:
                                return;
                        }
                        break;
                    IL_0003:
                        MelonLogger.LogError("Couldn't delete avatar");
                        num = (int)((num3 * 2064907121) ^ 0x4695662A);
                    }
                }
            });
        }

        private void ReuploadAvatar(string avatarID)
        {
            if (string.IsNullOrEmpty(avatarID))
            {
                MelonLogger.LogError("AvatarId was empty");
                return;
            }
            API.Fetch<ApiAvatar>(avatarID, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                SelectedAvatar = apiContainer_0.Model.Cast<ApiAvatar>();
                if (SelectedAvatar == null)
                {
                    MelonLogger.LogError("Couldn't fetch ApiAvatar");
                }
                else
                {
                    NewAvatarID = GenerateAvatarID();
                    API.Fetch<ApiAvatar>(NewAvatarID, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("AvatarId " + NewAvatarID + " already in use!");
                        ReuploadAvatar(avatarID);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                            MelonLogger.Log("AvatarId: " + SelectedAvatar.id + " | AssetUrl: " + SelectedAvatar.assetUrl + " | Author: " + SelectedAvatar.authorName);
                            try
                            {
                                string DownloadPath = await DownloadAvatar(SelectedAvatar);
                                if (!string.IsNullOrEmpty(DownloadPath))
                                {
                                    MelonLogger.Log("DownloadAvatarSuccess");
                                    string UncompressedVRCA = await UncompressBundle(DownloadPath);
                                    MelonLogger.Log("AssetBundle created");
                                    Debug("UncompressedVRCA Path is set to :" + UncompressedVRCA);
                                    if (!string.IsNullOrEmpty(UncompressedVRCA))
                                    {
                                        string unityVersion = SelectedAvatar.unityVersion.ToLower();
                                        string platform = SelectedAvatar.platform.ToLower();
                                        string ApiVersion = ApiWorld.VERSION.ApiVersion.ToString().ToLower();
                                        if (string.IsNullOrEmpty(ApiVersion))
                                        {
                                            ApiVersion = "4";
                                        }
                                       var avatarimage = "Avatar - " + SelectedAvatar.name + " - Image - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        var AvatarAssetBundle = "Avatar - " + SelectedAvatar.name + " - Asset bundle - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        MelonLogger.Log("AvatarNames generated!");
                                        MelonLogger.Log(avatarimage);
                                        MelonLogger.Log(AvatarAssetBundle);
                                        if (!(await ReplaceID(UncompressedVRCA, NewAvatarID, SelectedAvatar.id)))
                                        {
                                            MelonLogger.LogError("Failed to set AvatarId!");
                                        }
                                        string PackedBundle = await CompressAssetBundle(UncompressedVRCA);
                                        if (!string.IsNullOrEmpty(PackedBundle))
                                        {
                                            RegisterAction(delegate
                                            {
                                                MelonLogger.Log("Uploading vrca");
                                                ApiFileUtils.UploadFileAsync(PackedBundle, null, AvatarAssetBundle, this.OnUploadVrcaAsyncSuccess, this.OnUploadVrcaAsyncFailure, delegate (VRC.Core.ApiFile imageBundle,  string string_0, string string_1, float UploadingStatus)
                                                {
                                                    Debug("VRCA Uploading Progress : " + UploadingStatus);
                                                }, (VRC.Core.ApiFile File) => false);
                                            });
                                        }
                                        else
                                        {
                                            MelonLogger.LogError("Failed to recompress AssetBundle!");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MelonLogger.LogError(ex.ToString());
                            }
                        });
                    });
                }
            }, (Action<ApiContainer>)delegate
          {
              MelonLogger.LogError("Couldn't fetch avatar (API)");
          });
        }

        private void OnUploadVrcaAsyncSuccess(VRC.Core.ApiFile avatar, string string_21)
        {
            
            MelonLogger.Log("OnUploadVrcaAsyncSuccess");
            AvatarAssetBundle = avatar;
            Debug("AvatarAssetBundle : " + avatar.GetFileURL());
            Task.Run(async delegate
            {
                var image = await DownloadImage(SelectedAvatar.imageUrl);
                if (!string.IsNullOrEmpty(image))
                {
                    MelonLogger.Log("DownloadAvatarImageSuccess");
                    RegisterAction(delegate
                    {
                        MelonLogger.Log("Uploading image");
                        ApiFileUtils.UploadFileAsync(image, null, avatar.GetFileURL(), this.OnUploadVrcaAsynSuccess, this.OnUploadImageAsyncFailure, delegate (VRC.Core.ApiFile apiFile_0, string string_0, string string_1, float Progress)
                        {
                            Debug("Uploading Avatar Image Progress :" + Progress);
                        }, (VRC.Core.ApiFile Assets) => false);
                    });
                }
            });
        }

        private void OnUploadVrcaAsyncFailure(VRC.Core.ApiFile ImageUrl, string status)
        {
            MelonLogger.Log("OnUploadVrcaAsyncFailure");
        }

        private void OnUploadVrcaAsynSuccess(VRC.Core.ApiFile ImageUrl, string status)
        {

            MelonLogger.Log("OnUploadVrcaAsynSuccess");
            RegisterAction(delegate
            {
                apiAvatar_1 = new ApiAvatar
                {
                    id = NewAvatarID,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = SelectedAvatar.name,
                    imageUrl = ImageUrl.GetFileURL(),
                    assetUrl = AvatarAssetBundle.GetFileURL(),
                    description = SelectedAvatar.description,
                    releaseStatus = (isPrivate ? "private" : "public")
                };
                apiAvatar_1.Post((Action<ApiContainer>)OnApiAvatarPostSuccess, (Action<ApiContainer>)OnApiAvatarPostFailure);
            });
        }

        private void OnUploadImageAsyncFailure(VRC.Core.ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadImageAsyncFailure");
        }

        private void OnApiAvatarPostSuccess(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiAvatarPostSuccess");
            ClearOldSession();
            MelonLogger.Log("Check Your Avatar, Upload Completed!");

        }

        private void OnApiAvatarPostFailure(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiAvatarPostFailure");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }


        // Just used the Avatar Reuploading Code to reupload worlds too...
        // Stupid Hector
        private void ReuploadWorld(string SelectedWorldID)
        {
            if (string.IsNullOrEmpty(SelectedWorldID))
            {
                MelonLogger.LogError("WorldID was empty");
                return;
            }
            API.Fetch<ApiWorld>(SelectedWorldID, (Action<ApiContainer>)delegate (ApiContainer apiContainer_0)
            {
                SelectedWorld = apiContainer_0.Model.Cast<ApiWorld>();
                if (SelectedWorld == null)
                {
                    MelonLogger.LogError("Couldn't fetch ApiWorld");
                }
                else
                {
                    NewWorldID = GenerateWorldID();
                    API.Fetch<ApiAvatar>(NewWorldID, (Action<ApiContainer>)delegate
                    {
                        MelonLogger.LogError("WorldID " + NewWorldID + " already in use!");
                        ReuploadWorld(SelectedWorldID);
                    }, (Action<ApiContainer>)delegate
                    {
                        Task.Run(async delegate
                        {
                           MelonLogger.Log("WorldId: " + SelectedWorld.id + " | AssetUrl: " + SelectedWorld.assetUrl + " | Author: " + SelectedWorld.authorName);
                            try
                            {
                                string DownloadPath = await DownloadWorld(SelectedWorld);
                                if (!string.IsNullOrEmpty(DownloadPath))
                                {
                                    MelonLogger.Log("DownloadWorldSuccess");
                                    string UncompressedVRCW = await UncompressBundle(DownloadPath);
                                    MelonLogger.Log("AssetBundle created");
                                    Debug("UncompressedVRCW Path is set to :" + UncompressedVRCW);
                                    if (!string.IsNullOrEmpty(UncompressedVRCW))
                                    {
                                        string unityVersion = SelectedWorld.unityVersion.ToLower();
                                        string platform = SelectedWorld.platform.ToLower();
                                        string ApiVersion = SelectedWorld.apiVersion.ToString().ToLower();
                                        if (string.IsNullOrEmpty(ApiVersion))
                                        {
                                            ApiVersion = "4";
                                        }
                                        var WorldImage = "World - " + SelectedWorld.name + " - Image - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";
                                        var WorldAsset = "World - " + SelectedWorld.name + " - Asset bundle - " + unityVersion + "_" + ApiVersion + "_" + platform + "_Release";                                        MelonLogger.Log("AvatarNames generated!");
                                        MelonLogger.Log(WorldImage);
                                        MelonLogger.Log(WorldAsset);
                                        if (!(await ReplaceID(UncompressedVRCW, NewWorldID, SelectedWorld.id)))
                                        {
                                            MelonLogger.LogError("Failed to set WorldID!");
                                        }
                                        string PackedBundle = await CompressAssetBundle(UncompressedVRCW);
                                        if (!string.IsNullOrEmpty(PackedBundle))
                                        {
                                            RegisterAction(delegate
                                            {
                                                MelonLogger.Log("Uploading vrcw");
                                                ApiFileUtils.UploadFileAsync(PackedBundle, null, WorldAsset, this.OnUploadVrcwAsyncSuccess, this.OnUploadVrcwAsyncFailure, delegate (VRC.Core.ApiFile imageBundle,  string string_0, string string_1, float UploadingStatus)
                                                {
                                                    Debug("World Uploading Progress : " + UploadingStatus);
                                                }, (VRC.Core.ApiFile File) => false);
                                            });
                                        }
                                        else
                                        {
                                            MelonLogger.LogError("Failed to recompress AssetBundle!");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MelonLogger.LogError(ex.ToString());
                            }
                        });
                    });
                }
            }, (Action<ApiContainer>)delegate
          {
              MelonLogger.LogError("Couldn't fetch World (API)");
          });
        }

        private void OnUploadVrcwAsyncSuccess(VRC.Core.ApiFile World, string AssetBundle)
        {
            MelonLogger.Log("OnUploadVrcwAsyncSuccess");
            WorldAssetBundle = World;
            Debug("WorldAssetBundle : " + World.GetFileURL());
            Task.Run(async delegate
            {
                var Image = await DownloadImage(SelectedWorld.imageUrl);
                if (!string.IsNullOrEmpty(Image))
                {
                    MelonLogger.Log("OnUploadVrcwAsyncSuccess");
                    RegisterAction(delegate
                    {
                        MelonLogger.Log("Uploading image");
                        ApiFileUtils.UploadFileAsync(Image, null, AssetBundle, this.OnUploadVrcwAsynSuccess, this.OnUploadImageAsyncFailure, delegate (VRC.Core.ApiFile world, string ImageUrl, string AssetUrl, float progress)
                        {
                            Debug("World Image Uploading :" + progress);
                        }, (VRC.Core.ApiFile worlddone) => false);
                    });
                }
            });
        }

        private void OnUploadVrcwAsyncFailure(VRC.Core.ApiFile apiFile_2, string string_21)
        {
            MelonLogger.Log("OnUploadVrcwAsyncFailure");
        }

        private void OnUploadVrcwAsynSuccess(VRC.Core.ApiFile WorldImage, string notused)
        {
            MelonLogger.Log("OnUploadVrcwAsynSuccess");
            RegisterAction(delegate
            {
                ReuploadedWorld = new ApiWorld
                {
                    id = NewWorldID,
                    authorName = APIUser.CurrentUser.username,
                    authorId = APIUser.CurrentUser.id,
                    name = SelectedWorld.name,
                    imageUrl = WorldImage.GetFileURL(), //TODO : CHECK TO BE SURE.
                    assetUrl = WorldAssetBundle.GetFileURL(),
                    description = SelectedWorld.description,
                    releaseStatus = (isPrivate ? "private" : "public")
                };
                ReuploadedWorld.Post((Action<ApiContainer>)OnApiWorldPostSuccess, (Action<ApiContainer>)OnApiWorldPostFailure);
            });
        }



        private void OnApiWorldPostSuccess(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiWorldPostSuccess");
            ClearOldSession();
            MelonLogger.Log("Check Your Worlds, Upload Completed!");            
        }

        private void OnApiWorldPostFailure(ApiContainer apiContainer_0)
        {
            MelonLogger.Log("OnApiWorldPostFailure");
            if (!ClearOldSession())
            {
                MelonLogger.LogWarning("Error while cleaning up the AssetBundles directory, you can probably ignore this.");
            }
        }

        private bool ClearOldSession()
        {
            foreach (string item in Directory.EnumerateFiles(AssetBundlePath))
            {
                if (!item.EndsWith("UBPU.exe"))
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            foreach (string item2 in Directory.EnumerateDirectories(AssetBundlePath))
            {
                if (!item2.EndsWith("VrcaStore"))
                {
                    try
                    {
                        Directory.Delete(item2, recursive: true);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (Directory.EnumerateFiles(AssetBundlePath).Count() == 1 && Directory.EnumerateDirectories(AssetBundlePath).Count() == 1)
            {
                return true;
            }
            return false;
        }

        private string GenerateAvatarID()
        {
            return "avtr_" + Guid.NewGuid().ToString();
        }

        private string GenerateWorldID()
        {
            return "wrld_" + Guid.NewGuid().ToString();
        }



        private int Dunnowtfisdis(byte[] byte_0, byte[] byte_1)
        {
            int num = 0;
            while (true)
            {
                if (num < byte_0.Length - byte_1.Length)
                {
                    bool flag = true;
                    for (int i = 0; i < byte_1.Length; i++)
                    {
                        if (byte_0[num + i] != byte_1[i])
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                    num++;
                    continue;
                }
                return -1;
            }
            return num;
        }

        private async Task<bool> ReplaceID(string filepath, string NewID, string OldID)
        {
            await Task.Delay(100);
            try
            {
                byte[] array = File.ReadAllBytes(filepath);
                byte[] bytes = Encoding.ASCII.GetBytes(OldID);
                Encoding.ASCII.GetBytes(OldID.ToLower());
                byte[] bytes2 = Encoding.ASCII.GetBytes(NewID);
                if (!OldID.Contains("avtr_") && !OldID.Contains("wrld_"))
                {
                    MelonLogger.LogError("Custom avatar ids aren't supported");
                    return false;
                }
                byte[] array2 = new byte[array.Length + bytes2.Length - bytes.Length];
                byte[] array3 = array;
                int num;
                while ((num = Dunnowtfisdis(array3, bytes)) >= 0)
                {
                    Buffer.BlockCopy(array3, 0, array2, 0, num);
                    Buffer.BlockCopy(bytes2, 0, array2, num, bytes2.Length);
                    Buffer.BlockCopy(array3, num + bytes.Length, array2, num + bytes2.Length, array3.Length - num - bytes.Length);
                    array3 = array2;
                }
                File.WriteAllBytes(filepath, array2);
                MelonLogger.Log("AssetBundle overwritten!");
                return true;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.ToString());
                return false;
            }
        }

        private async Task<string> DownloadAvatar(ApiAvatar apiAvatar_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiAvatar_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrca");
            File.WriteAllBytes(text, bytes);
            MelonLogger.Log("DownloadAvatar");
            return text;
        }

        private async Task<string> DownloadWorld(ApiWorld apiWorld_2)
        {
            byte[] bytes = await new HttpClient().GetByteArrayAsync(apiWorld_2.assetUrl);
            string text = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".vrcw");
            File.WriteAllBytes(text, bytes);
            MelonLogger.Log("DownloadWorld");
            return text;
        }

        private async Task<string> DownloadImage(string string_21)
        {
            HttpResponseMessage httpResponseMessage = await new HttpClient().GetAsync(string_21);
            byte[] array = await httpResponseMessage.Content.ReadAsByteArrayAsync();
            if (array == null || array.Length == 0)
            {
                MelonLogger.Log("image was null or 0");
            }
            string text = httpResponseMessage.Content.Headers.GetValues("Content-Type").First().Split('/')[1];
            MelonLogger.Log(text);
            string text2 = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + "." + text);
            MelonLogger.Log(text2);
            File.WriteAllBytes(text2, array);
            MelonLogger.Log("DownloadImage");
            return text2;
        }

        private string GetDumpedVRCA(string path)
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from string_0 in Directory.EnumerateFiles(path + "_dump")
                                                                        select new FileInfo(string_0) into fileInfo_0
                                                                        orderby fileInfo_0.CreationTime
                                                                        select fileInfo_0.FullName;
            if (enumerable.Count() != 1)
            {
                foreach (string item in enumerable)
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(item)))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return enumerable.ElementAt(0);
        }

        public void RunUBPU(string[] string_21)
        {
            try
            {
                GetUnpackerMethod?.Invoke(null, new object[1]
                {
            string_21
                });
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.ToString());
            }
        }

        private async Task<string> UncompressBundle(string downloadedpath)
        {

            string AssetBundlePath = string.Empty;
            Debug("GenerateAssetBundle Called with params " + downloadedpath);

            if (File.Exists(downloadedpath))
            {
                string path = Path.Combine(ReuploaderButtons.AssetBundlePath, Path.GetFileName(downloadedpath));
                Debug(path);
                File.Move(downloadedpath, path);
                Debug("Moved " + downloadedpath + " to " + path);
                if (File.Exists(path))
                {
                    File.Delete(downloadedpath);
                    Debug("Deleted " + downloadedpath);
                    RunUBPU(new string[1]
                    {
                    path
                    });
                    MelonLogger.Log("Decompressing assetbundle..");
                    
                    await Task.Delay(1000);
                    MelonLogger.Log("Finished decompressing!");
                    AssetBundlePath = GetDumpedVRCA(path);
                    File.Delete(path);
                    Debug("Deleted " + path);
                    return AssetBundlePath;
                }
                else
                {
                    Debug("File Not Found.");
                    return AssetBundlePath;

                }
            }
            else
            {
                Debug("File Not Found.");
                return AssetBundlePath;
            }
        }


        private async Task<string> CompressAssetBundle(string UncompressedBundlePath)
        {
            try
            {
                string text = GetXMLFile();
                if (string.IsNullOrEmpty(text))
                {
                    MelonLogger.LogWarning("XML File Empty!");
                    return string.Empty;
                }
                MelonLogger.Log("Compressing assetbundle..");
                string directoryName = Path.GetDirectoryName(Application.dataPath);
                Directory.SetCurrentDirectory(AssetBundlePath);
                RunUBPU(new string[2]
                {
                    text,
                    "lz4hc"
                });
                await Task.Delay(1000);
                Directory.SetCurrentDirectory(directoryName);
                MelonLogger.Log("Finished compressing!");
                var Compressed = GetLZ4HCFile();
                if (!string.IsNullOrEmpty(Compressed))
                {
                    int startIndex = Compressed.IndexOf(".LZ4HC");
                    string fileName = Path.GetFileName(Compressed.Remove(startIndex, 6));
                    Debug("Filename :" + fileName);
                    string destFileName = Path.Combine(VrcaStorePath, fileName);
                    Debug("destFileName :" + fileName);
                    File.Move(Compressed, destFileName);
                    return destFileName;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                MelonLogger.LogError(ex.Message);
                return string.Empty;
            }
        }

        private string GetXMLFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from Path in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(Path) into File
                                                                        orderby File.CreationTime
                                                                        select File.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".xml"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }

        private string GetLZ4HCFile()
        {
            string result = string.Empty;
            System.Collections.Generic.IEnumerable<string> enumerable = from Path in Directory.EnumerateFiles(AssetBundlePath)
                                                                        select new FileInfo(Path) into File
                                                                        orderby File.CreationTime
                                                                        select File.FullName;
            if (enumerable.Any())
            {
                foreach (string item in enumerable)
                {
                    if (item.EndsWith(".LZ4HC"))
                    {
                        result = item;
                    }
                }
                return result;
            }
            return result;
        }
    }
}