using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Kickable : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Vector3 forceMultiplier;
    [Button]
    public void Kick()
    {
        float x = Random.Range(-1f, 1f) * forceMultiplier.x;
        float y = Random.Range(0.25f, 1f) * forceMultiplier.y;
        float z = Random.Range(-1f, 1f) * forceMultiplier.z;
        rb.AddForce(new Vector3(x,y,z));
    }
}
