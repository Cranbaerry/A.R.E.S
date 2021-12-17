using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class TextButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly TextMeshProUGUI buttonTextBig;
        private readonly Button buttonButton;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;
        
        public TextButton(Transform parent, string text, Action click, string tooltip, string bigText)
        {
            gameObject = Object.Instantiate(APIStuff.GetSingleButtonTemplate(), parent);
            buttonText = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).First();
            buttonText.text = text;
            buttonTextBig = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).Last();
            buttonTextBig.text = bigText;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = tooltip;
        }
        
        public TextButton(MenuPage pge, string text, Action click, string tooltip, string bigText) : this(pge.menuContents, text, click, tooltip, bigText)
        {
        }
        
        public TextButton(ButtonGroup grp, string text, Action click, string tooltip, string bigText) : this(grp.gameObject.transform, text, click, tooltip, bigText)
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
        
        public void SetBigText(string newText)
        {
            buttonTextBig.text = newText;
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