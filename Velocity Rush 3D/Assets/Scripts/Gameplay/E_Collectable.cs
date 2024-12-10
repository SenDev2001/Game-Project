using UnityEngine;

public class E_Collectable : MonoBehaviour
{
    public int value = 1; 
    private M_AudioManager audioManager;  

    void Start()
    {
   
        audioManager = FindObjectOfType<M_AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AM not found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        return;


        M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("SM not found ");
            return;
        }

        scoreManager.AddScore(value);

        if (audioManager != null)
        {
            audioManager.PlaySound(audioManager.coinCollectClip);
        }

        Destroy(gameObject);
    }
}
