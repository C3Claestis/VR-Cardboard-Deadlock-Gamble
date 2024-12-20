using System.Collections;
using UnityEngine;

public class DiceRoll : MonoBehaviour
{
    public Vector3 defaultPos;
    public float rollDuration = 2f;  // Durasi roll dalam detik
    public float rollSpeed = 500f;   // Kecepatan rotasi dadu
    private int finalSide;
    private bool isRolling = false;
    private Vector3 rollDirection;
    private float elapsedTime = 0f;

    private Quaternion targetRotation; // Untuk rotasi akhir dadu
    private bool shouldStopRolling = false; // Untuk menandakan bahwa roll selesai dan rotasi akhir perlu diterapkan

    public int GetFinalSide() => finalSide;

    void Update()
    {
        if (isRolling)
        {
            elapsedTime += Time.deltaTime;

            // Rotasi selama durasi roll
            transform.Rotate(rollDirection * Time.deltaTime);

            // Setelah durasi roll selesai, terapkan rotasi akhir
            if (elapsedTime >= rollDuration && !shouldStopRolling)
            {
                shouldStopRolling = true;
                ApplyFinalRotation(); // Langsung terapkan rotasi akhir setelah durasi roll selesai
            }
        }

        // Jika sudah selesai rolling, terapkan rotasi akhir
        if (shouldStopRolling)
        {
            transform.rotation = targetRotation; // Langsung terapkan rotasi akhir
            isRolling = false; // Berhenti sepenuhnya setelah mencapai rotasi akhir
        }
    }

    public IEnumerator RollDice()
    {
        isRolling = true;
        shouldStopRolling = false;
        elapsedTime = 0f;

        // Memberikan rotasi acak awal agar dadu tampak tidak terduga
        rollDirection = new Vector3(
            Random.Range(-rollSpeed, rollSpeed),
            Random.Range(-rollSpeed, rollSpeed),
            Random.Range(-rollSpeed, rollSpeed)
        );

        // Rotasi awal acak
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        yield return null;
    }

    void ApplyFinalRotation()
    {
        // Tentukan sisi dadu secara acak antara 1 hingga 6
        finalSide = Random.Range(1, 7); // Angka acak antara 1 dan 6

        // Sesuaikan rotasi berdasarkan sisi yang muncul
        switch (finalSide)
        {
            case 1:
                targetRotation = Quaternion.Euler(0, 0, 90);  // Angka 1
                break;
            case 2:
                targetRotation = Quaternion.Euler(0, 0, -90);  // Angka 2
                break;
            case 3:
                targetRotation = Quaternion.Euler(180, 0, 0);  // Angka 3
                break;
            case 4:
                targetRotation = Quaternion.Euler(0, 0, 0);  // Angka 4
                break;
            case 5:
                targetRotation = Quaternion.Euler(90, 0, 0);  // Angka 5
                break;
            case 6:
                targetRotation = Quaternion.Euler(-90, 0, 0);  // Angka 6
                break;
        }

        shouldStopRolling = true;
    }
}
