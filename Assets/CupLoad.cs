﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CupLoad : MonoBehaviour
{
    private AudioSource audioSrc;

    //pantallas
    public GameObject smoke, smoke2, fight, diagram, wall, realShop, tutorial;

    public GameObject subEnemy1, subEnemy2, subEnemy3, subPlayer, final, menu, imagePlayer, imagePlayer2, imageAnimPlayer1, imageAnimPlayer2, imageAnimEnemy1, imageAnimEnemy2, imageAnimFinal, eliminateds1, eliminateds2, vs;

    public Sprite champ1, champ2, champ3, champ4, player1, player2, player3, player4;

    public Sprite enemyCartel1,enemyCartel2,enemyCartel3;

    public GameObject cartelPlayer, cartelEnemy, finalImage;

    public TextMeshProUGUI playerName, enemyName, title, finalName;

    void Start()
    {
        Animator animator = menu.GetComponent<Animator>();

        audioSrc=GetComponent<AudioSource>();
        audioSrc.volume=PlayerPrefs.GetFloat("volume");


        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            imagePlayer.GetComponent<Image>().sprite = champ1;
            imagePlayer2.GetComponent<Image>().sprite = champ1;

            cartelPlayer.GetComponent<Image>().sprite = player1;
            playerName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";

            finalName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
            finalImage.GetComponent<Image>().sprite = player1;

            imageAnimPlayer1.GetComponent<Image>().sprite = champ1;
            imageAnimPlayer2.GetComponent<Image>().sprite = champ1;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            imagePlayer.GetComponent<Image>().sprite = champ2;
            imagePlayer2.GetComponent<Image>().sprite = champ2;

            cartelPlayer.GetComponent<Image>().sprite = player2;
            playerName.GetComponent<TextMeshProUGUI>().text="Daga Johar";

            finalName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
            finalImage.GetComponent<Image>().sprite = player2;

            imageAnimPlayer1.GetComponent<Image>().sprite = champ2;
            imageAnimPlayer2.GetComponent<Image>().sprite = champ2;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            imagePlayer.GetComponent<Image>().sprite = champ3;
            imagePlayer2.GetComponent<Image>().sprite = champ3;

            cartelPlayer.GetComponent<Image>().sprite = player3;
            playerName.GetComponent<TextMeshProUGUI>().text="Irina Jones";

            finalName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
            finalImage.GetComponent<Image>().sprite = player3;

            imageAnimPlayer1.GetComponent<Image>().sprite = champ3;
            imageAnimPlayer2.GetComponent<Image>().sprite = champ3;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            imagePlayer.GetComponent<Image>().sprite = champ4;
            imagePlayer2.GetComponent<Image>().sprite = champ4;

            cartelPlayer.GetComponent<Image>().sprite = player4;
            playerName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";

            finalName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
            finalImage.GetComponent<Image>().sprite = player4;

            imageAnimPlayer1.GetComponent<Image>().sprite = champ4;
            imageAnimPlayer2.GetComponent<Image>().sprite = champ4;
        }

        subEnemy1.SetActive(true);
        subEnemy2.SetActive(true);
        subEnemy3.SetActive(true);
        subPlayer.SetActive(true);
        final.SetActive(true);

        if(PlayerPrefs.GetInt("IDEnemy")==12)
        {

            imageAnimPlayer1.SetActive(true);
            imageAnimEnemy1.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text="Quarter final";
            
            enemyName.GetComponent<TextMeshProUGUI>().text="Miguel Ruiz";
            cartelEnemy.GetComponent<Image>().sprite = enemyCartel1;

            subEnemy1.SetActive(false);
            subEnemy2.SetActive(false);
            subEnemy3.SetActive(false);
            subPlayer.SetActive(false);
            final.SetActive(false);
            vs.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==11)
        {

            imageAnimPlayer2.SetActive(true);
            imageAnimEnemy2.SetActive(true);
            
            title.GetComponent<TextMeshProUGUI>().text="Semifinal";

            enemyName.GetComponent<TextMeshProUGUI>().text="Alex Duran";
            cartelEnemy.GetComponent<Image>().sprite = enemyCartel2;

            eliminateds1.SetActive(true);

            final.SetActive(false);
            vs.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==10)
        {
            imageAnimFinal.SetActive(true);

            enemyName.GetComponent<TextMeshProUGUI>().text="Korona";
            cartelEnemy.GetComponent<Image>().sprite = enemyCartel3;
            title.GetComponent<TextMeshProUGUI>().text="Final";

            eliminateds1.SetActive(true);
            eliminateds2.SetActive(true);
            vs.SetActive(false);
        }


    }

    public void OpenFight()
    {
        if(fight!=null && diagram!=null)
        {
            Animator animator = wall.GetComponent<Animator>();
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = diagram.GetComponent<Animator>();
            Animator animator4 = smoke.GetComponent<Animator>();
            Animator animator5 = smoke2.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
                animator4.SetBool("Open", false);
                animator5.SetBool("Open", false);
            }
        }
    }

    public void CloseFight()
    {
        if(fight!=null && diagram!=null)
        {
            Animator animator = wall.GetComponent<Animator>();
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = diagram.GetComponent<Animator>();
            Animator animator4 = smoke.GetComponent<Animator>();
            Animator animator5 = smoke2.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
                animator3.SetBool("Open", false);
                animator4.SetBool("Open", true);
                animator5.SetBool("Open", true);
            }
        }
    }

    public void OpenShop()
    {
        if(smoke2!=null && realShop!=null)
        {
            Animator animator = smoke2.GetComponent<Animator>();
            Animator animator2 = realShop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                smoke2.transform.SetSiblingIndex(5);

                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseShop()
    {
        if(smoke!=null && realShop!=null)
        {
            Animator animator = smoke2.GetComponent<Animator>();
            Animator animator2 = realShop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                smoke2.transform.SetSiblingIndex(0);

                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }

    public void returnMenu(string Level)
    {
        SceneManager.LoadScene(Level);
    }

    public void OpenTutorial()
    {
        if(tutorial!=null && fight!=null)
        {
            Animator animator = fight.GetComponent<Animator>();
            Animator animator2 = tutorial.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open2", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseTutorial()
    {
        if(tutorial!=null && fight!=null)
        {
            Animator animator = fight.GetComponent<Animator>();
            Animator animator2 = tutorial.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open2", false);
                animator2.SetBool("Open", false);
            }
        }
    }

}
