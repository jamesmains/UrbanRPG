using ParentHouse.UI;
using ParentHouse.Utils;
using UnityEngine.EventSystems;

namespace ParentHouse {
    public class NeedDisplayBar : DisplayBar, IPointerEnterHandler, IPointerExitHandler
    {
        public Need targetNeed;
    
        protected void Update()
        {
            UpdateRadialBar(targetNeed.Value / 100);
        }
    
        public void Setup()
        {
            if(nameText!=null) nameText.text = targetNeed.Name;
            if(descriptionText!=null) descriptionText.text = targetNeed.Description;
            if (iconImage != null) iconImage.sprite = targetNeed.Icon;
            if (fillBarImage != null) fillBarImage.sprite = targetNeed.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameEvents.OnShowTooltip.Invoke($"{targetNeed.Name}\n{targetNeed.Description}");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEvents.OnHideTooltip.Invoke();
        }
    }
}
