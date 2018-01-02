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
    {
        gen = this.gameObject.GetComponent<CityGenerator>();
        playerPos = new Vector2(player.position.x, player.position.z);
        UpdateVisible();
    }


    private void Update()
    {
        if (Vector2.Distance(playerPos, new Vector2(player.position.x, player.position.z)) > 10)
        {
            playerPos = new Vector2(player.position.x, player.position.z);
            UpdateVisible();
        }
        else if (start)
        {
            playerPos = new Vector2(player.position.x, player.position.z);
            UpdateVisible();
            start = false;
        }
    }


    void UpdateVisible()
    {
        int playerChunkX = Mathf.RoundToInt(playerPos.x / chunkSize);
        int playerChunkY = Mathf.RoundToInt(playerPos.y / chunkSize);

        for (int i = 0; i < cityChunksLast.Count; i++)
        {
            cityChunksLast[i].SetVisible(false);
        }

        cityChunksLast.Clear();

        for (int yOff = -chunksVisible; yOff <= chunksVisible; yOff++)
        {
            for (int xOff = -chunksVisible; xOff <= chunksVisible; xOff++)
            {
                Vector2 chunkCoord = new Vector2(playerChunkX + xOff, playerChunkY + yOff);
                Debug.Log(chunkCoord);

                if (cityChunkDictionary.ContainsKey (chunkCoord))
                {
                    cityChunkDictionary[chunkCoord].ForceUpdate();

                    if(cityChunkDictionary[chunkCoord].IsVisible())
                    {
                        cityChunksLast.Add(cityChunkDictionary[chunkCoord]);
                    }
                }
                else
                {
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
        {
            prefab = _prefab;
        }

        public CityChunk(Vector2 coord, int size, GameObject prefab, Transform parent)
        {
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
        {
            gen.GenerateCity(prefab.transform);
            gen.DrawTheBlock(prefab.GetComponent<CityDisplay>());
        }


        public void ForceUpdate()
        {
            float playerDist = Mathf.Sqrt(bounds.SqrDistance(playerPos));
            bool visible = playerDist <= viewDistance;
            SetVisible(visible);
        }


        public void SetVisible(bool _visible)
        {
            prefab.GetComponent<CityDisplay>().SetVisible(_visible);
        }


        public bool IsVisible()
        {
            return prefab.activeSelf;
        }
    }
}
