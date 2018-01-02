using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleUIManager : MonoBehaviour
{

    [SerializeField] VehicleSelector vehicleSelector;
    [SerializeField] GameManager gm;

	public void StartGame()
    {
        gm.playerVehicle = vehicleSelector.GetVehicle();
        SceneManager.LoadScene(1);
        gm.StartGame();
    }

}
