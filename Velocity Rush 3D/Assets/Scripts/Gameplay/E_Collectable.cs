using UnityEngine;

public class E_Collectable : MonoBehaviour
{
    public int value = 1; 
    private M_AudioManager audioManager;
    private M_ScoreManager scoreManager;

    void Start()
    {
        audioManager = FindAnyObjectByType<M_AudioManager>();
        scoreManager = FindAnyObjectByType<M_ScoreManager>();
   
        audioManager = FindObjectOfType<M_AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AM not found.");
        }
        if (scoreManager == null)
        {
            Debug.LogError("SM not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        return;

        if (scoreManager != null)
        {
            scoreManager.AddScore(value);
        }

        if (audioManager != null)
        {
            audioManager.PlayCoinCollectSound();
        }

        Destroy(gameObject);
    }
}
