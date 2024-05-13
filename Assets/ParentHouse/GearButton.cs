using System.Collections.Generic;
using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ParentHouse {
    public class GearButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] private GameObject highlight;
        [field: SerializeField] private Image gearIcon;
        [field: SerializeField] private Sprite emptyGearIcon;
    
        [field: SerializeField, FoldoutGroup("Debug"), ReadOnly] private Actor Actor;
        [field: SerializeField, FoldoutGroup("Debug"), ReadOnly] private Gear Gear;
        [field: SerializeField, FoldoutGroup("Debug"), ReadOnly] private GearType Type;

        private static readonly List<GearButton> highlightedButtons = new();
        private static GearButton hoveredGearButton;
    
        public void Setup(Actor actor, Gear gear, GearType type)
        {
            Actor = actor;
            Gear = gear;
            Type = type;
        
            var option = Actor.EquippedGear.FirstOrDefault(o => o.gear == Gear);
            if(option!=null && option.gear == Gear)
                ReplaceHighlight();
            else ToggleEquipHighlight(false);
        
            gearIcon.sprite = Gear != null ? gear.Sprite : emptyGearIcon;
        }
    
        public void Equip()
        {
            var option = Actor.EquippedGear.FirstOrDefault(o => o.type == Type);
            if (option != null)
            {
                option.gear = Gear;
                GameEvents.OnUpdateOutfit.Invoke(Actor);
                ReplaceHighlight();
            }
        
        }

        private void ToggleEquipHighlight(bool state)
        {
            highlight.SetActive(state);
        }

        private void ToggleHoveredHighlight(bool state)
        {
        
        }

        private void ReplaceHighlight()
        {
            var option = highlightedButtons.FirstOrDefault(o => o.Type == Type);
            if (option != null)
            {
                option.ToggleEquipHighlight(false);
                highlightedButtons.Remove(option);
            }
            ToggleEquipHighlight(true);
            highlightedButtons.Add(this);
        }
    

        public void OnPointerEnter(PointerEventData eventData)
        {
            if ((hoveredGearButton is null || hoveredGearButton != this))
            {
                if (hoveredGearButton is not null) hoveredGearButton.ToggleHoveredHighlight(false);
                hoveredGearButton = this;
                ToggleHoveredHighlight(true);
            }

            if (Gear != null)
            {
                GameEvents.OnShowTooltip.Invoke(Gear.Name);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoveredGearButton != null)
            {
                hoveredGearButton.ToggleHoveredHighlight(false);
                hoveredGearButton = null;   
            }
            GameEvents.OnMouseExitInventorySlot.Invoke();
            GameEvents.OnHideTooltip.Invoke();
        }
    }
}
