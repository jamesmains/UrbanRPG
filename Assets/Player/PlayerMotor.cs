using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CustoAnimator animator;
    [SerializeField] private VectorVariable playerPositionVariable;
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
        bool isMoving = (inputX != 0 || inputY != 0);
        int action = isMoving ? 1 : 0;
        animator.ChangeDirection(new Vector2((int)inputX,(int)inputY),action);
        Vector3 moveForce = new Vector3(inputX,0, inputY).normalized;
        rb.AddForce(moveForce * moveSpeed);
        playerPositionVariable.Value = transform.position;
    }
    public void MovePlayerTo(Transform newPosition)
    {
        transform.position = newPosition.position;
    }
}
