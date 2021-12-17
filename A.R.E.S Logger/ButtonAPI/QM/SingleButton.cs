using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class SingleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Button buttonButton;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;
        
        public SingleButton(Transform parent, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false)
        {
            gameObject = Object.Instantiate(APIStuff.GetSingleButtonTemplate(), parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = tooltip;
            buttonImage = gameObject.transform.Find("Icon").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
                buttonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    buttonImage.color = Color.white;
                    buttonImage.GetComponent<StyleElement>().enabled = false;
                }
            }
            else
            {
                buttonImage.gameObject.SetActive(false);
            }
        }
        
        public SingleButton(MenuPage pge, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(pge.menuContents, text, click, tooltip, icon, preserveColor)
        {
        }
        
        public SingleButton(ButtonGroup grp, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(grp.gameObject.transform, text, click, tooltip, icon, preserveColor)
        {
        }
        
        public void SetAction(Action newAction)
        {
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(newAction);
        }
        
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        
        public void SetTooltip(string newTooltip)
        {
            buttonTooltip.field_Public_String_0 = newTooltip;
        }
        
        public void SetIcon(Sprite newIcon, bool preserveColor = false)
        {
            if (newIcon == null)
            {
                buttonImage.gameObject.SetActive(false);
                return;
            }
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;
            buttonImage.gameObject.SetActive(true);
            if (preserveColor)
            {
                buttonImage.color = Color.white;
            }
        }
        
        public void SetIconColor(Color color)
        {
            buttonImage.color = color;
        }
        
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
        
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}