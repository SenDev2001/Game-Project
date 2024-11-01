using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E_Collectable : MonoBehaviour
{
    public int value = 1; // Score value of the coin

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            M_ScoreManager scoreManager = FindObjectOfType<M_ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(value);
            }
            else
            {
                Debug.LogError("ScoreManager not found in the scene.");
            }
            Destroy(gameObject); // Remove coin from the scene
        }
    }
}
