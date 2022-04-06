using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY,
        MouseX,
        MouseY
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float horSensitivity = 9.0f;
    public float verSensitivity = 9.0f;

    public float maxVertical = 45.0f;
    public float minVertical = -45.0f;

    private float _rotationX = 0;

    private void Start()
    {
        _rotationX = transform.localEulerAngles.x;

        Rigidbody body = GetComponent<Rigidbody>();

        if (body != null)
            body.freezeRotation = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * horSensitivity, 0);
        } else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * verSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minVertical, maxVertical);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        } else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * verSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minVertical, maxVertical);

            float rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * horSensitivity;

            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }           
    }
}
