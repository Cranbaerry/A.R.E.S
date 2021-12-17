using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;
using VRC.UI.Elements;
using VRC.UI.Elements.Menus;
using Object = UnityEngine.Object;

namespace ARES.Utils.API.QM
{
    public class MenuPage
    {
        private readonly UIPage page;
        private readonly GameObject gameObject;
        public readonly Transform menuContents;
        private readonly TextMeshProUGUI pageTitleText;
        private readonly bool isRoot;
        public readonly GameObject backButtonGameObject;
        public readonly GameObject extButtonGameObject;
        private bool preserveColor;
        public readonly RectMask2D menuMask;
        
        public MenuPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool extButton = false, Action extButtonAction = null, string extButtonTooltip = "", Sprite extButtonSprite = null, bool preserveColor = false)
		{
			gameObject = Object.Instantiate(APIStuff.GetMenuPageTemplate(), APIStuff.GetMenuPageTemplate().transform.parent);
			gameObject.name = "Menu_" + menuName;
			gameObject.transform.SetSiblingIndex(5);
			gameObject.SetActive(false);
			Object.DestroyImmediate(gameObject.GetComponent<LaunchPadQMMenu>());
			page = gameObject.AddComponent<UIPage>();
			page.field_Public_String_0 = menuName;
			page.field_Private_Boolean_1 = true;
			page.field_Private_MenuStateController_0 = APIStuff.GetMenuStateControllerInstance();
			page.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
			page.field_Private_List_1_UIPage_0.Add(page);
			APIStuff.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName, page);
			if (root)
			{
				var list = APIStuff.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
				list.Add(page);
				APIStuff.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0 = list.ToArray();
			}
			gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
			menuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
			pageTitleText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
			pageTitleText.text = pageTitle;
			isRoot = root;
			backButtonGameObject = gameObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
			extButtonGameObject = gameObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject;
			backButtonGameObject.SetActive(backButton);
			backButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
			backButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(new Action(() =>
			{
				if (isRoot)
				{
					APIStuff.GetMenuStateControllerInstance().Method_Public_Void_String_Boolean_0("QuickMenuDashboard");
					return;
				}
				page.Method_Protected_Virtual_New_Void_0();
			}));
			extButtonGameObject.SetActive(extButton);
			extButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
			extButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(extButtonAction);
			extButtonGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = extButtonTooltip;
			if (extButtonSprite != null)
			{
				extButtonGameObject.GetComponentInChildren<Image>().sprite = extButtonSprite;
				extButtonGameObject.GetComponentInChildren<Image>().overrideSprite = extButtonSprite;
				if (preserveColor)
				{
					extButtonGameObject.GetComponentInChildren<Image>().color = Color.white;
					extButtonGameObject.GetComponentInChildren<StyleElement>(true).enabled = false;
				}
			}
			this.preserveColor = preserveColor;
			menuMask = menuContents.parent.gameObject.GetComponent<RectMask2D>();
			menuMask.enabled = true;
			gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = true;
			gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbar = gameObject.transform.Find("ScrollRect/Scrollbar").GetComponent<Scrollbar>();
			gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}
        
        public void AddExtButton(Action onClick, string tooltip, Sprite icon)
        {
	        var extBtn = Object.Instantiate(extButtonGameObject, extButtonGameObject.transform.parent);
	        extBtn.SetActive(true);
	        extBtn.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
	        extBtn.GetComponentInChildren<Button>().onClick.AddListener(onClick);
	        extBtn.GetComponentInChildren<Image>().sprite = icon;
	        extBtn.GetComponentInChildren<Image>().overrideSprite = icon;
	        extBtn.GetComponentInChildren<UiTooltip>().field_Public_String_0 = tooltip;
        }
        
        public void OpenMenu()
        {
	        if (isRoot)
	        {
		        APIStuff.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_0(page.field_Public_String_0);
		        return;
	        }
	        APIStuff.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_0(page.field_Public_String_0);
        }
        
        public void CloseMenu()
        {
	        page.Method_Public_Virtual_New_Void_0();
        }
    }
}