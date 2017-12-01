using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombie : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollBones;
    [SerializeField] float requiredHitImpact;

    bool dead = false;


	void Start ()
    {
		foreach (Rigidbody rb in ragdollBones)
        {   //Disable the ragdolling for all of my bones.
            rb.isKinematic = true;
        }
	}


    public bool CollideWithMe(Vector3 _velocity)
    {   //If we are hit with enough force to kill, return the result and call the Die() function.
            Debug.Log(_velocity.magnitude);
            if (_velocity.magnitude > requiredHitImpact && !dead)
            {
                Die();
                return true;
            }
        else
        {   //If I am dead or the impact wasn't enough, return false.
            return false;
        }
    }
	

    void Die()
    {
        foreach (Rigidbody rb in ragdollBones)
        {   //Enable the ragdolling for all of my bones.
            rb.isKinematic = false;
        }

        StartCoroutine(CorpseTimeout());
    }


    IEnumerator CorpseTimeout()
    {   //After 5 seconds, destroy the character/body.
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}