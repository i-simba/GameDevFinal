using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public static MainMenuAudio manager;

    [Header("Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Clip")]
    [SerializeField] public AudioClip   music;
    [SerializeField] public AudioClip   click;

    void Awake()
    {
        if ( manager == null )
            manager = this;
        else
            Destroy( this );
    }

    void Start()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX( AudioClip sfx )
    {
        sfxSource.PlayOneShot( sfx );
    }
}
