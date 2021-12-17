using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements.Controls;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class Tab
    {
        private readonly GameObject gameObject;
        private readonly MenuTab menuTab;
        private readonly Image tabIcon;
        private readonly GameObject badgeGameObject;
        private readonly TextMeshProUGUI badgeText;
        
        public Tab(Transform parent, string menuName, string tooltip, Sprite icon = null)
        {
            gameObject = Object.Instantiate(APIStuff.GetMenuTabTemplate(), parent);
            gameObject.name = menuName + "Tab";
            menuTab = gameObject.GetComponent<MenuTab>();
            menuTab.field_Private_MenuStateController_0 = APIStuff.GetMenuStateControllerInstance();
            menuTab.field_Public_String_0 = menuName;
            tabIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
            tabIcon.sprite = icon;
            tabIcon.overrideSprite = icon;
            badgeGameObject = gameObject.transform.GetChild(0).gameObject;
            badgeText = badgeGameObject.GetComponentInChildren<TextMeshProUGUI>();
            menuTab.GetComponent<StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            menuTab.GetComponent<Button>().onClick.AddListener(new Action(() =>
            {
                menuTab.GetComponent<StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            }));
            gameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = tooltip;
        }
        
        public void SetBadge(bool showing = true, string text = "")
        {
            if (badgeGameObject == null || badgeText == null)
            {
                return;
            }
            badgeGameObject.SetActive(showing);
            badgeText.text = text;
        }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}