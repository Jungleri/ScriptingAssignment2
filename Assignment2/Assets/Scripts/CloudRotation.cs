using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudRotation : MonoBehaviour
{

    [SerializeField] float rotSpeed = 1;

	void Update ()
    {
        this.transform.Rotate(new Vector3(0, Time.deltaTime * rotSpeed, 0));
	}
}