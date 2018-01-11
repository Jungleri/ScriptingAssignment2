using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    public CoinManager coinManager;

    [SerializeField] float myScoreValue;
    [SerializeField] Rigidbody rb;

    bool dead = false;


    public void DestroyCoin()
    {   //When we want to manually destroy a coin, call this function.
        coinManager.CoinRemoved(myScoreValue, false);
        dead = true;
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.CompareTag("Player") && !dead)
        {   //If the player hit us, inform the coin manager.
            coinManager.CoinRemoved(myScoreValue, true);
            dead = true;

            StartCoroutine(CoinTimeout());
        }
        else
        {   //If it wasnt the player, we don't care.
            return;
        }
    }


    IEnumerator CoinTimeout()
    {   //After 1 second passes, the coin will be destroyed.
        rb.AddForce(Vector3.up * 10);
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}