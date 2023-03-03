using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCrossfade : MonoBehaviour
{
    [SerializeField] private Material[] skyboxes;
    [SerializeField] private float speed;

    void Update() {
        RenderSettings.skybox.Lerp(skyboxes[0], skyboxes[1], speed * Time.deltaTime);
        DynamicGI.UpdateEnvironment();
    }
}
