using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombie : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollBones;


	void Start ()
    {
		foreach (Rigidbody rb in ragdollBones)
        {
            rb.isKinematic = true;
        }
	}
	

	void Update ()
    {
		
	}
}
