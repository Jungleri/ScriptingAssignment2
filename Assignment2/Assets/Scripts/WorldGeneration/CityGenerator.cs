﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CityGenerator : MonoBehaviour
{
    [Header("Map Values")]
    public int mapWidth;
    public int mapHeight;

    [Header("Variation Values")]
    public float noiseScale;
    public int octaves;
    public int seed;
    public Vector2 offset;

    [Header("Other")]
    public bool autoUpdate;
    public CityBlockType[] blockVariations;


    public void GenerateCity()
    {
        //Generate a noisemap using our NoiseGenerator class.
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, offset);

        //From this noisemap, create an array of city blocks.
        EBlockType[,] cityBlockMap = new EBlockType[mapWidth,mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //For each tile in our map, compare it against the list of city block types we created.
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < blockVariations.Length; i++)
                {
                    //Go through each of the blockVariations available, and compare the current tiles value, matching it with a prefab.
                    if(currentHeight <= blockVariations[i].shareOfThisType)
                    {
                        //If ([y * mapWidth + x]) sets the 1D array point from our 2 map size values.
                        cityBlockMap[x,y] = blockVariations[i].blockType;
                        break;
                    }
                }

            }
        }

        //Find our gameobject that 
        CityDisplay city = FindObjectOfType<CityDisplay>();
        if(!city)
        {   //If there is no CityDisplay, warn the user.
            Debug.Log("[Error] No city render objet present! Please add an object of type 'CityDisplay'.");
        }
        else
        {
            city.DrawCity(cityBlockMap);
        }
    }


    void OnValidate()
    {
        //Some values will likely cause problems so they require validating, to make sure our noise generator script can output a valid map.
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }

        if (mapHeight < 1)
        {
            mapHeight = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}


[System.Serializable]
public struct CityBlockType
{
    //Simple struct which contains the information required to assign the block map a value (name is for designer convenience).
    public string name;
    public float shareOfThisType;
    public EBlockType blockType; 
}