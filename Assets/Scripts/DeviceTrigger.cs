using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _target;


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            _target.SendMessage("Operate", SendMessageOptions.RequireReceiver);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Test");
    }
}
