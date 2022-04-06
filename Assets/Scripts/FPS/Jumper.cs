using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityBody))]
[RequireComponent(typeof(Stats))]
public class Jumper : MonoBehaviour
{
    private GravityBody _gravityBody;
    private Stats _stats;
    private Animator _animator;

    private bool _isGrounded = false;

    private void Start()
    {
        _gravityBody = GetComponent<GravityBody>();
        _stats = GetComponent<Stats>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && _gravityBody.IsDetectorGrounded())
        {
            _gravityBody.SetVerticalSpeed(_stats.jumpSpeed);
            _animator?.SetBool("Jumping", true);
            _gravityBody.IgnoreSlope();
            _isGrounded = false;
        } else if (!_isGrounded && _gravityBody.IsControllerGrounded())
        {
            _animator?.SetBool("Jumping", false);
            _gravityBody.ConsiderSlope();
            _isGrounded = true;
        }
    }
}
