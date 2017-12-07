using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollision : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    private void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.GetComponentInParent<AIZombie>())
        {   //If the collided object is an AI, get its controller component and send it the collision event.
            AIZombie ai = _col.gameObject.GetComponentInParent<AIZombie>();
            ai.CollideWith(rb.velocity);
        }
    }
}
