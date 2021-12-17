using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class ToggleButton
    {
        /*private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Toggle buttonToggle;
        private readonly UiToggleTooltip toggleTooltip;
        public readonly GameObject gameObject;*/

        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Button buttonButton;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;
        private bool currentState;
        private readonly Action OnAction;
        private readonly Action OffAction;
        
        public ToggleButton(Transform parent, string btnText, Action onAction, Action offAction, string toolTipText, bool defaultState = false)
        {
            gameObject = Object.Instantiate(APIStuff.GetSingleButtonTemplate(), parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = btnText;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(new Action(HandleClick));
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = toolTipText;
            buttonImage = gameObject.transform.Find("Icon").GetComponentInChildren<Image>(true);
            OnAction = onAction;
            OffAction = offAction;
            currentState = defaultState;
            var tmpIcon = currentState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            buttonImage.sprite = tmpIcon;
            buttonImage.overrideSprite = tmpIcon;
        }

        public ToggleButton(MenuPage pge, string btnText, Action onAction, Action offAction, string toolTipText, bool defaultState = false) : this(pge.menuContents, btnText, onAction, offAction, toolTipText, defaultState)
        {
        }
        
        public ToggleButton(ButtonGroup grp, string btnText, Action onAction, Action offAction, string toolTipText, bool defaultState = false) : this(grp.gameObject.transform, btnText, onAction, offAction, toolTipText, defaultState)
        {
        }
        
        private void HandleClick()
        {
            currentState = !currentState;
            var stateIcon = currentState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            buttonImage.sprite = stateIcon;
            buttonImage.overrideSprite = stateIcon;
            if (currentState)
            {
                OnAction.Invoke();
            }
            else
            {
                OffAction.Invoke();
            }
        }

        public void SetToggleState(bool newState, bool shouldInvoke = false)
        {
            var newIcon = newState ? APIStuff.GetOnIconSprite() : APIStuff.GetOffIconSprite();
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;

            if (shouldInvoke)
            {
                if (newState)
                {
                    OnAction.Invoke();
                }
                else
                {
                    OffAction.Invoke();
                }
            }
        }

        public void ClickMe()
        {
            HandleClick();
        }
        
        public bool GetCurrentState()
        {
            return currentState;
        }
        
        /*public ToggleButton(Transform parent, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null)
        {
            gameObject = Object.Instantiate(APIStuff.GetToggleButtonTemplate(), parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonToggle = gameObject.GetComponentInChildren<Toggle>(true);
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = false;
            buttonToggle.onValueChanged.AddListener(new Action<bool>(stateChanged));
            toggleTooltip = gameObject.GetComponentInChildren<UiToggleTooltip>(true);
            toggleTooltip.field_Public_String_0 = onTooltip;
            toggleTooltip.field_Public_String_1 = offTooltip;
            toggleTooltip.Method_Public_Void_Boolean_0(true);
            buttonImage = gameObject.transform.Find("Icon_On").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
                return;
            }
            buttonImage.sprite = APIStuff.GetOnIconSprite();
            buttonImage.overrideSprite = APIStuff.GetOnIconSprite();
        }
        
        public ToggleButton(MenuPage pge, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null) : this(pge.menuContents, text, stateChanged, offTooltip, onTooltip, icon)
        {
        }
        
        public ToggleButton(ButtonGroup grp, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null) : this(grp.gameObject.transform, text, stateChanged, offTooltip, onTooltip, icon)
        {
        }
        
        public void SetAction(Action<bool> newAction)
        {
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.onValueChanged.AddListener(newAction);
        }
        
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        
        public void SetTooltip(string newOffTooltip, string newOnTooltip)
        {
            toggleTooltip.field_Public_String_0 = newOnTooltip;
            toggleTooltip.field_Public_String_1 = newOffTooltip;
        }
        
        public void SetIcon(Sprite newIcon)
        {
            if (newIcon == null)
            {
                buttonImage.gameObject.SetActive(false);
                return;
            }
            buttonImage.sprite = newIcon;
            buttonImage.overrideSprite = newIcon;
            buttonImage.gameObject.SetActive(true);
        }
        
        public void SetToggleState(bool newState, bool invoke = false)
        {
            var onValueChanged = buttonToggle.onValueChanged;
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = newState;
            buttonToggle.onValueChanged = onValueChanged;
            //buttonToggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_0(newState);
            //buttonToggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_1(newState);
            //buttonToggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_2(newState);
            //buttonToggle.GetComponent<ToggleIcon>().Method_Private_Void_Boolean_PDM_3(newState);
            if (invoke)
            {
                buttonToggle.onValueChanged.Invoke(newState);
            }
        }
        
        public void SetInteractable(bool val)
        {
            buttonToggle.interactable = val;
        }
        
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }*/
    }
}