using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
 
public class PJLoad : MonoBehaviour {

    public GameObject Player, ImagePlayer;

    public GameObject Fight, Table;
 
    public Sprite champ1, champ2, champ3, champ4; 
 
    public TextMeshProUGUI playerName, fightName;

    private void Start() {

        Player.transform.SetSiblingIndex(PlayerPrefs.GetInt("IDEnemy"));

        //Debug.Log(transform.GetSiblingIndex());


        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ1;
            playerName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ2;
            playerName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ3;
            playerName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ4;
            playerName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
        }
    }

    public void OpenFight()
    {
        if(Fight!=null && Table!=null)
        {
            Animator animator2 = Fight.GetComponent<Animator>();
            Animator animator3 = Table.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);

                if(PlayerPrefs.GetInt("IDEnemy")==7)
                {
                    fightName.GetComponent<TextMeshProUGUI>().text="El Calvo";
                }
            }
        }


    }


}