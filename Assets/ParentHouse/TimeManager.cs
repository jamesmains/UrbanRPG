using System.Collections.Generic;
using ParentHouse.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ParentHouse {
    public class TimeManager : MonoBehaviour {
        private const float InverseDayLength = 1f / 1440f;
        public const float DayLength = 1440f;

        [SerializeField] [Tooltip("How fast time will go")]
        public static float TimeMultiplier = 1;

        public static TimeManager Instance;

        [SerializeField] [Header("Managed Objects")]
        private Light DirectionalLight;

        [SerializeField] private LightPreset DayNightPreset, DayNightSkyboxPreset;
        [SerializeField] private TimeVariable currentTimeVariable;
        [SerializeField] private Material skyboxMaterial;

        [SerializeField] [Range(0, 1440)] [Header("Modifiers")] [Tooltip("The game's current time of day")]
        private float TimeOfDay;

        [SerializeField] [Tooltip("Angle to rotate the sun")]
        private float SunDirection = 170f;

        [SerializeField] private bool ControlLights = true;

        private List<Light> SpotLights = new();
        private float TimeRemainingInDay;

        private void Awake() {
            if (Instance != null) Destroy(this);
            Instance = this;
        }

        /// <summary>
        ///     On project start, if controlLights is true, collect all non-directional lights in the current scene and place in a
        ///     list
        /// </summary>
        private void Start() {
            TimeOfDay = currentTimeVariable.Value;
            TimeRemainingInDay = 1440 - TimeOfDay;
            if (ControlLights) {
                var lights = FindObjectsOfType<Light>();
                foreach (var li in lights)
                    switch (li.type) {
                        case LightType.Disc:
                        case LightType.Point:
                        case LightType.Rectangle:
                        case LightType.Spot:
                            SpotLights.Add(li);
                            break;
                        case LightType.Directional:
                        default:
                            break;
                    }
            }
        }

        /// <summary>
        ///     This method will not run if there is no preset set
        ///     On each frame, this will calculate the current time of day factoring game time and the time multiplier (1440 is how
        ///     many minutes exist in a day 24 x 60)
        ///     Then send a time percentage to UpdateLighting, to evaluate according to the set preset, what that time of day
        ///     should look like
        /// </summary>
        [Button]
        private void Update() {
#if UNITY_EDITOR
            TimeMultiplier = Input.GetKey(KeyCode.Space) ? 400 : 1;
#endif

            if (DayNightPreset == null)
                return;

            if (Global.PlayerLock > 0) TimeMultiplier = 0;

            TimeOfDay = TimeOfDay + Time.deltaTime * TimeMultiplier;
            TimeRemainingInDay -= Time.deltaTime * TimeMultiplier;
            currentTimeVariable.SetValue(TimeOfDay);

            if (TimeRemainingInDay <= 0) {
                TimeRemainingInDay = 1440;
                GameEvents.OnNewDay.Invoke();
            }

            TimeOfDay = TimeOfDay % 1440;
            UpdateLighting(TimeOfDay * InverseDayLength);
        }

        /// <summary>
        ///     Based on the time percentage recieved, set the current scene's render settings and light coloring to the preset
        ///     In addition, rotate the directional light (the sun) according to the current time
        /// </summary>
        /// <param name="timePercent"></param>
        private void UpdateLighting(float timePercent) {
            RenderSettings.ambientLight = DayNightPreset.AmbientColour.Evaluate(timePercent);
            RenderSettings.fogColor = DayNightPreset.FogColour.Evaluate(timePercent);

            //Set the directional light (the sun) according to the time percent
            if (DirectionalLight != null)
                if (DirectionalLight.enabled) {
                    DirectionalLight.color = DayNightPreset.DirectionalColour.Evaluate(timePercent);
                    DirectionalLight.transform.localRotation =
                        Quaternion.Euler(new Vector3(timePercent * 360f - 90f, SunDirection, 0));
                }

            if (DayNightSkyboxPreset != null) {
                skyboxMaterial.SetColor("_SkyGradientTop",
                    DayNightSkyboxPreset.DirectionalColour.Evaluate(timePercent));
                skyboxMaterial.SetColor("_SkyGradientBottom", DayNightSkyboxPreset.AmbientColour.Evaluate(timePercent));
            }
        }

        public float GetCurrentTimeOfDay() {
            return TimeOfDay;
        }

        public float GetTimeRemainingInDay() {
            return TimeRemainingInDay;
        }
    }
}