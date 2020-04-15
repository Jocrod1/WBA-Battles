using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
 
public class PJLoad : MonoBehaviour {
 
    public Sprite champ1, champ2, champ3, champ4; 
 
    public TextMeshProUGUI playerName;
 
    private void Start() {

        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            GetComponent<Image>().sprite = champ1;
            playerName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            GetComponent<Image>().sprite = champ2;
            playerName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            GetComponent<Image>().sprite = champ3;
            playerName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            GetComponent<Image>().sprite = champ4;
            playerName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
        }

    }
}