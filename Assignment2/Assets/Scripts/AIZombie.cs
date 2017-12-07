using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIZombie : MonoBehaviour
{
    [SerializeField] float requiredHitImpact;
    [SerializeField] float myScoreValue = 10;

    public AIManager aiManager;
    bool dead = false;



    public bool CollideWith(Vector3 _velocity)
    {   //If we are hit with enough force to kill, return the result and call the Die() function.
            Debug.Log(_velocity.magnitude);
            if (_velocity.magnitude > requiredHitImpact && !dead)
            {
                Die();
                return true;
            }
        else
        {   //If I am dead or the impact wasn't enough, return false.
            return false;
        }
    }


    void Die()
    {
        aiManager.AIDied(myScoreValue, true);
        StartCoroutine(CorpseTimeout());
        dead = true;
    }


    IEnumerator CorpseTimeout()
    {   //After 5 seconds, destroy the character/body.
        yield return new WaitForSeconds(5);
        Debug.Log("Dead");
        Destroy(this.gameObject);
    }
}