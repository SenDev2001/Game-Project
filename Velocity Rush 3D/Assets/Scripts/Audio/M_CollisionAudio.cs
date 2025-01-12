using UnityEngine;

public class M_CollisionAudio : MonoBehaviour
{
    private M_AudioManager audioManager;

    void Start()
    {
        // Find the AudioManager in the scene
        audioManager = FindObjectOfType<M_AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    // This method is called when the player enters a trigger area (obstacle)
    private void OnTriggerEnter(Collider other)
    {
        // Debugging if trigger is detected
        Debug.Log("Player entered trigger: " + other.gameObject.name);

        // Check if the player enters a trigger area of an object tagged as "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            // Play the player collision sound using the AudioManager
            if (audioManager != null)
            {
                Debug.Log("Playing player collision sound.");
                audioManager.PlayPlayerCollisionSound();  // Play the hit sound
            }
        }
    }
}
