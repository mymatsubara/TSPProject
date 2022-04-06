using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCamera : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    public float sensHor = 5.0f;
    public float sensVer = 5.0f;

    public float maxVer = 80.0f;
    public float minVer = -80.0f;

    private float rotX;

    // Start is called before the first frame update
    void Start()
    {
        rotX = transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Rotation
        float rotY = Input.GetAxis("Mouse X") * sensVer;
        _parent.Rotate(0, rotY, 0);

        // Vertical Rotation
        rotX = Mathf.Clamp(rotX - Input.GetAxis("Mouse Y") * sensHor, minVer, maxVer);
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
    }
}
