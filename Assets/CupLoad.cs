using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CupLoad : MonoBehaviour
{
    private AudioSource audioSrc;

    public GameObject subEnemy1, subEnemy2, subEnemy3, subPlayer, final, menu, imagePlayer, imagePlayer2, imagePlayer3;

    public Sprite champ1, champ2, champ3, champ4;

    void Start()
    {
        Animator animator = menu.GetComponent<Animator>();

        audioSrc=GetComponent<AudioSource>();
        audioSrc.volume=PlayerPrefs.GetFloat("volume");


        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            imagePlayer.GetComponent<Image>().sprite = champ1;
            imagePlayer2.GetComponent<Image>().sprite = champ1;
            imagePlayer3.GetComponent<Image>().sprite = champ1;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            imagePlayer.GetComponent<Image>().sprite = champ2;
            imagePlayer2.GetComponent<Image>().sprite = champ2;
            imagePlayer3.GetComponent<Image>().sprite = champ2;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            imagePlayer.GetComponent<Image>().sprite = champ3;
            imagePlayer2.GetComponent<Image>().sprite = champ3;
            imagePlayer3.GetComponent<Image>().sprite = champ3;
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            imagePlayer.GetComponent<Image>().sprite = champ4;
            imagePlayer2.GetComponent<Image>().sprite = champ4;
            imagePlayer3.GetComponent<Image>().sprite = champ4;
        }

        subEnemy1.SetActive(true);
        subEnemy2.SetActive(true);
        subEnemy3.SetActive(true);
        subPlayer.SetActive(true);
        final.SetActive(true);

        if(PlayerPrefs.GetInt("IDEnemy")==10)
        {
            animator.SetInteger("Position", 0);

            subEnemy1.SetActive(false);
            subEnemy2.SetActive(false);
            subEnemy3.SetActive(false);
            subPlayer.SetActive(false);
            final.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==11)
        {
            animator.SetInteger("Position", 1);

            final.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==12)
        {
            animator.SetInteger("Position", 2);
        }


    }

}
