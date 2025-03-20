using System;
using System.Collections;
using UnityEngine;

public class FeverBuff : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(FeverTime), 10f);
    }

    private void OnDisable()
    {
        _audio.pitch /= 2f;
        Time.timeScale /= 2f;
    }
    
    public IEnumerator FeverTime(float duration)
    {
        Time.timeScale *= 2f;
        _audio.pitch *= 2f;
        yield return new WaitForSeconds(duration);
        this.enabled = false;
    }
}
