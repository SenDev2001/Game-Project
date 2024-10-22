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

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player Died");
            M_GameManager.Instance.ShowRestartUI();
        }
    }
}
