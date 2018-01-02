using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coin : MonoBehaviour
{
    public CoinManager coinManager;
    [SerializeField] float requiredHitImpact;
    [SerializeField] float myScoreValue = 10;
    bool dead = false;


    public bool CollideWith(Vector3 _velocity)
    {   
        //If we are hit with enough force to kill, return the result and call the Die() function.
        Debug.Log(_velocity.magnitude);
        if (_velocity.magnitude > requiredHitImpact && !dead)
        {
            coinManager.CoinRemoved(myScoreValue, true);
            dead = true;
            return true;
        }
        else
        {  
            //If I am dead or the impact wasn't enough, return false.
            dead = true;
            return false;
        }
    }

    IEnumerator CoinTimeout()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}