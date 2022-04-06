using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] private string name;
    private void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(name);
        Destroy(gameObject);
    }
}
