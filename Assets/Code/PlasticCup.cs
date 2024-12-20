using System.Collections;
using UnityEngine;

public class PlasticCup : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPos; // Posisi awal
    [SerializeField] private Vector3 menutup; // Posisi tertutup
    [SerializeField] private Vector3 membuka; // Posisi terbuka
    
    [SerializeField] private float durasi = 2f; // Durasi pergerakan dalam detik
    
    private bool isTutup = false;
    private Coroutine currentCoroutine = null;

    public void SetDefaultPos()
    {
        transform.position = defaultPos;
    }
    public void SetTutup(bool tutup)
    {
        isTutup = tutup;

        // Jika sudah ada coroutine, biarkan berjalan sampai selesai, kecuali target berubah
        if (currentCoroutine != null && IsTargetReached(isTutup ? menutup : membuka))
            return;

        // Mulai coroutine baru
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(MoveToTarget(isTutup ? menutup : membuka));
    }

    private IEnumerator MoveToTarget(Vector3 targetPos)
    {
        while (!IsTargetReached(targetPos))
        {
            // Gerakkan objek menuju target secara perlahan
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos,
                (Vector3.Distance(menutup, membuka) / durasi) * Time.deltaTime);
            yield return null; // Tunggu frame berikutnya
        }

        // Pastikan objek berada tepat di posisi target
        transform.localPosition = targetPos;

        // Reset coroutine setelah selesai
        currentCoroutine = null;
    }

    private bool IsTargetReached(Vector3 targetPos)
    {
        return Vector3.Distance(transform.localPosition, targetPos) < 0.01f;
    }
}
