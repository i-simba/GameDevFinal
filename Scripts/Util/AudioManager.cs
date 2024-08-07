using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager manager;

    [Header("Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("MISC")]
    [SerializeField] public AudioClip   music;
    [SerializeField] public AudioClip   click;
    [SerializeField] public AudioClip   bell;
    [SerializeField] public AudioClip   coin;
    [SerializeField] public AudioClip   wood;

    [Header("Player")]
    [SerializeField] public AudioClip   death;
    [SerializeField] public AudioClip   scream1;
    [SerializeField] public AudioClip   scream2;
    [SerializeField] public AudioClip   scream3;

    [Header("Gun")]
    [SerializeField] public AudioClip   gun;
    [SerializeField] public AudioClip   reload;

    [Header("Zombie")]
    [SerializeField] public AudioClip   thump;
    [SerializeField] public AudioClip   zombie;
    [SerializeField] public AudioClip   zhurt;
    [SerializeField] public AudioClip   zattack;

    private Dictionary<int, AudioClip>  screams;

    void Awake()
    {
        if ( manager == null )
            manager = this;
        else
            Destroy( this );

        screams = new Dictionary<int, AudioClip> {
            { 1, scream1 },
            { 2, scream2 },
            { 3, scream3 }
        };
    }

    void Start()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void Click()
    {
        sfxSource.PlayOneShot( click );
    }

    public void PlaySFX( AudioClip sfx )
    {
        sfxSource.PlayOneShot( sfx );
    }

    public void PlayScream( int rand )
    {
        sfxSource.PlayOneShot( screams[rand] );
    }
}
