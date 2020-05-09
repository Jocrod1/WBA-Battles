using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    private int coins;
    private AudioSource audioSrc;

    public AudioMixer MXr;

    public Slider audioSlider;

    private void Start() {

        audioSrc=GetComponent<AudioSource>();

         //PlayerPrefs.SetInt("money", 1000);
        GlobalManager.Money = 1000;

        coins = GlobalManager.Money;

        audioSlider.value = GlobalManager.SettingsData.Volume;

    }


    private void Update() {
        //audioSrc.volume = GlobalManager.SettingsData.Volume;
    }

    public void SetVolume(float vol)
    {
         //PlayerPrefs.SetFloat("volume",vol);
        GlobalManager.SettingsData.Volume = vol;
    }

}
