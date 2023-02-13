using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private bool lockX, lockY, lockZ, flip;

    private void Awake()
    {
        if (flip)
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void Update()
    {
        
        transform.LookAt(Camera.main.transform.position);
        var rot = transform.rotation.eulerAngles;
        
        rot.x = lockX ? 0 : rot.x;
        rot.y = lockY ? 0 : rot.y;
        rot.z = lockZ ? 0 : rot.z;
        
        transform.rotation = Quaternion.Euler(rot);
    }
}
