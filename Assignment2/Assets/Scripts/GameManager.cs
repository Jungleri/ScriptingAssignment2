using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EGameState { Menu, PreGame, Game, Cooldown, PostGame }

public class GameManager : MonoBehaviour
{
    EGameState gameState;
    [SerializeField] ScoreManager scoreMang;
    [SerializeField] TimerManager timerMang;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }


    void Start ()
    {
        gameState = EGameState.Cooldown;
	}
	

	void Update ()
    {
		
	}

    void Update
}
