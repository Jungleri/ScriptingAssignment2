using UnityEngine;


public class SkylineFollowPlayer : MonoBehaviour
{
    GameObject player;


	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(!player)
        {   //If there is no tagged player present, for whatever reason, remove the skyline as it could interfere.
            Debug.Log("ERROR: No player character present, removing skyline.");
            DestroyObject(this);
        }
	}


    void FixedUpdate()
    {
        if (player)
        {   //Update the skline's x/z to match the player vehicle.
            this.transform.position = new Vector3(player.transform.position.x, -5f, player.transform.position.z);
        }
	}
}
