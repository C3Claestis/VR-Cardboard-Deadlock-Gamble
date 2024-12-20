using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("============ Component Script ============")]
    [SerializeField] StatsManager statsManager;
    [SerializeField] DiceRoll diceRollKiri;
    [SerializeField] DiceRoll diceRollKanan;
    [SerializeField] PlasticCup plasticCupKiri;
    [SerializeField] PlasticCup plasticCupKanan;

    [Header("============ Component UI ============")]
    [SerializeField] GameObject panelVotePlayer;
    [SerializeField] GameObject panelVoteHighLowPlayer;

    [SerializeField] GameObject panelVoteEnemy;
    [SerializeField] GameObject panelVoteHighLowEnemy;

    [SerializeField] Text playerVoteTxt;
    [SerializeField] Text enemyVoteTxt;

    [SerializeField] Text hasilVoteTxt;

    [SerializeField] GameObject equalsPlayerObj;
    [SerializeField] GameObject highPlayerObj;
    [SerializeField] GameObject lowPlayerObj;

    [SerializeField] GameObject equalsEnemyObj;
    [SerializeField] GameObject highEnemyObj;
    [SerializeField] GameObject lowEnemyObj;

    [SerializeField] Text correctPlayerTxt;
    [SerializeField] Text correctEnemyTxt;

    [SerializeField] Animator gunPlayer;
    [SerializeField] Animator gunEnemy;

    [SerializeField] GameObject panelGameOverI;
    [SerializeField] GameObject panelGameOverII;
    [SerializeField] Text winI;
    [SerializeField] Text winII;
    [SerializeField] private Color playerWinColor = Color.green; 
    [SerializeField] private Color enemyWinColor = Color.red;

    public static GameManager Instance;

    int hasilVoteIndex;
    int playerVoteIndex;
    int enemyVoteIndex;

    string playerHighLowIndex;
    string enemyHighLowIndex;
    Animator enemy;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StopGame()
    {
        StopAllCoroutines(); // Hentikan semua coroutine

        // Matikan UI atau berikan efek game over
        if (statsManager.GetHPPlayer() <= 0)
        {
            Debug.Log("Game Over! Enemy Wins.");
            enemy.SetBool("Clap", true);
            panelGameOverI.SetActive(true);
            panelGameOverII.SetActive(true);
            winI.text = "ENEMY WIN";
            winII.text = "ENEMY WIN";
            winI.color = enemyWinColor;
            winII.color = enemyWinColor;
        }
        else if (statsManager.GetHPEnemy() <= 0)
        {
            Debug.Log("Game Over! Player Wins.");
            enemy.SetBool("Lose", true);
            panelGameOverI.SetActive(true);
            panelGameOverII.SetActive(true);
            winI.text = "PLAYER WIN";
            winII.text = "PLAYER WIN";
            winI.color = playerWinColor;
            winII.color = playerWinColor;
        }
    }
    void Start()
    {
        StartCoroutine(StartGamble());
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Animator>();
    }

    IEnumerator StartGamble()
    {
        yield return new WaitForSeconds(3f);
        enemy.SetBool("Clap", false);
        enemy.SetBool("Laugh", false);
        enemy.SetBool("Dodge", false);

        plasticCupKanan.gameObject.SetActive(true);
        plasticCupKiri.gameObject.SetActive(true);

        diceRollKanan.gameObject.SetActive(true);
        diceRollKiri.gameObject.SetActive(true);

        plasticCupKanan.SetTutup(true);
        plasticCupKiri.SetTutup(true);

        StartCoroutine(diceRollKanan.RollDice());
        StartCoroutine(diceRollKiri.RollDice());

        StartCoroutine(ActivePanelVote());

        playerHighLowIndex = null;
        enemyHighLowIndex = null;

        hasilVoteIndex = 0;
        playerVoteIndex = 0;
        enemyVoteIndex = 0;

        hasilVoteTxt.text = "?";
        playerVoteTxt.text = "?";
        enemyVoteTxt.text = "?";

        correctPlayerTxt.text = "?";
        correctEnemyTxt.text = "?";

        equalsPlayerObj.SetActive(false);
        lowPlayerObj.SetActive(false);
        highPlayerObj.SetActive(false);

        equalsEnemyObj.SetActive(false);
        lowEnemyObj.SetActive(false);
        highEnemyObj.SetActive(false);
    }

    IEnumerator ActivePanelVote()
    {
        yield return new WaitForSeconds(5f);
        panelVotePlayer.SetActive(true);
        panelVoteEnemy.SetActive(true);

        StartCoroutine(EnemyVote());
        yield return null;
    }

    #region Player Function
    public void SetVotePlayer(int value)
    {
        playerVoteIndex = value;
        playerVoteTxt.text = playerVoteIndex.ToString();
        panelVotePlayer.SetActive(false);
        panelVoteHighLowPlayer.SetActive(true);
    }

    public void SetHighLowPlayer(string value)
    {
        playerHighLowIndex = value;

        if (playerHighLowIndex == "Equal")
        {
            equalsPlayerObj.SetActive(true);
        }
        else if (playerHighLowIndex == "High")
        {
            highPlayerObj.SetActive(true);
        }
        else
        {
            lowPlayerObj.SetActive(true);
        }

        panelVoteHighLowPlayer.SetActive(false);
        CheckPerhitungan();
    }
    #endregion

    #region Enemy Function
    IEnumerator EnemyVote()
    {
        yield return new WaitForSeconds(Random.Range(3, 10));
        enemyVoteIndex = Random.Range(1, 12);
        panelVoteEnemy.SetActive(false);
        enemyVoteTxt.text = enemyVoteIndex.ToString();
        panelVoteHighLowEnemy.SetActive(true);
        StartCoroutine(EnemyHighLow());
    }

    IEnumerator EnemyHighLow()
    {
        yield return new WaitForSeconds(Random.Range(3, 10));
        int valueHighLow = Random.Range(1, 3);

        if (valueHighLow == 1)
        {
            enemyHighLowIndex = "Low";
            lowEnemyObj.SetActive(true);
        }
        else if (valueHighLow == 2)
        {
            enemyHighLowIndex = "Equal";
            equalsEnemyObj.SetActive(true);
        }
        else
        {
            enemyHighLowIndex = "High";
            highEnemyObj.SetActive(true);
        }

        panelVoteHighLowEnemy.SetActive(false);
        CheckPerhitungan();
    }
    #endregion

    #region Judgement Gamble
    void CheckPerhitungan()
    {
        if (!string.IsNullOrEmpty(playerHighLowIndex) && !string.IsNullOrEmpty(enemyHighLowIndex) && playerVoteIndex > 0 && enemyVoteIndex > 0)
        {
            StartCoroutine(PutPlasticCup());
        }
    }

    IEnumerator PutPlasticCup()
    {
        plasticCupKiri.SetTutup(false);
        plasticCupKanan.SetTutup(false);
        yield return new WaitForSeconds(2f);

        CalculateFinalResult();
    }
    void CalculateFinalResult()
    {
        hasilVoteIndex = diceRollKiri.GetFinalSide() + diceRollKanan.GetFinalSide();
        hasilVoteTxt.text = hasilVoteIndex.ToString();

        Debug.Log("Player Vote: " + playerVoteIndex + ", Enemy Vote: " + enemyVoteIndex);
        Debug.Log("Final Dice Result: " + hasilVoteIndex);

        // Compare player vote
        string playerResult = CompareResult(playerVoteIndex, playerHighLowIndex);
        correctPlayerTxt.text = playerResult;
        Debug.Log("Player HighLow Result: " + playerResult);

        // Compare enemy vote
        string enemyResult = CompareResult(enemyVoteIndex, enemyHighLowIndex);
        correctEnemyTxt.text = enemyResult;
        Debug.Log("Enemy HighLow Result: " + enemyResult);

        if(correctPlayerTxt.text == "Correct" && correctEnemyTxt.text == "Incorrect")
        {
            PlayerShoot();
            StartCoroutine(ResetAgain(6f));
        }
        else if(correctPlayerTxt.text == "Incorrect" && correctEnemyTxt.text == "Correct")
        {
            EnemyShoot();
            StartCoroutine(ResetAgain(6f));
        }
        else
        {
            enemy.SetBool("Laugh", true);
            StartCoroutine(ResetAgain(3f));
        }
    }

    string CompareResult(int voteIndex, string highLowIndex)
    {
        //HASIL VOTE DIJADIKAN NILAI ACUAN >, <, =

        if ((highLowIndex == "High" && hasilVoteIndex > voteIndex) ||
            (highLowIndex == "Low" && hasilVoteIndex < voteIndex) ||
            (highLowIndex == "Equal" && hasilVoteIndex == voteIndex))
        {
            return "Correct";
        }
        return "Incorrect";
    }
    #endregion

    #region Shoot Function
    void PlayerShoot()
    {
        gunPlayer.SetBool("Shoot", true);
        enemy.SetBool("Dodge", true);
        StartCoroutine(statsManager.SetHPEnemy(5));
    }
    void EnemyShoot()
    {
        gunEnemy.SetBool("Shoot", true);
        enemy.SetBool("Clap", true);
        StartCoroutine(statsManager.SetHPPlayer(5));
    }
    IEnumerator ResetAgain(float value)
    {
        plasticCupKanan.gameObject.SetActive(false);
        plasticCupKiri.gameObject.SetActive(false);

        diceRollKanan.gameObject.SetActive(false);
        diceRollKiri.gameObject.SetActive(false);

        yield return new WaitForSeconds(value);
        gunEnemy.SetBool("Shoot", false);
        gunPlayer.SetBool("Shoot", false);

        plasticCupKanan.SetDefaultPos();
        plasticCupKiri.SetDefaultPos();

        diceRollKanan.transform.position = diceRollKanan.defaultPos;
        diceRollKiri.transform.position = diceRollKiri.defaultPos;

        StartCoroutine(StartGamble());
    }
    #endregion
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
