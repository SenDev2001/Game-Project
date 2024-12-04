using UnityEngine;

public class E_Collectable : MonoBehaviour
{
    public int value = 1; // Score value of the coin
    private M_AudioManager audioManager;  // Reference to the AudioManager

    void Start()
    {
        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<M_AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add score if the ScoreManager is found
            M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(value);
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene.");
            }

            // Play the coin collect sound if AudioManager is available
            if (audioManager != null)
            {
                audioManager.PlaySound(audioManager.coinCollectClip);
            }

            // Destroy the coin object after collection
            Destroy(gameObject);
        }
    }
}
