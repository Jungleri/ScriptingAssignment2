using UnityEngine;


public class SkylineFollowPlayer : MonoBehaviour
{
    GameObject player;


    void FixedUpdate()
    {
        if (player)
        {   //Update the skline's x/z to match the player vehicle.
            this.transform.position = new Vector3(player.transform.position.x, -5f, player.transform.position.z);
        }
        else
        {   //If there isnt a player, find it.
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}
}
