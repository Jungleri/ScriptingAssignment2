using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    [SerializeField]
    private WheelCollider wc;

    [SerializeField]
    private Transform wheelGeo;

	void FixedUpdate ()
    {
        Vector3 _position;
        Quaternion _rotation;    

        wc.GetWorldPose(out _position, out _rotation);

        wheelGeo.position = _position;
        wheelGeo.rotation = _rotation;

	}
}
