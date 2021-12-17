using UnityEngine;
using UnityEngine.UI;

namespace ARES.Utils.API.QM
{
    public class SimpleButtonGroup
    {
        public readonly GameObject gameObject;
        public readonly RectMask2D parentMenuMask;
        
        public SimpleButtonGroup(Transform parent)
        {
            gameObject = Object.Instantiate(APIStuff.GetButtonGroupTemplate(), parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }
        
        public SimpleButtonGroup(MenuPage pge) : this(pge.menuContents)
        {
        }
        
        public void Destroy()
        {
            Object.Destroy(gameObject);
        }
        
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}