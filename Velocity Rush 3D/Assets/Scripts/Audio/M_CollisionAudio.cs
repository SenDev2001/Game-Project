using System.Runtime.CompilerServices;
using UnityEngine;

public class M_CollisionAudio : MonoBehaviour
{
    private M_AudioManager audioManager;

    void Start()
    {
        InitializeAudioManager();
    }

    private void InitializeAudioManager()
    {
        audioManager = FindObjectOfType<M_AudioManager>();

        // Check if the AudioManager is found
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    // This method is called when a collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        HandleTriggerCollision(other);
    }

    // Handle the collision with a trigger
    private void HandleTriggerCollision(Collider other)
    {
        LogTriggerEntry(other);

        // If the other object is tagged as "Obstacle", play the collision sound
        if (other.CompareTag("Obstacle"))
        {
            PlayCollisionSound();
        }
    }

    // Log the trigger entry
    private void LogTriggerEntry(Collider other)
    {
        Debug.Log("Player entered trigger: " + other.gameObject.name);
    }

    // Play the player collision sound if audioManager is set
    private void PlayCollisionSound()
    {
        if (audioManager != null)
        {
            Debug.Log("Playing player collision sound.");
            audioManager.PlayPlayerCollisionSound();
        }
    }
}
