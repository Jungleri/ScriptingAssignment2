using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoinManager : MonoBehaviour
{
    [Header("Importnt Values")]
    public bool spawnCoins = false;
    public int targetCoins = 100;
    int currentCoins;
    [SerializeField] float maxSpawnDist = 350;
    [SerializeField] Coin[] coinPrefabs;

    [Header("Object References")]
    GameObject player;
    ScoreManager scoreManager;
    GameManager gameManager;


    void Awake()
    {
        player = player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerVehicle;

        if (!player)
        {   //If there is no tagged player present, for whatever reason, throw out an error.
            Debug.Log("[ERROR] No player character found by the AIManager. Check player vehicle has spawned with correct tags.");
            player = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().playerVehicle;
        }

        //Grab the various managing entities we require.
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        scoreManager = gm.GetComponent<ScoreManager>();
        gameManager = gm.GetComponent<GameManager>();

        //Inform the game manager of our presence.
        gameManager.coinMang = this;
    }


    void Update ()
    {
        if (player == null)
        {   //If for whatever reason, the player gets removed, regrab a reference to her.
            player = gameManager.playerVehicle;
        }

        if(currentCoins < targetCoins && spawnCoins)
        {   //If we can spawn a coin, do so.
            SpawnCoin();
        }
	}


    void SpawnCoin()
    {   
        if(player == null)
        {
            return;
        }

        //Generate a random position within maxSpawnDist units of the player vehicle.
        Vector2 direction = Random.insideUnitCircle * maxSpawnDist;
        Vector3 hitPos = new Vector3(direction.x, 10, direction.y) + player.transform.position;
  
        if (!(Physics.Raycast(new Ray(hitPos, Vector3.down * 5), 6)))
        {   //If we are not inside a building, spawn a coin and increase the active coin count.
            Coin coin = Instantiate(coinPrefabs[Random.Range(0, (coinPrefabs.Length - 1))], hitPos, Quaternion.identity, this.transform);
            coin.coinManager = this;
            currentCoins++;
            return;
        }
    }


    public void CoinRemoved(float _score, bool collected)
    {
        if(collected)
        {   //If the coin was collected, and didnt get removed in some other way, increase the game score.
            scoreManager.IncreaseScore(_score);
        }
        //Remove a coin from our active count so it can be replenished.
        currentCoins--;
    }


    public void ClearRound()
    {   //Find all of the coins we spawned and destroy them. A new round is beginning.
        spawnCoins = false;
        for (int i = 0; i < targetCoins; i++)
        {
            Coin _coin = GameObject.FindObjectOfType<Coin>();
            _coin.DestroyCoin();
        }
    }


    public void NewRound(int _level)
    {   //A new round has begun, update the target coins and allow the spawning.
        targetCoins = Mathf.RoundToInt(125 - (Mathf.Pow((_level * 1.5f), 2)));
        spawnCoins = true;
    }
}