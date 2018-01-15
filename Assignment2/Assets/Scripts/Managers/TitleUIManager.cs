using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//##################### DEPRECATED #####################
public class TitleUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject InstructionsTab;
    [SerializeField]
    GameObject MenuTab;
    [SerializeField]
    GameObject vehicleSelect;


    public void ToggleInstructions()
    {   //For enabling/disabling the instructions tab.
        if(InstructionsTab.activeSelf)
        {
            InstructionsTab.SetActive(false);
            MenuTab.SetActive(true);
            vehicleSelect.SetActive(true);
        }
        else
        {
            InstructionsTab.SetActive(true);
            MenuTab.SetActive(false);
            vehicleSelect.SetActive(false);
        }
    }
}
