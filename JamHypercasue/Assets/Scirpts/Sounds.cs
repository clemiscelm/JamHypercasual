using System;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] AudioClip[] sounds;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
        
    }
    public void PlaySound(AudioClip clip = null)
    {
        
        if (clip == null)
        {
            audioSource.PlayOneShot(sounds[0]);
            Debug.LogError("Audio clip is null");
            return;
        }
        if (audioSource == null)
        {
            Debug.LogError("Audio source is null");
            return;
        }
    
    }
}
