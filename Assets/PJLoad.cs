using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
 
public class PJLoad : MonoBehaviour {

    private AudioSource audioSrc;

    public GameObject playerTable, imagePlayer, cartelPlayer, cartelEnemy;

    public GameObject fight, table;
 
    public Sprite champ1, champ2, champ3, champ4;

    public Sprite enemy1,enemy2,enemy3,enemy4,enemy5,enemy6,enemy7, player1,player2,player3,player4; 
 
    public TextMeshProUGUI playerName, fightName, cartelName;

    public TextMeshProUGUI[] positions, names;

    private void Start() {

        audioSrc=GetComponent<AudioSource>();
        audioSrc.volume=PlayerPrefs.GetFloat("volume");

        //positions= new float[position1, position2, position3, position4, position5, position6, position7, position8];


        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            imagePlayer.GetComponent<Image>().sprite = champ1;
            cartelPlayer.GetComponent<Image>().sprite = player1;
            playerName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
            cartelName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            imagePlayer.GetComponent<Image>().sprite = champ2;
            cartelPlayer.GetComponent<Image>().sprite = player2;
            playerName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
            cartelName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            imagePlayer.GetComponent<Image>().sprite = champ3;
            cartelPlayer.GetComponent<Image>().sprite = player3;
            playerName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
            cartelName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            imagePlayer.GetComponent<Image>().sprite = champ4;
            cartelPlayer.GetComponent<Image>().sprite = player4;
            playerName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
            cartelName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
        }

        //positions
        positions[0].GetComponent<TextMeshProUGUI>().text="1";
        positions[1].GetComponent<TextMeshProUGUI>().text="2";
        positions[2].GetComponent<TextMeshProUGUI>().text="3";
        positions[3].GetComponent<TextMeshProUGUI>().text="4";
        positions[4].GetComponent<TextMeshProUGUI>().text="5";
        positions[5].GetComponent<TextMeshProUGUI>().text="6";
        positions[6].GetComponent<TextMeshProUGUI>().text="7";
        positions[7].GetComponent<TextMeshProUGUI>().text="8";


        playerTable.transform.SetSiblingIndex(PlayerPrefs.GetInt("IDEnemy"));
        //Debug.Log(transform.GetSiblingIndex());

        if(PlayerPrefs.GetInt("IDEnemy")==7)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Dante Gray";
            cartelEnemy.GetComponent<Image>().sprite = enemy1;
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==6)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Kwan Lee";
            cartelEnemy.GetComponent<Image>().sprite = enemy2;

            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="7";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==5)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Black Dwayne";
            cartelEnemy.GetComponent<Image>().sprite = enemy3;

            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="6";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==4)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Miguel Ruiz";
            cartelEnemy.GetComponent<Image>().sprite = enemy4;

            positions[4].GetComponent<TextMeshProUGUI>().text="6";
            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="5";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[4].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==3)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Moicano Blue";
            cartelEnemy.GetComponent<Image>().sprite = enemy5;

            positions[3].GetComponent<TextMeshProUGUI>().text="5";
            positions[4].GetComponent<TextMeshProUGUI>().text="6";
            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="4";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[4].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[3].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==2)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Ray Rocker";
            cartelEnemy.GetComponent<Image>().sprite = enemy6;

            positions[2].GetComponent<TextMeshProUGUI>().text="4";
            positions[3].GetComponent<TextMeshProUGUI>().text="5";
            positions[4].GetComponent<TextMeshProUGUI>().text="6";
            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="3";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[4].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[3].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[2].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==1)
        {
            fightName.GetComponent<TextMeshProUGUI>().text="Korona";
            cartelEnemy.GetComponent<Image>().sprite = enemy7;

            positions[1].GetComponent<TextMeshProUGUI>().text="3";
            positions[2].GetComponent<TextMeshProUGUI>().text="4";
            positions[3].GetComponent<TextMeshProUGUI>().text="5";
            positions[4].GetComponent<TextMeshProUGUI>().text="6";
            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="2";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[4].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[3].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[2].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[1].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==0)
        {
            positions[0].GetComponent<TextMeshProUGUI>().text="2";
            positions[1].GetComponent<TextMeshProUGUI>().text="3";
            positions[2].GetComponent<TextMeshProUGUI>().text="4";
            positions[3].GetComponent<TextMeshProUGUI>().text="5";
            positions[4].GetComponent<TextMeshProUGUI>().text="6";
            positions[5].GetComponent<TextMeshProUGUI>().text="7";
            positions[6].GetComponent<TextMeshProUGUI>().text="8";
            positions[7].GetComponent<TextMeshProUGUI>().text="1";

            names[6].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[5].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[4].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[3].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[2].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[1].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
            names[0].GetComponent<TextMeshProUGUI>().color= new Color(1f,1f,1f,0.5f);
        }
    }

    public void OpenFight()
    {
        if(fight!=null && table!=null)
        {
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = table.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
            }
        }


    }


}