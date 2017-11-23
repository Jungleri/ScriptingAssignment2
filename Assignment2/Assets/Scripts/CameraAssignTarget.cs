﻿using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Cameras;
using UnityEngine;

public class CameraAssignTarget : MonoBehaviour
{
    [SerializeField]
    GameObject player;


    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //If there is no tagged player present, for whatever reason, throw out an error.
        if (!player)
        {
            Debug.Log("ERROR: No player character found by the camera. Check player vehicle has spawn with correct tags.");
        }
        else
        {
            this.GetComponent<AutoCam>().m_Target = player.transform;
        }
    }
}