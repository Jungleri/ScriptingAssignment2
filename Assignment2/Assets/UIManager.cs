using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EGameState { Menu, PreGame, Game, Cooldown, PostGame }

public class UIManager : MonoBehaviour
{
    [SerializeField] Text timerText;
    float timeRemaining = ;


	void Start ()
    {

	}
	

	void FixedUpdate ()
    {
        timerText.text = 

    }

    UpdateTimeRemaining(float _timeRemaining)
    {

    }

}
