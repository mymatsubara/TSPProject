using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GroundDetector : MonoBehaviour
{
    public float groundDistance = 0.1f;

    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);

        return hit.distance < _controller.height / 2 + groundDistance;        
    }
}
