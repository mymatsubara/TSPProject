using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GroundDetector))]
public class GravityBody : MonoBehaviour
{
    public float standingSpeed = -1.5f;
    public float terminalSpeed = -30.0f;
    public float gravity = -40.0f;

    private CharacterController _controller;
    private GroundDetector _groundDetector;
    private float _speed;
    private ControllerColliderHit _contact;
    private float _defaultSlopeLimit;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _groundDetector = GetComponent<GroundDetector>();
        _speed = standingSpeed;
        _defaultSlopeLimit = _controller.slopeLimit;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _controller.Move(new Vector3(0, _speed * Time.deltaTime, 0));


        if (_controller.isGrounded && !_groundDetector.IsGrounded())
        {
            _controller.Move(_contact.normal * Time.deltaTime);
        }

        if (_controller.isGrounded)
        {
            _speed = standingSpeed;
            return;
        }


        
        _speed += gravity * Time.deltaTime;
        if (_speed < terminalSpeed)
        {
            _speed = terminalSpeed;
        }
    }

    public void SetVerticalSpeed(float speed)
    {
        _speed = speed;
    }

    public bool IsDetectorGrounded()
    {
        return _groundDetector.IsGrounded();
    }

    public bool IsControllerGrounded()
    {
        return _controller.isGrounded;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }

    public void IgnoreSlope()
    {
        _controller.slopeLimit = 90.0f;
    }

    public void ConsiderSlope()
    {
        _controller.slopeLimit = _defaultSlopeLimit;
    }
}
