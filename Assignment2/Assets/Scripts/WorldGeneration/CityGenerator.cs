using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;


public class CityGenerator : MonoBehaviour
{
    [Header("Map Values")]
    public int mapWidth;
    public int mapHeight;
    Transform currentBlock;
    EBlockType[,] cityBlockMap;

    [Header("Variation Values")]
    public float noiseScale;
    public int octaves;
    public int seed;
    public Vector2 offset;

    [Header("Other")]
    public bool autoUpdate;
    public CityBlockType[] blockVariations;

    Queue<CityThreadInfo<EBlockType[,]>> cityDataThreadInfoQueue = new Queue<CityThreadInfo<EBlockType[,]>>();

    public void GenerateCity()
    {
        //Overload method which was originally used to generate the city in editor.
    }

    public EBlockType[,] GenerateCity(Transform _parent)
    {
        currentBlock = _parent;

        //Generate a noisemap using our NoiseGenerator class.
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, offset);

        //From this noisemap, create an array of city blocks.
        cityBlockMap = new EBlockType[mapWidth, mapHeight];


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

        return cityBlockMap;
    }


    //Threading for small performance gains.
    public void RequestCity(Action<EBlockType[,]> callback)
    {
        ThreadStart threadStart = delegate
        {   //Set up the desired thread.
            CityDataThread(callback);
        };
        //Start the desired thread.
        new Thread(threadStart).Start();
    }


    void CityDataThread(Action<EBlockType[,]> callback)
    {   
        EBlockType[,] cityData = GenerateCity(currentBlock);
        lock (cityDataThreadInfoQueue)
        {   //Lock the thread so it cannot be accessed by 2 entities at the same time.
            cityDataThreadInfoQueue.Enqueue(new CityThreadInfo<EBlockType[,]>(callback, cityData));
        }
    }


    private void Update()
    {
        if(cityDataThreadInfoQueue.Count > 0)
        {   //If our thread queue has something in it, hande that something.
            for (int i = 0; i < cityDataThreadInfoQueue.Count; i++)
            {
                CityThreadInfo<EBlockType[,]> threadInfo = cityDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }


    public void DrawTheBlock(CityDisplay city)
    {   //Instruct the CityDisplay to actually spawn/draw the city.
        city.DrawCity(cityBlockMap);
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


    struct CityThreadInfo<T>
    {   //Struct for our ciy generation thread.
        public readonly Action<T> callback;
        public readonly T parameter;

        public CityThreadInfo(Action<T> callback, T parameter)
        {   //Set the instance callback to the truct callback.
            this.callback = callback;
            this.parameter = parameter;
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