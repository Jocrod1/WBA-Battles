using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : Manager
{
    public GameObject Smoke, Selection, ButtonSelection, Menu, Motivation, Tutorial, Options;

    public Button btnContinue;

    public int IDChamp, IDMotivation;

    public Animator BlackPanel;

    private void Start() {
        //valor default
        PlayerPrefs.GetInt("IDEnemy", -1);

        if(PlayerPrefs.GetInt("IDEnemy")==-1)
        {
            btnContinue.interactable=false;
        }
    }

    public void playgame(string Level)
    {
        StartCoroutine(LoadYourAsyncScene(Level));
    }

    IEnumerator LoadYourAsyncScene(string level)
    {
        BlackPanel.SetTrigger("Out");

        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(level);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
            Animator animator3= Menu.GetComponent<Animator>();

            if(animator!=null && animator2!=null && animator3!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
            }
        }
    }

    public void CloseNewGame()
    {
        if(Smoke!=null && Selection!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Selection.GetComponent<Animator>();
            Animator animator3= Menu.GetComponent<Animator>();

            if(animator!=null && animator2!=null && animator3!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
                animator3.SetBool("Open", false);
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
        else if(PlayerPrefs.GetInt("IDEnemy")<=12 && PlayerPrefs.GetInt("IDEnemy")>=10)
        {
            string Table="Championship";
            playgame(Table);
        }
    }

    //boton cuando se elija en boxeador
    public void SelectChamp(string Tabla)
    {
        PlayerPrefs.SetInt("money", 1000);

        //GlobalManager.GameplayData.IDPlayer = IDChamp;
        PlayerPrefs.SetInt("IDPlayer", IDChamp);
        PlayerPrefs.SetInt("IDMotivation", IDMotivation);

        //como comienza un nuevo juego, el eemigo es el primero siempre (el numero 7)
        PlayerPrefs.SetInt("IDEnemy", 7);

        PlayerPrefs.SetString("WaitOneHour", "-1");

        Smoke.transform.SetSiblingIndex(0);

        playgame("Gym");
    }

    public void Motivation1(string Tabla)
    {
        IDMotivation=1;
        SelectChamp(Tabla);
    }

    public void Motivation2(string Tabla)
    {
        IDMotivation=2;
        SelectChamp(Tabla);
    }

    public void Motivation3(string Tabla)
    {
        IDMotivation=3;
        SelectChamp(Tabla);
    }

    public void Champ1()
    {
        IDChamp=1;

        if(Smoke!=null && Motivation!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Motivation.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);

                Smoke.transform.SetSiblingIndex(5);
            }
        }
    }

    public void Champ2()
    {
        IDChamp=2;

        if(Smoke!=null && Motivation!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Motivation.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);

                Smoke.transform.SetSiblingIndex(5);
            }
        }
    }

    public void Champ3()
    {
        IDChamp=3;

        if(Smoke!=null && Motivation!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Motivation.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);

                Smoke.transform.SetSiblingIndex(5);
            }
        }
    }

    public void Champ4()
    {
        IDChamp=4;

        if(Smoke!=null && Motivation!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Motivation.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);

                Smoke.transform.SetSiblingIndex(5);
            }
        }
    }

    public void ReturnMotivation()
    {
        IDChamp=0;

        if(Smoke!=null && Motivation!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Motivation.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);

                Smoke.transform.SetSiblingIndex(0);
            }
        }
    }


    public void OpenTutorial()
    {
        if(Tutorial!=null && Menu!=null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            Animator animator2 = Tutorial.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open2", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseTutorial()
    {
        if(Tutorial!=null && Menu!=null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            Animator animator2 = Tutorial.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open2", false);
                animator2.SetBool("Open", false);
            }
        }
    }

    public void OpenOptions()
    {
        if(Options!=null && Menu!=null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            Animator animator2 = Options.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open3", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseOptions()
    {
        if(Options!=null && Menu!=null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            Animator animator2 = Options.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open3", false);
                animator2.SetBool("Open", false);
            }
        }
    }


}
