using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    GameManager gm;


    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        ReturnToMenu();
    }


    public void SkipCredits()
    {
        gm.UpdateGameState(EGameState.Menu);
        SceneManager.LoadScene(0);
    }


    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(15);
        SkipCredits();
    }
}