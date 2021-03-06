﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CityDisplay : MonoBehaviour
{
    [SerializeField] float cityBlockWidth = 1f;
    [SerializeField] float cityBlockHeight = 1f;
    [SerializeField] CityBlockGeneration cityBlockBase;

    [SerializeField] GameObject environmentParent;
    List<GameObject> myBlocks = new List<GameObject>();


    public void SetVisible(bool _visible)
    {
        this.gameObject.SetActive(_visible);
    }

    public void DrawCity(EBlockType[,] cityMap)
    {   //Create an environment parent which hold all of the buildings out of the way.
        environmentParent = Instantiate(environmentParent, this.transform.position, Quaternion.identity, this.transform);

        //Get the map width and height so we can go through each one further down.
        float mapWidth = cityMap.GetLength(0);
        float mapHeight = cityMap.GetLength(1);

        //Find the top left tile, so we can keep 'this' object in the centre of the grid.
        float topLeftX = ((mapWidth - 1) / -2f) * cityBlockWidth;
        float topLeftZ = ((mapHeight - 1) / 2f) * cityBlockHeight;

        for (int y = 0; y < mapWidth; y++)
        {
            for (int x = 0; x < mapHeight; x++)
            {   //For each tile in our grid, starting at the top left, spawn the predetermined city block.
                Vector3 position = new Vector3(topLeftX + (x * cityBlockWidth), this.transform.position.y, topLeftZ - (y * cityBlockHeight));
                GameObject block = Instantiate(cityBlockBase.gameObject, position + this.transform.position, Quaternion.identity, environmentParent.transform);
                //Set the block type of the newly spawned city block.
                block.GetComponent<CityBlockGeneration>().blockType = cityMap[y, x];
                block.GetComponent<CityBlockGeneration>().SpawnMyBlock();
            }
        }
    }
}