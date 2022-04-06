using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpeningDevice : MonoBehaviour
{
    [SerializeField] private Vector3 dPos;

    private bool _open;    

    // Update is called once per frame
    public void Operate()
    {
        if (_open)
        {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
        } else
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
        }
        _open = !_open;
    }
}
