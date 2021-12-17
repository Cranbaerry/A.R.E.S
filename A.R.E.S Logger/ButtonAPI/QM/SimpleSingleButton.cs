using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class SimpleSingleButton
    {
        public readonly TextMeshProUGUI buttonText;
        private readonly Button buttonButton;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;

        public SimpleSingleButton(Transform parent, string text, Action click, string tooltip)
        {
            gameObject = Object.Instantiate(APIStuff.GetSingleButtonTemplate(), parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonText.fontSize = 28f;
            buttonText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -25f, 0f);
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = tooltip;
            Object.Destroy(gameObject.transform.Find("Icon").gameObject);
            Object.Destroy(gameObject.transform.Find("Icon_Secondary").gameObject);
            buttonText.color = new Color(0.9f, 0.9f, 0.9f);
            buttonText.richText = true;
        }
        
        public SimpleSingleButton(MenuPage pge, string text, Action click, string tooltip) : this(pge.menuContents, text, click, tooltip)
        {
        }
        
        public SimpleSingleButton(ButtonGroup grp, string text, Action click, string tooltip) : this(grp.gameObject.transform, text, click, tooltip)
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