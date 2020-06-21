using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CupLoad : MonoBehaviour
{
    private AudioSource audioSrc;

    //pantallas
    [Header("Scenes")]
    public GameObject smoke;
    public GameObject smoke2;
    public GameObject fight;
    public GameObject menu;
    public GameObject wall;
    public GameObject realShop;
    public GameObject tutorial;


    [Header("Cartel Object")]
    public GameObject cartelPlayer;
    public GameObject cartelEnemy;
    public GameObject finalImage;


    [Header("Semifinal Object")]
    public GameObject[] imgSemifinal;

    [Header("Final Object")]
    public GameObject imgFinal;

    [Header("Player Image")]
    public GameObject[] imagePlayer;

    [Header("Animations")]
    public GameObject imageAnimPlayer1;
    public GameObject imageAnimPlayer2;
    public GameObject imageAnimEnemy1;
    public GameObject imageAnimEnemy2;
    public GameObject imageAnimFinal;
    public GameObject vs;
    public GameObject eliminateds1;
    public GameObject eliminateds2;

    [Header("Poster Image")]
    public Sprite[] posterImage;


    [Header("Face Image")]
    public Sprite[] playerCartel;
    public Sprite[] enemyCartel;
    public TextMeshProUGUI playerName, enemyName;

    [Header("Final Text")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI finalName;


    void Start()
    {
        Animator animator = menu.GetComponent<Animator>();

        audioSrc=GetComponent<AudioSource>();
        audioSrc.volume=PlayerPrefs.GetFloat("volume");


        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            imagePlayer[0].GetComponent<Image>().sprite = posterImage[0];
            imagePlayer[1].GetComponent<Image>().sprite = posterImage[0];

            cartelPlayer.GetComponent<Image>().sprite = playerCartel[0];
            playerName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";

            finalName.GetComponent<TextMeshProUGUI>().text="Arlen Smith";
            finalImage.GetComponent<Image>().sprite = playerCartel[0];

            imageAnimPlayer1.GetComponent<Image>().sprite = posterImage[0];
            imageAnimPlayer2.GetComponent<Image>().sprite = posterImage[0];
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            imagePlayer[0].GetComponent<Image>().sprite = posterImage[1];
            imagePlayer[1].GetComponent<Image>().sprite = posterImage[1];

            cartelPlayer.GetComponent<Image>().sprite = playerCartel[1];
            playerName.GetComponent<TextMeshProUGUI>().text="Daga Johar";

            finalName.GetComponent<TextMeshProUGUI>().text="Daga Johar";
            finalImage.GetComponent<Image>().sprite = playerCartel[1];

            imageAnimPlayer1.GetComponent<Image>().sprite = posterImage[1];
            imageAnimPlayer2.GetComponent<Image>().sprite = posterImage[1];
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            imagePlayer[0].GetComponent<Image>().sprite = posterImage[2];
            imagePlayer[1].GetComponent<Image>().sprite = posterImage[2];

            cartelPlayer.GetComponent<Image>().sprite = playerCartel[2];
            playerName.GetComponent<TextMeshProUGUI>().text="Irina Jones";

            finalName.GetComponent<TextMeshProUGUI>().text="Irina Jones";
            finalImage.GetComponent<Image>().sprite = playerCartel[2];

            imageAnimPlayer1.GetComponent<Image>().sprite = posterImage[2];
            imageAnimPlayer2.GetComponent<Image>().sprite = posterImage[2];
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            imagePlayer[0].GetComponent<Image>().sprite = posterImage[3];
            imagePlayer[1].GetComponent<Image>().sprite = posterImage[3];

            cartelPlayer.GetComponent<Image>().sprite = playerCartel[3];
            playerName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";

            finalName.GetComponent<TextMeshProUGUI>().text="Angenis Nadai";
            finalImage.GetComponent<Image>().sprite = playerCartel[3];

            imageAnimPlayer1.GetComponent<Image>().sprite = posterImage[3];
            imageAnimPlayer2.GetComponent<Image>().sprite = posterImage[3];
        }

        imgSemifinal[0].SetActive(true);
        imgSemifinal[1].SetActive(true);
        imgSemifinal[2].SetActive(true);
        imgSemifinal[3].SetActive(true);
        imgFinal.SetActive(true);

        if(PlayerPrefs.GetInt("IDEnemy")==12)
        {

            imageAnimPlayer1.SetActive(true);
            imageAnimEnemy1.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text="Quarter final";
            
            enemyName.GetComponent<TextMeshProUGUI>().text="Miguel Ruiz";
            cartelEnemy.GetComponent<Image>().sprite = enemyCartel[0];

            imgSemifinal[0].SetActive(false);
            imgSemifinal[1].SetActive(false);
            imgSemifinal[2].SetActive(false);
            imgSemifinal[3].SetActive(false);
            imgFinal.SetActive(false);
            vs.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==11)
        {

            imageAnimPlayer2.SetActive(true);
            imageAnimEnemy2.SetActive(true);
            
            title.GetComponent<TextMeshProUGUI>().text="Semifinal";

            enemyName.GetComponent<TextMeshProUGUI>().text="Alex Duran";
            cartelEnemy.GetComponent<Image>().sprite =  enemyCartel[1];

            eliminateds1.SetActive(true);

            imgFinal.SetActive(false);
            vs.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==10)
        {
            imageAnimFinal.SetActive(true);

            enemyName.GetComponent<TextMeshProUGUI>().text="Korona";
            cartelEnemy.GetComponent<Image>().sprite =  enemyCartel[2];
            title.GetComponent<TextMeshProUGUI>().text="Final";

            eliminateds1.SetActive(true);
            eliminateds2.SetActive(true);
            vs.SetActive(false);
        }


    }

    public void OpenFight()
    {
        if(fight!=null && menu!=null)
        {
            Animator animator = wall.GetComponent<Animator>();
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = menu.GetComponent<Animator>();
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
        if(fight!=null && menu!=null)
        {
            Animator animator = wall.GetComponent<Animator>();
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = menu.GetComponent<Animator>();
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
