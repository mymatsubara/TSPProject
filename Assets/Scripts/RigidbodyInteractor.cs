using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyInteractor : MonoBehaviour
{
    public float pushForce = 5.0f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody target = hit.collider.GetComponent<Rigidbody>();
        if (target != null && !target.isKinematic)
        {
            target.AddForce(hit.moveDirection * pushForce);
        }
    }
}
