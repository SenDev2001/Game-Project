using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AudioManager : MonoBehaviour

{
    public AudioSource playerCollisionSource;
    public AudioSource coinCollectSource;

    public AudioClip playerCollisionSound;
    public AudioClip coinCollectSound;

    // Play the player collision sound
    public void PlayPlayerCollisionSound()
    {
        if (playerCollisionSource != null && playerCollisionSound != null)
        {
            playerCollisionSource.PlayOneShot(playerCollisionSound);
        }
    }

    // Play the coin collect sound
    public void PlayCoinCollectSound()
    {
        if (coinCollectSource != null && coinCollectSound != null)
        {
            coinCollectSource.PlayOneShot(coinCollectSound);
        }
    }
}
