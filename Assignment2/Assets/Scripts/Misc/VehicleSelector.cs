using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VehicleSelector : MonoBehaviour
{
    public int carNo = 1;
    [SerializeField] Transform[] vehicles;
    Transform selectedCar;
    [SerializeField] GameObject[] vehiclePrefabs;


    public GameObject GetVehicle()
    {   //Returns the currently selected vehicle, as a the prefab.
        return vehiclePrefabs[carNo - 1];
    }


    public void NextVehicle(bool _forwards)
    {
        //Selecting the next/previous car, looping round.
        if (_forwards)
            carNo++;
        else
            carNo--;
        if (carNo > vehicles.Length)
            carNo = 1;
        else if (carNo < 1)
            carNo = vehicles.Length;
    }


    void Update()
    {
        if(selectedCar != vehicles[carNo - 1])
        {   //If the shown car is not what we are calling, update.
            if (selectedCar != null)
            {
                selectedCar.gameObject.SetActive(false);
            }
            selectedCar = vehicles[carNo - 1];
            vehicles[carNo - 1].gameObject.SetActive(true);
        }
    }
}