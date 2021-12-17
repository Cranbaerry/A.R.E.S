using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ARES.Utils.API.SM
{
    public class SMButton
    {
        public GameObject button;
        public Text text;

        public SMButton(SMButtonType type, string location, float PosX, float PosY, string buttonText, Action buttonAction)
        {
            Initialize(type, APIStuff.GetSocialMenuInstance().transform.Find(location), PosX, PosY, buttonText, buttonAction, 1f, 1f);
        }

        public SMButton(SMButtonType type, Transform location, float PosX, float PosY, string buttonText, Action buttonAction)
        {
            Initialize(type, location, PosX, PosY, buttonText, buttonAction, 1f, 1f);
        }

        public SMButton(SMButtonType type, string location, float PosX, float PosY, string buttonText, Action buttonAction, float XSize, float YSize)
        {
            Initialize(type, APIStuff.GetSocialMenuInstance().transform.Find(location), PosX, PosY, buttonText, buttonAction, XSize, YSize);
        }

        public SMButton(SMButtonType type, Transform location, float PosX, float PosY, string buttonText, Action buttonAction, float XSize, float YSize)
        {
            Initialize(type, location, PosX, PosY, buttonText, buttonAction, XSize, YSize);
        }

        private void Initialize(SMButtonType type, Transform location, float PosX, float PosY, string buttonText, Action buttonAction, float XSize, float YSize)
        {
            switch (type)
            {
                case SMButtonType.ChangeAvatar:
                    button = UnityEngine.Object.Instantiate(APIStuff.GetSocialMenuInstance().transform.Find("Avatar/Change Button").gameObject, location);
                    text = button.transform.Find("Label").GetComponent<Text>();
                    break;

                case SMButtonType.DropPortal:
                    button = UnityEngine.Object.Instantiate(APIStuff.GetSocialMenuInstance().transform.Find("WorldInfo/WorldButtons/PortalButton").gameObject, location);
                    text = button.transform.Find("Text").GetComponent<Text>();
                    break;

                case SMButtonType.EditStatus:
                    button = UnityEngine.Object.Instantiate(APIStuff.GetSocialMenuInstance().transform.Find("Social/UserProfileAndStatusSection/Status/EditStatusButton").gameObject, location);
                    text = button.transform.Find("Text").GetComponent<Text>();
                    text.transform.SetAsLastSibling();
                    break;

                case SMButtonType.ExitVRChat:
                    button = UnityEngine.Object.Instantiate(APIStuff.GetSocialMenuInstance().transform.Find("Settings/Footer/Exit").gameObject, location);
                    text = button.transform.Find("Text").GetComponent<Text>();
                    break;

                default:
                    button = UnityEngine.Object.Instantiate(APIStuff.GetSocialMenuInstance().transform.Find("Social/UserProfileAndStatusSection/Status/EditStatusButton").gameObject, location);
                    text = button.transform.Find("Text").GetComponent<Text>();
                    break;
            }
            button.name = $"SMButton-{APIStuff.RandomNumbers()}";
            var origColors = button.GetComponent<Button>().colors;
            button.GetComponent<Button>().colors = new ColorBlock()
            {
                colorMultiplier = origColors.colorMultiplier,
                disabledColor = origColors.disabledColor,
                fadeDuration = origColors.fadeDuration,
                highlightedColor = origColors.highlightedColor,
                normalColor = origColors.normalColor,
                selectedColor = origColors.normalColor,
                pressedColor = origColors.pressedColor
            };
            button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            button.GetComponent<Button>().onClick.AddListener(buttonAction);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.GetComponent<RectTransform>().sizeDelta /= new Vector2(XSize, YSize);
            SetText(buttonText);
            SetLocation(new Vector2(PosX, PosY));
            text.color = Color.white;
        }

        public void SetInteractable(bool state)
        {
            button.GetComponent<Button>().interactable = state;
        }

        public void SetText(string message)
        {
            text.supportRichText = true;
            text.text = message;
        }

        public void SetLocation(Vector2 location)
        {
            button.GetComponent<RectTransform>().anchoredPosition = location;
        }

        public void SetColor(Color color)
        {
            button.GetComponentInChildren<Image>().color = color;
        }

        public void SetActive(bool state)
        {
            button.SetActive(state);
        }

        public Text GetText()
        {
            return text;
        }

        public GameObject GetGameObject()
        {
            return button;
        }

        public void SetImage(Sprite image)
        {
            button.GetComponentInChildren<Image>().sprite = image;
        }

        public void SetImageColor(Color color)
        {
            var orig = button.GetComponent<Button>().colors;
            var cb = new ColorBlock()
            {
                colorMultiplier = orig.colorMultiplier,
                disabledColor = orig.disabledColor,
                fadeDuration = orig.fadeDuration,
                highlightedColor = color,
                normalColor = color,
                pressedColor = orig.pressedColor
            };
            button.GetComponent<Button>().colors = cb;
        }

        public async void SetImage(string URL)
        {
            await GetRemoteTexture(button.GetComponentInChildren<Image>(), URL);
        }

        private async Task GetRemoteTexture(Image Instance, string url)
        {
            var www = UnityWebRequestTexture.GetTexture(url);
            var asyncOp = www.SendWebRequest();
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            if (www.isNetworkError || www.isHttpError)
            {
                return;
            }
            Sprite Sprite;
            Sprite = Sprite.CreateSprite(DownloadHandlerTexture.GetContent(www), new Rect(0, 0, DownloadHandlerTexture.GetContent(www).width, DownloadHandlerTexture.GetContent(www).height), Vector2.zero, 100 * 1000, 1000, SpriteMeshType.FullRect, Vector4.zero, false);
            Instance.sprite = Sprite;
            Instance.color = Color.white;
            DownloadHandlerTexture.GetContent(www);
        }

        public void SetShader(string shaderName)
        {
            var Material = new Material(button.GetComponentInChildren<Image>().material);
            Material.shader = Shader.Find(shaderName);
            button.gameObject.GetComponentInChildren<Image>().material = Material;
        }

        public Texture2D getTexture()
        {
            Texture2D texture2D = new Texture2D(button.gameObject.GetComponent<Image>().mainTexture.width, button.gameObject.GetComponent<Image>().mainTexture.height, TextureFormat.RGBA32, false);
            return texture2D;
        }

        public void ToggleTexture(bool state)
        {
            button.GetComponentInChildren<Image>().enabled = state;
        }

        public enum SMButtonType
        {
            EditStatus,
            ChangeAvatar,
            ExitVRChat,
            DropPortal,
            Header
        }
    }
}