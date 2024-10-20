using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class E_Obstacle : MonoBehaviour

    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Call the player's die method
                other.GetComponent<E_PlayerHealth>().Die();
            }
        }
    }

}

