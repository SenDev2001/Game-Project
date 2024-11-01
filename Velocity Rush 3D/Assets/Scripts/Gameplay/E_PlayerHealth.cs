using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{

    public class E_PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Die();
            }
        }*/

        public void Die()
        {
            Debug.Log("Player Died");
            M_GameManager gameManager = FindObjectOfType<M_GameManager>();
            if (gameManager != null)
            {
                gameManager.ShowRestartUI();
            }
            else
            {
                Debug.LogError(" not found in the scene.");
            }
        }
        // obstacle script

        // triggerenter
        // if gameobject.comparetag(Player)
        // player.Die
    }
}
