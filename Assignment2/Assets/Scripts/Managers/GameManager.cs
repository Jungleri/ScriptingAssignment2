using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enumerator for the game state. Outside of the class so it becomes global, usable in other scrips.
public enum EGameState { Menu, PreGame, Game, Cooldown, PostGame }

public class GameManager : MonoBehaviour
{
    public EGameState gameState = EGameState.PreGame;
    [SerializeField] ScoreManager scoreMang;
    [SerializeField] TimerManager timerMang;
    [SerializeField] GameUIManager uiMang;
    [SerializeField] VehicleSelector vehicleSelector;

    public GameObject playerVehicle;

    int currentLevel = 0;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(!scoreMang || !timerMang)
        {   //Incase the all the required scene compnenty aren't present, log it out.
            Debug.Log("[Error] Manager Scripts Are Not Assigned In Level: " + SceneManager.GetActiveScene().ToString());
        }
    }


    void Start ()
    {   //Sets the game state to menu.
        gameState = EGameState.Menu;
	}


    private void Update()
    {   //Branch into a separate function depending on the games state.
        if(gameState == EGameState.Game)
        {
            GameUpdateLoop();
        }
        else if(gameState == EGameState.PreGame)
        {
            PreGameUpdateLoop();
        }
    }


    void UpdateGameState(EGameState _gameState)
    {   //Updating the game state on this, and other managing scripts.
        gameState = _gameState;
        timerMang.gameState = _gameState;
        scoreMang.gameState = _gameState;
    }


    public void StartGame()
    {   //When we hit the 'Start' button on the title screen, load the main scene and start level 1.
        playerVehicle = vehicleSelector.GetVehicle();
        SceneManager.LoadScene(1);
        StartLevel(1);
    }


    void StartLevel(int difficulty)
    {   //############################## Start a level. ##############################
        Debug.Log("Level: " + currentLevel);
        currentLevel++;
        timerMang.PreRoundCountdown();
    }


    public void TimerComplete()
    {   //When the gametimer runs down, update the game state.
        if (gameState == EGameState.PreGame)
        {   //If we were in the pregame, set the state to game.
            Debug.Log("[Game] PreRound over, entering Game.");
            UpdateGameState(EGameState.Game);
            timerMang.RoundCountdown();
        }
        else if(gameState == EGameState.Game)
        {   //If we were in the game, set the state to postgame.
            Debug.Log("[Game] Game over, entering PostGame.");
            UpdateGameState(EGameState.PostGame);
        }
    }


    void GameUpdateLoop()
    {
        if (uiMang)
        {   //Update the UI with our current statistics.
            uiMang.UpdateScore(scoreMang.score);
            uiMang.UpdateTimeRemaining(timerMang.timeRemaining);
            uiMang.UpdateScoreMultiplier(scoreMang.scoreMultiplier);
        }
        else if (!uiMang)
        {   //Find the UI Manager
            uiMang = GameObject.FindObjectOfType<GameUIManager>();
        }

    }


    void PreGameUpdateLoop()
    {
        if (uiMang)
        {   //Update the UI with out current statistics.
            uiMang.UpdateTimeRemaining(timerMang.timeRemaining);
        }
        else if (!uiMang)
        {   //Find the UI Manager
            uiMang = GameObject.FindObjectOfType<GameUIManager>();
        }
    }
}
