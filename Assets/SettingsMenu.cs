using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private int coins;
    private AudioSource audioSrc;

    public Slider audioSlider;

    private void Start() {

        audioSrc=GetComponent<AudioSource>();

        PlayerPrefs.SetInt("money", 1000);

        coins=PlayerPrefs.GetInt("money");

        audioSlider.value=PlayerPrefs.GetFloat("volume");

    }


    private void Update() {
        audioSrc.volume=PlayerPrefs.GetFloat("volume");
    }

    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume",vol);
    }

}
