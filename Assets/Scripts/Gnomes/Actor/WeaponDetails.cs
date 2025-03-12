using Sirenix.OdinInspector;
using UnityEngine;

namespace Gnomes.Actor {
    [CreateAssetMenu(fileName = "New Weapon Details", menuName = "GNOME/Weapon Details")]
    public class WeaponDetails : SerializedScriptableObject {
        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public string WeaponName;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public GameObject SpawnOnUseObject;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public float Range = 1f;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")]
        [SerializeField]
        // Todo: Move to weapon behavior?
        public float DamageLinger = 0.1f;

        [HorizontalGroup("DetailsSplit", 0.8f), VerticalGroup("DetailsSplit/Left")] [SerializeField]
        public float BaseDamageValue = 1f;

        [HorizontalGroup("DetailsSplit", 0.2f), VerticalGroup("DetailsSplit/Right")]
        [SerializeField, PreviewField(100), HideLabel]
        public Sprite WeaponIcon;
    }
}