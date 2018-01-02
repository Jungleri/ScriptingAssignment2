using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinManager : MonoBehaviour
{
    public bool spawnCoins = false;
    public int targetCoins = 100;
    int currentCoins;
    [SerializeField] float maxSpawnDist = 350;
    [SerializeField] Coin[] coinPrefabs;

    GameObject player;
    ScoreManager scoreManager;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (!player)
        {   
            //If there is no tagged player present, for whatever reason, throw out an error.
            Debug.Log("[ERROR] No player character found by the AIManager. Check player vehicle has spawned with correct tags.");
        }

        scoreManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ScoreManager>();
    }


    void Update ()
    {
		while(currentCoins < targetCoins)
        {
            //Could use IF statement instead, would do the same thing. Spawn 1 per frame with the break.
            SpawnCoin();
            break;
        }
	}


    void SpawnCoin()
    {
        while(true)
        {
            //Generate a random position within maxSpawnDist units of the player vehicle.
            Vector2 direction = Random.insideUnitCircle * maxSpawnDist;
            Vector3 hitPos = new Vector3(direction.x, 10, direction.y) + player.transform.position;

            if (!(Physics.Raycast(new Ray(hitPos, Vector3.down * 5), 6)))
            {
                //If we are not inside a building, spawn a coin and increase the active coin count.
                Coin coin = Instantiate(coinPrefabs[Random.Range(0, (coinPrefabs.Length - 1))], hitPos, Quaternion.identity, this.transform);
                coin.coinManager = this;
                currentCoins++;
                return;
            }
        }
    }


    public void CoinRemoved(float _score, bool collected)
    {
        if(collected)
        {
            //If the coin was collected, and didnt get removed in some other way, increase the game score.
            scoreManager.IncreaseScore(_score);
        }
        //Remove a coin from our active count so it can be replenished.
        currentCoins--;
    }
}
