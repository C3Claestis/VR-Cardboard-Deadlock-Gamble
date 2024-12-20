using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField] Image barPlayer;
    [SerializeField] Image barEnemy;

    [SerializeField] Sprite[] barPlayersprite = new Sprite[5]; // Pastikan array memiliki 5 elemen
    [SerializeField] Sprite[] barEnemysprite = new Sprite[5];

    private int hpPlayer = 5;
    private int hpEnemy = 5;

    public int GetHPPlayer() => hpPlayer;
    public int GetHPEnemy() => hpEnemy;

    public IEnumerator SetHPPlayer(int value)
    {
        yield return new WaitForSeconds(3f);
        hpPlayer -= value;

        // Pastikan hpPlayer tetap dalam rentang valid (0 hingga 4)
        hpPlayer = Mathf.Clamp(hpPlayer, 0, barPlayersprite.Length - 1);

        // Atur sprite berdasarkan hpPlayer
        barPlayer.sprite = barPlayersprite[hpPlayer];

        // Tambahkan logika lain jika diperlukan, seperti game over
        if (hpPlayer == 0)
        {
            Debug.Log("Player HP habis!");
            GameManager.Instance.StopGame();
        }
    }

    public IEnumerator SetHPEnemy(int value)
    {
        yield return new WaitForSeconds(3f);
        hpEnemy -= value;

        // Pastikan hpEnemy tetap dalam rentang valid (0 hingga 4)
        hpEnemy = Mathf.Clamp(hpEnemy, 0, barEnemysprite.Length - 1);

        // Atur sprite berdasarkan hpEnemy
        barEnemy.sprite = barEnemysprite[hpEnemy];

        // Tambahkan logika lain jika diperlukan, seperti game over
        if (hpEnemy == 0)
        {
            Debug.Log("Enemy HP habis!");
            GameManager.Instance.StopGame();
        }
    }
}
