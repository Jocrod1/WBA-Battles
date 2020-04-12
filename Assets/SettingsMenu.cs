using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private int coins;
    private AudioSource audioSrc;

    private void Start() {

        audioSrc=GetComponent<AudioSource>();

        PlayerPrefs.SetInt("money", 1000);

        coins=PlayerPrefs.GetInt("money");



    }


    private void Update() {
        audioSrc.volume=PlayerPrefs.GetFloat("volume");

        print(PlayerPrefs.GetFloat("volume"));
    }

    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("volume",vol);
    }

}
