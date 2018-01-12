using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

//Enumerator for the game state. Outside of the class so it becomes global, usable in other scrips.
public enum EGameState { Menu, PreGame, Game, Cooldown, PostGame }

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public EGameState gameState = EGameState.PreGame;

    [Header("Game Managing Scripts")]
    [SerializeField] ScoreManager scoreMang;
    [SerializeField] TimerManager timerMang;
    public GameUIManager uiMang;
    public CoinManager coinMang;

    [Header("Other")]
    [SerializeField] VehicleSelector vehicleSelector;
    GameObject chosenVehicle;
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
    {   //Branch into a separate update functions depending on the games state.
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
    {   //Updating the game state on this, and other scripts which require the information.
        gameState = _gameState;
        timerMang.gameState = _gameState;
        scoreMang.gameState = _gameState;
        uiMang.gameState = _gameState;

        Debug.Log("[Game] Entering " + _gameState);
    }


    public void StartGame()
    {   //When we hit the 'Start' button on the title screen, load the main scene and start level 1.
        chosenVehicle = vehicleSelector.GetVehicle();
        SceneManager.LoadScene(1);
        //Add the delegate 'OnSceneLoaded' to receive and event once the scene has loaded sucessfully.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {   //Once our scene has loaded correctly, spawn the player, assign it to the camera, and begin the first level.
        SpawnPlayer();
        GameObject.FindObjectOfType<AutoCam>().m_Target = playerVehicle.transform;
        StartLevel();
    }

    void StartLevel()
    {   //############################## Start a level. ##############################
        currentLevel++;
        if (currentLevel <= 5)
        {   //If we are on level 1 - 5, continue.
            Debug.Log("[Game] Level: " + currentLevel);
            //Reset the player back to 0,0,0 for a new level.
            playerVehicle.transform.position = Vector3.zero;
            playerVehicle.transform.rotation = Quaternion.identity;
            UpdateGameState(EGameState.PreGame);
            coinMang.NewRound(currentLevel);
            timerMang.PreRoundCountdown();
        }
        else
        {   //We hit this block when the game is over. We must save the score and load the Post-Game Scene.
            UpdateGameState(EGameState.PostGame);
            Debug.Log("[Game] Game over.");
        }
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
        {   //If we were in the game, set the state to Cooldown.
            Debug.Log("[Game] Round over, entering Cooldown.");
            UpdateGameState(EGameState.Cooldown);
            uiMang.EndRound(currentLevel, scoreMang.score);
            scoreMang.EndRound(currentLevel);
            timerMang.CooldownCountdown();
        }
        else if(gameState == EGameState.Cooldown)
        {   //If the cooldown has ended, clear the current coins and start the next level (if applicable).
            Debug.Log("[Game] Cooldown over, entering next PreRound.");
            coinMang.ClearRound();
            StartLevel();
        }
    }


    void SpawnPlayer()
    {
        playerVehicle = Instantiate(chosenVehicle, Vector3.zero, Quaternion.identity);
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