using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Collectable : MonoBehaviour
{
    public int value = 1; // Score value of the coin

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            M_ScoreManager.Instance.AddScore(value);
            Destroy(gameObject); // Remove coin from the scene
        }
    }
}
