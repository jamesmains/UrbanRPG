using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float moveSpeed; // might be replaced with scriptable object float variable 
    private float inputX, inputY;
    private bool horizontalFlip;

    private void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        horizontalFlip = inputX > 0 ? false: inputX < 0 ? true : horizontalFlip;
        bool isMoving = (inputX != 0 || inputY != 0);
        animator.SetBool("isMoving",isMoving);
        spriteRenderer.flipX = horizontalFlip;
        Vector3 moveForce = new Vector3(inputX,0, inputY).normalized;
        rb.AddForce(moveForce * moveSpeed); 
    }
    public void MovePlayerTo(Transform newPosition)
    {
        transform.position = newPosition.position;
    }
}
