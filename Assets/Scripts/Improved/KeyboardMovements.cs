using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Stats))]
public class KeyboardMovements : MonoBehaviour
{
    private CharacterController _controller;
    private Stats _stats;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {        
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<Stats>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * _stats.movSpeed;
        float deltaZ = Input.GetAxis("Vertical") * _stats.movSpeed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, _stats.movSpeed) * Time.deltaTime;
        movement = transform.TransformVector(movement);
        
        _controller.Move(movement);

        _animator?.SetFloat("Speed", movement.sqrMagnitude * 10);
    }
}
