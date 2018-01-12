using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.AI;

public class InfiniteWorld : MonoBehaviour
{

    [SerializeField] const float viewDistance = 180;
    [SerializeField] static CityGenerator gen;
    public Transform player;
    public static Vector2 playerPos;
    int chunkSize = 240;
    int chunksVisible = 1;
    public GameObject displayPrefab;
    bool start = true;

    Dictionary<Vector2, CityChunk> cityChunkDictionary = new Dictionary<Vector2, CityChunk>();
    List<CityChunk> cityChunksLast = new List<CityChunk>();


    private void Start()
    {   //When we begin, setup some values.
        player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerVehicle.transform;
        gen = this.gameObject.GetComponent<CityGenerator>();
        playerPos = new Vector2(player.position.x, player.position.z);
        //Call UpdateVisible so the world is prepared for the player on game start.
        UpdateVisible();
    }


    private void Update()
    {
        if (player != null)
        {   //If there is a player character, 
            if (Vector2.Distance(playerPos, new Vector2(player.position.x, player.position.z)) > 10)
            {   //If the player has moved > 10 units, call check with UpdateVisible() whether we need to spawn/despawn blocks.
                playerPos = new Vector2(player.position.x, player.position.z);
                UpdateVisible();
            }
            else if (start)
            {   //This only runs on the first update frame. More as a precaution than anything.
                playerPos = new Vector2(player.position.x, player.position.z);
                UpdateVisible();
                start = false;
            }
        }
        else
        {   //If there is no player, attempt to get reference to it from the game manager.
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerVehicle.transform;
        }
    }


    void UpdateVisible()
    {   //Find which chunk the player is currently in, using their position and the predetermined chunk size.
        int playerChunkX = Mathf.RoundToInt(playerPos.x / chunkSize);
        int playerChunkY = Mathf.RoundToInt(playerPos.y / chunkSize);

        for (int i = 0; i < cityChunksLast.Count; i++)
        {   //All previously visible chunks are now set to inactive. We will construct this frame's visible chunks below.
            cityChunksLast[i].SetVisible(false);
        }

        //Clear the last frame's visible chunk list now that they are inactive.
        cityChunksLast.Clear();

        //In a grid around the player, go through each chunk.
        for (int yOff = -chunksVisible; yOff <= chunksVisible; yOff++)
        {
            for (int xOff = -chunksVisible; xOff <= chunksVisible; xOff++)
            {
                //The chunks corresponding to these grid positions, are set to active. One way or another.
                Vector2 chunkCoord = new Vector2(playerChunkX + xOff, playerChunkY + yOff);

                //The CityChunkDictionary contains all of the coordinates that have been spawned at some point.
                if (cityChunkDictionary.ContainsKey (chunkCoord))
                {   //If there is an entry in the dictionary corresponding to this coordinate, make it update itself.
                    cityChunkDictionary[chunkCoord].ForceUpdate();

                    if(cityChunkDictionary[chunkCoord].IsVisible())
                    {   //If, after updating, the chunk is visible, add it to this frames list of active chunks.
                        cityChunksLast.Add(cityChunkDictionary[chunkCoord]);
                    }
                }
                else
                {   //If there isn't a currently avaiable chunk, spawn a new one and add it to the dictionary of chunks.
                    cityChunkDictionary.Add(chunkCoord, new CityChunk(chunkCoord, chunkSize, displayPrefab, this.transform));
                }
            }
        }
    }


    public class CityChunk
    {
        Vector2 pos;
        Bounds bounds;
        GameObject prefab;


        void UpdatePrefab(GameObject _prefab)
        {   //Simply set our prefab as the one passed in under the CityChunk() method.
            prefab = _prefab;
        }


        public CityChunk(Vector2 coord, int size, GameObject prefab, Transform parent)
        {   //With the given values, spawn a city block.
            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);
            prefab = GameObject.Instantiate(prefab, posV3, Quaternion.identity);
            prefab.transform.parent = parent;
            UpdatePrefab(prefab);

            gen.RequestCity(OnCityDataReceived);
            SetVisible(false);
        }


        void OnCityDataReceived(EBlockType[,]cityData)
        {   //When I have recieved the city data from the generation script, generate my block and draw it.
            gen.GenerateCity(prefab.transform);
            gen.DrawTheBlock(prefab.GetComponent<CityDisplay>());
        }


        public void ForceUpdate()
        {   //Calculate the distance between me and the player, set myself visible based on this calculation.
            float playerDist = Mathf.Sqrt(bounds.SqrDistance(playerPos));
            bool visible = playerDist <= viewDistance;
            SetVisible(visible);
        }


        public void SetVisible(bool _visible)
        {   //Update the city block with whatever value is passed in.
            prefab.GetComponent<CityDisplay>().SetVisible(_visible);
        }


        public bool IsVisible()
        {   //Am I visible/active?
            return prefab.activeSelf;
        }
    }
}