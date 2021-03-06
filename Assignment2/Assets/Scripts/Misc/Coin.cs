﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    public CoinManager coinManager;

    [SerializeField] float myScoreValue;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioClip sound;

    bool dead = false;


    public void DestroyCoin()
    {   //When we want to manually destroy a coin, call this function.
        coinManager.CoinRemoved(myScoreValue, false, this);
        dead = true;
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision _col)
    {
        if(_col.gameObject.CompareTag("Player") && !dead)
        {   //If the player hit us, inform the coin manager.
            coinManager.CoinRemoved(myScoreValue, true, this);
            dead = true;

            AudioSource.PlayClipAtPoint(sound, this.transform.position);
            Destroy(this.gameObject);
        }
        else
        {   //If it wasnt the player, we don't care.
            return;
        }
    }
}