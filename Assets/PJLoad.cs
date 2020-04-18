using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
 
public class PJLoad : Manager {

    public GameObject Player, ImagePlayer;

    public GameObject Fight, Table;
 
    public Sprite champ1, champ2, champ3, champ4; 
 
    public TextMeshProUGUI playerName, fightName;

    private void Start() {

        Player.transform.SetSiblingIndex(GlobalManager.GameplayData.IDEnemy);

        //Debug.Log(transform.GetSiblingIndex());

        int IDPlayer = GlobalManager.GameplayData.IDPlayer;

        if (IDPlayer == 1)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ1;
            playerName.GetComponent<TextMeshProUGUI>().text = "Arlen Smith";
        }
        else if (IDPlayer == 2)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ2;
            playerName.GetComponent<TextMeshProUGUI>().text = "Daga Johar";
        }
        else if (IDPlayer == 3)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ3;
            playerName.GetComponent<TextMeshProUGUI>().text = "Irina Jones";
        }
        else if (IDPlayer == 4)
        {
            ImagePlayer.GetComponent<Image>().sprite = champ4;
            playerName.GetComponent<TextMeshProUGUI>().text = "Angenis Nadai";
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

                if(GlobalManager.GameplayData.IDEnemy == 7)
                {
                    fightName.GetComponent<TextMeshProUGUI>().text="El Calvo";
                }
            }
        }


    }


}