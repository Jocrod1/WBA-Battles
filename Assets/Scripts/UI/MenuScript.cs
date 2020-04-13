using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{
    public GameObject Smoke, Selection, ButtonSelection;

    public int IDChamp;

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        if(Smoke!=null && Selection!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Selection.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    //boton cuando se elija en boxeador
    public void SelectChamp()
    {

    }

    public void Champ1()
    {
        IDChamp=1;

        ButtonSelection.gameObject.SetActive(true);
    }

    public void Champ2()
    {
        IDChamp=2;

        ButtonSelection.gameObject.SetActive(true);
    }

    public void Champ3()
    {
        IDChamp=3;

        ButtonSelection.gameObject.SetActive(true);
    }

    public void Champ4()
    {
        IDChamp=4;

        ButtonSelection.gameObject.SetActive(true);
    }


}
