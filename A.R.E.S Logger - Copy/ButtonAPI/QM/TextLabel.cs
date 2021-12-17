using TMPro;
using UnityEngine;

namespace ARES.Utils.API.QM
{
    public class TextLabel
    {
        protected GameObject gameObject;
        protected TextMeshProUGUI text;

        public TextLabel(Transform location, string labelText, Color? textColor = null)
        {
            gameObject = Object.Instantiate(GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickLinks/LeftItemContainer/Text_Title"), location, false);
            text = gameObject.GetComponent<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.Center;
            text.text = labelText;
            text.autoSizeTextContainer = true;
            text.enableWordWrapping = false;
            text.fontSize = 32;
            if (textColor != null) text.color = (Color)textColor;
        }
        
        public TextLabel(MenuPage page, string labelText, Color? textColor = null) : this(page.menuContents, labelText, textColor) {}

        public void SetText(string newText)
        {
            text.text = newText;
        }

        public void Destroy()
        {
            Object.Destroy(gameObject);
        }
    }
}