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

    public static bool FirstTime = true;

    public Slider MasterVolume, SFXVolume, MusicVolume;

    private void Start() {

        audioSrc=GetComponent<AudioSource>();

         //PlayerPrefs.SetInt("money", 1000);
        GlobalManager.Money = 1000;

        coins = GlobalManager.Money;

        float defValue = 10;

        if (FirstTime) {
            //Master Volume
            MasterVolume.value = defValue;
            MXr.SetFloat("MasterVol", defValue);
            PlayerPrefs.SetFloat("MasterVol", defValue);

            //SFX Volume
            SFXVolume.value = defValue;
            MXr.SetFloat("SFXVol", defValue);
            PlayerPrefs.SetFloat("SFXVol", defValue);

            //Music Volume
            MusicVolume.value = defValue;
            MXr.SetFloat("MusicVol", defValue);
            PlayerPrefs.SetFloat("SFXVol", defValue);


            //Cut Routine
            return;
        }

        //Master Volume
        float master = PlayerPrefs.GetFloat("MasterVol", defValue);
        if(master == defValue)
            PlayerPrefs.SetFloat("MasterVol", defValue);
        MasterVolume.value = master;
        MXr.SetFloat("MasterVol", master);



        //Sound FXs Volume
        float sfx = PlayerPrefs.GetFloat("SFXVol", defValue);
        if (sfx == defValue)
            PlayerPrefs.SetFloat("SFXVol", defValue);
        SFXVolume.value = sfx;
        MXr.SetFloat("SFXVol", sfx);



        //Music Volume
        float music = PlayerPrefs.GetFloat("MusicVol", defValue);
        if (music == defValue)
            PlayerPrefs.SetFloat("MusicVol", defValue);
        MusicVolume.value = music;
        MXr.SetFloat("MusicVol", music);

    }


    private void Update() {
        //audioSrc.volume = GlobalManager.SettingsData.Volume;
    }

    public void SetMasterVolume(float vol)
    {
        PlayerPrefs.SetFloat("MasterVol",vol);
        MXr.SetFloat("MasterVol", vol);
    }

    public void SetFXVolume(float vol)
    {
        PlayerPrefs.SetFloat("SFXVol",vol);
        MXr.SetFloat("SFXVol", vol);
    }
    public void SetMusicVolume(float vol)
    {
        PlayerPrefs.SetFloat("MusicVol",vol);
        MXr.SetFloat("MusicVol", vol);
    }

}
