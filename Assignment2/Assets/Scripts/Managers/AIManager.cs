﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIManager : MonoBehaviour
{
    [SerializeField] float maxSpawnDist = 400;
    [SerializeField] AIZombie[] zombiePrefab;

    int activeAI = 0;
    public int targetActiveAI = 0;

    GameObject player;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {   //If there is no tagged player present, for whatever reason, throw out an error.
            Debug.Log("[ERROR] No player character found by the AIManager. Check player vehicle has spawned with correct tags.");
        }
    }


    void Update()
    {
        while(activeAI < targetActiveAI)
        {   //Could use IF statement instead, would do the same thing. Spawn 1 per frame.
            SpawnAI();
            break;
        }
    }


	void SpawnAI()
    {
        while (true)
        {   //Generate a random position within maxSpawnDist units of the player vehicle.
            Vector3 direction = Random.insideUnitCircle * maxSpawnDist;
            Vector3 hitPos = direction + player.transform.position;

            //Generate a NavMeshHit event on our generated position, this tries to find the closest position on a navmesh surface.
            NavMeshHit hit;
            NavMesh.SamplePosition(hitPos, out hit, 50, 1);

            if (hit.hit)
            {   //If we have a valid position, spawn an a random zombie on our navmesh position, increase the active AI by 1 and return, breaking out of the SpawnAI() function.
                Instantiate(zombiePrefab[Random.Range(0, (zombiePrefab.Length - 1))], hit.position, Quaternion.identity, this.transform);
                activeAI++;
                return;
            }
        }
    }


    void RemoveAI()
    {
        //Remove an AI from the list, then spawn a new one.
        activeAI--;
        SpawnAI();
    }
}