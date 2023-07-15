using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner confiner;

    private void Awake()
    {
        GameObject boundingBox = GameObject.FindWithTag("CameraBoundingBox");
        if (boundingBox == null)
        {
            Destroy(confiner);
            Destroy(this);
            return;
        }

        confiner.m_BoundingVolume = boundingBox.GetComponent<MeshCollider>();
    }
}
