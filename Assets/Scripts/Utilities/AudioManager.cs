using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip gameMusic;
    public AudioClip diamondSfx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (gameMusic == null || musicSource == null) return;

        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayDiamondSound()
    {
        if (diamondSfx == null || sfxSource == null) return;
        sfxSource.PlayOneShot(diamondSfx);
    }
    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    public void RestartMusic()
    {
        if (musicSource == null || gameMusic == null) return;

        musicSource.Stop();
        musicSource.clip = gameMusic;
        musicSource.time = 0f;   // baþa sar
        musicSource.loop = true;
        musicSource.Play();
    }

}


