using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject doorGfx;
    private bool active = false;
    private void PortalPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(DoPortal());
    }
    IEnumerator DoPortal()
    {
        doorGfx.SetActive(false);
        active = true;
        yield return new WaitForSeconds(.1f);
        FindObjectOfType<PlayerMotor>().MovePlayerTo(destination);
        doorGfx.SetActive(true);
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !active)
            PortalPlayer();
    }
}
