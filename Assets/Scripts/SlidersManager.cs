using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    void Start()
    {
        SetMusicSlider();
        SetSFXSlider();
    }

    public void SetMusicSlider()
    {
        float vol = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(vol) *20);
    }

    public void SetSFXSlider()
    {
        float vol = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(vol) *20);
    }
}
