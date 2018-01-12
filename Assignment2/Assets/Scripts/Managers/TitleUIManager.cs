using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//##################### DEPRECATED #####################
public class TitleUIManager : MonoBehaviour
{

    [SerializeField] VehicleSelector vehicleSelector;
    [SerializeField] GameManager gm;

	public void StartGame()
    {   //When this is called, give the selected vehicle to the GameManager, and load the game scene.
        gm.playerVehicle = vehicleSelector.GetVehicle();
        SceneManager.LoadScene(1);
        gm.StartGame();
    }

}
