using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARES.Utils.API.QM
{
    public class ButtonGroup
    {
        private readonly TextMeshProUGUI headerText;
        public readonly GameObject gameObject;
        private readonly GameObject headerGameObject;
        public readonly RectMask2D parentMenuMask;
        
        public ButtonGroup(Transform parent, string text)
        {
            headerGameObject = Object.Instantiate(APIStuff.GetButtonGroupHeaderTemplate(), parent);
            headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerText.text = text;
            gameObject = Object.Instantiate(APIStuff.GetButtonGroupTemplate(), parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }
        
        public ButtonGroup(MenuPage pge, string text) : this(pge.menuContents, text) {}
        
        public void SetText(string newText)
        {
            headerText.text = newText;
        }
        
        public void Destroy()
        {
            Object.Destroy(headerGameObject);
            Object.Destroy(gameObject);
        }
        
        public void SetActive(bool state)
        {
            if (headerGameObject != null)
            {
                headerGameObject.SetActive(state);
            }
            gameObject.SetActive(state);
        }

        public void ClearButtons()
        {
            gameObject.transform.DestroyChildren(null);
        }
    }
}