using System.Collections.Generic;
using System.Linq;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class GearSlot : MonoBehaviour {
        [SerializeField] public Gear gear;
        [SerializeField] public GearType gearType;

        [FoldoutGroup("Display")] [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField] private List<Sprite> sheet = new();

        [FoldoutGroup("Data")] [SerializeField]
        public int frameIndex;

        [FoldoutGroup("Data")] [SerializeField]
        private int startIndex;

        [FoldoutGroup("Data")] [SerializeField]
        private int frameCount;

        [FoldoutGroup("Data")] [SerializeField]
        private AnimationSheet[] animationData;

        public CustoAnimator parentAnimator;

        private int currentAnimationIndex;
        private int currentDirection;

        private void Awake() {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeGear(Gear newGear) {
            gear = newGear;
            LoadSpriteSheet(gear.spriteSheetId);
            parentAnimator.SyncFrames();
        }

        public void Tick(int targetDirection, bool flip) {
            currentDirection = targetDirection;
            spriteRenderer.flipX = flip;
            Animate();
        }

        private void SetSprite() {
            var index = startIndex + currentDirection * frameCount + frameIndex;
            Debug.Log($"Sheet Length {sheet.Count}, index: {index} (GearSlot.cs)");
            spriteRenderer.sprite = sheet[index];
        }

        private void Animate() {
            if (sheet.Count <= 0) return;
            frameIndex++;
            if (frameIndex > frameCount - 1)
                frameIndex = 0;
            SetSprite();
        }

        private void LoadSpriteSheet(string sheetName) {
            sheet.Clear();
            sheet = Resources.LoadAll<Sprite>(sheetName).ToList();
        }

        public void LoadAnimation(int actionIndex) {
            if (actionIndex == -1) return;
            currentAnimationIndex = actionIndex;
            startIndex = animationData[actionIndex].startIndex;
            frameCount = animationData[actionIndex].frameCount;
            if (frameCount <= 0)
                frameCount = 8;
        }

        public void ClearAnimation() {
            spriteRenderer.sprite = null;
            sheet.Clear();
        }

        public void ResetAnimation() {
            LoadAnimation(currentAnimationIndex);
        }
    }
}