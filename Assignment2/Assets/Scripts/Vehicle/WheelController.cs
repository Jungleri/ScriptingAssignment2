using UnityEngine;


public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider wc;
    [SerializeField] Transform wheelGeo;


	void FixedUpdate ()
    {   //Update the wheel's geometry to the wheel colliders position.
        Vector3 _position;
        Quaternion _rotation;    

        wc.GetWorldPose(out _position, out _rotation);

        wheelGeo.position = _position;
        wheelGeo.rotation = _rotation;

	}
}
