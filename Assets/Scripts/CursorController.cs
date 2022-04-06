using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    bool cursorActive = false;

    private void Start()
    {
        Cursor.lockState = !cursorActive ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = cursorActive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cursorActive = !cursorActive;
            Cursor.lockState = !cursorActive ? CursorLockMode.Locked : CursorLockMode.Confined;
            Cursor.visible = cursorActive;
        }
    }
}
