using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip coinCollectClip;

    void avake()
    {
        if (audioSource == null) 
            audioSource = GetComponent<AudioSource>(); 
    }
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("no sounds!");
        }
    }
    
}
