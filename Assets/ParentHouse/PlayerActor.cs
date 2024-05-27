using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class PlayerActor : MonoBehaviour {
        [SerializeField] private ItemLookupTable ItemLookupTable;
        [SerializeField] private Actor playerActor;
        [SerializeField] private List<GearOption> defaultOutfit = new();
        [SerializeField] private Color defaultHairColor;

        private void OnEnable() {
            LoadOutfit();
        }

        private void OnDisable() {
            SaveOutfit();
        }

        [FoldoutGroup("Saving and Loading")]
        [Button]
        public void SaveOutfit() {
            print(playerActor.hairColor);
            // SaveLoad.SavePlayerOutfit(new PlayerOutfitSaveData(playerActor.EquippedGear.ToArray(),playerActor.hairColor));
        }

        [FoldoutGroup("Saving and Loading")]
        [Button]
        public void LoadOutfit() {
            // playerActor.EquippedGear.Clear();
            // PlayerOutfitSaveData loadedData = SaveLoad.LoadPlayerOutfit();
            //
            // if (loadedData is null)
            // {
            //     playerActor.EquippedGear = defaultOutfit;
            //     playerActor.hairColor = defaultHairColor;
            //     SaveOutfit();
            //     return;
            // }
            //
            // for (var i = 0; i < loadedData.GearSaveDataItems.Length; i++)
            // {
            //     var n = loadedData.GearSaveDataItems[i];
            //     var t = loadedData.GearTypeSaveDataItems[i];
            //     Gear loadedGear = (Gear) ItemLookupTable.GetItem(n);
            //
            //     playerActor.EquippedGear.Add(new GearOption(loadedGear, (GearType)t));
            //     try
            //     {
            //         var slot = (Animator.gearSlots.First(o => o.gear.GearType == loadedGear.GearType));
            //         if (slot != null)
            //             slot.gear = loadedGear;
            //     }
            //     catch
            //     {
            //         // ignored
            //     }
            // }
            // if (ColorUtility.TryParseHtmlString(loadedData.HairColorHexCode, out Color c))
            // {
            //     playerActor.hairColor = c;
            // }
            // Animator.UpdateActor(playerActor);
        }
    }

    [Serializable]
    public class PlayerOutfitSaveData {
        public string[] GearSaveDataItems;
        public int[] GearTypeSaveDataItems;
        public string HairColorHexCode;

        public PlayerOutfitSaveData(GearOption[] outfit, Color c) {
            GearSaveDataItems = new string[outfit.Length];
            GearTypeSaveDataItems = new int[outfit.Length];
            HairColorHexCode = $"#{ColorUtility.ToHtmlStringRGB(c)}";

            for (var i = 0; i < outfit.Length; i++) {
                if (outfit[i].gear != null)
                    GearSaveDataItems[i] = outfit[i].gear.Name;
                GearTypeSaveDataItems[i] = (int) outfit[i].type;
            }
        }
    }
}