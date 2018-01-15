using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//######################## DEPRECATED########################
public class AIZombie : MonoBehaviour
{
    [SerializeField] float requiredHitImpact;
    [SerializeField] float myScoreValue = 10;
    public AIManager aiManager;
    bool dead = false;

    [Header("MyComponents")]
    [SerializeField] Animator animControl;
    [SerializeField] Rigidbody[] myBones;
    [SerializeField] NavMeshAgent nma;


    void Awake()
    {
        SetKinematic(true);
    }


    private void FixedUpdate()
    {
        if(!dead)
        {   //If I am not dead, wander around the map.
            if (!nma.hasPath)
            {   //Generate a random position we will try and move towards.
                Vector2 direction = Random.insideUnitCircle * 20;
                Vector3 hitPos = new Vector3(direction.x, 0, direction.y) + this.transform.position;

                NavMeshHit hit;
                NavMesh.SamplePosition(hitPos, out hit, 5, 1);

                if(hit.hit)
                {   //If the sample position was a hit, send it to our NavMeshAgent.
                    nma.SetDestination(hit.position);
                }
            }
        }
    }


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
        if (!dead)
        {   //Assign myself a layer which doesn't collide with the player anymore.
            this.gameObject.layer = 8;
            for (int i = 0; i < myBones.Length; i++)
            {
                myBones[i].gameObject.layer = 8;
            }
            //Inform the AI manager that I have been hit.
            aiManager.AIDied(myScoreValue, true);
            //Start the despawner timer.
            StartCoroutine(CorpseTimeout());
            //Enable ragdolling.
            SetKinematic(false);
            animControl.enabled = false;
            //Disable our NavMeshAgent, we dont need it anymore.
            nma.enabled = false;
            dead = true;
        }
    }


    void SetKinematic(bool _value)
    {
        for (int i = 0; i < myBones.Length; i++)
        {   //For each of our bones, update the Kinematic setting.
            myBones[i].isKinematic = _value;
        }
    }


    IEnumerator CorpseTimeout()
    {   //After 5 seconds, destroy the character/body.
        yield return new WaitForSeconds(5);
        Debug.Log("Dead");
        Destroy(this.gameObject);
    }
}