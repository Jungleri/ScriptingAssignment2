using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;
using UnityEngine;


public class CameraAssignTarget : MonoBehaviour
{
    GameObject player;


    void Update ()
    {
        if (!player)
        {   //If we do not currently have reference to the player character, grab it from the GameManager and assign it to the AutoCam script.
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerVehicle;
            this.GetComponent<AutoCam>().m_Target = player.transform;
        }
    }
}