using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomClipSource : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        _audioSource.Stop();
        _audioSource.clip = clips[Random.Range(0, clips.Length)];
        _audioSource.Play();
    }
}
