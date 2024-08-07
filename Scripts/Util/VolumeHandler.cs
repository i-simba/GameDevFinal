using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] AudioMixer mixer;

    [Header("Sliders")]
    [SerializeField] Slider     music;
    [SerializeField] Slider     sfx;

    public void ChangeMusicVolume()
    {
        float vol = music.value;
        mixer.SetFloat( "MusicVol", Mathf.Log10( vol ) * 20 );
    }

    public void ChangeSFXVolume()
    {
        float vol = sfx.value;
        mixer.SetFloat( "SFXVol", Mathf.Log10( vol ) * 20 );
    }
}
