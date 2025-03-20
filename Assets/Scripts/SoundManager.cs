using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource _audioSource;
    public AudioClip bgm;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayBGM()
    {
        if (bgm != null)
        {
            _audioSource.clip = bgm;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}
