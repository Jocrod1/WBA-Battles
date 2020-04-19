using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : Manager
{
    public GameObject Smoke, Selection, ButtonSelection;

    public int IDChamp;

    private void Start() {
        PlayerPrefs.GetInt("IDEnemy", -1);
    }

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

    public void ContinueGame()
    {
        if(PlayerPrefs.GetInt("IDEnemy")<=7 && PlayerPrefs.GetInt("IDEnemy")>=0)
        {
            string Table="Table";
            playgame(Table);
        }
        else if(PlayerPrefs.GetInt("IDEnemy")<=-1 && PlayerPrefs.GetInt("IDEnemy")>=-4)
        {
            string Table="Championship";
            playgame(Table);
        }
    }

    //boton cuando se elija en boxeador
    public void SelectChamp(string Tabla)
    {
        //GlobalManager.GameplayData.IDPlayer = IDChamp;
        PlayerPrefs.SetInt("IDPlayer", IDChamp);

        //como comienza un nuevo juego, el eemigo es el primero siempre (el numero 7)
        PlayerPrefs.SetInt("IDEnemy", 7);

        playgame(Tabla);
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
