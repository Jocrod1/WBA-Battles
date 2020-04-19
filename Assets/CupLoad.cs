using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupLoad : MonoBehaviour
{

    public GameObject subEnemy1, subEnemy2, subEnemy3, subPlayer, final, menu;

    void Start()
    {
        Animator animator = menu.GetComponent<Animator>();

        subEnemy1.SetActive(true);
        subEnemy2.SetActive(true);
        subEnemy3.SetActive(true);
        subPlayer.SetActive(true);
        final.SetActive(true);

        if(PlayerPrefs.GetInt("IDEnemy")==-1)
        {
            animator.SetInteger("Position", 0);

            subEnemy1.SetActive(false);
            subEnemy2.SetActive(false);
            subEnemy3.SetActive(false);
            subPlayer.SetActive(false);
            final.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==-2)
        {
            animator.SetInteger("Position", 1);

            final.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")==-3)
        {
            animator.SetInteger("Position", 2);
        }


    }

}
