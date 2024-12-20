using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject Panel_Stats;
    [SerializeField] GameObject Panel_Game;
    [SerializeField] GameObject Panel_Start;
    [SerializeField] GameObject Panel_Guide;
    [SerializeField] GameObject Panel_Credits;
    [SerializeField] GameObject enemyBody;
    [SerializeField] GameObject gunPlayer;
    [SerializeField] GameObject gunEnemy;
    
    public void StartGame()
    {
        gameManager.SetActive(true);
        Panel_Credits.SetActive(false);
        Panel_Game.SetActive(true);
        Panel_Guide.SetActive(false);
        Panel_Start.SetActive(false);
        Panel_Stats.SetActive(true);
        enemyBody.SetActive(true);
        gunEnemy.SetActive(true);
        gunPlayer.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
