using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;
 using System;
 using TMPro;
 using UnityEngine.SceneManagement;
 
 public class Counter : MonoBehaviour
 {
     public GameObject smoke, timeMenu;

    private DateTime currentDate, timeInPref;

    private string strTime, playerPrefTime;

    public TextMeshProUGUI date, btnFight;

    void Start()
     {

        date.GetComponent<TextMeshProUGUI>().text=System.DateTime.Now.ToString();

        playerPrefTime=PlayerPrefs.GetString("WaitOneHour", "-1");

        if(playerPrefTime!="-1")
        {
            timeInPref=DateTime.Parse(playerPrefTime);
        }

     }

     private void Update() {

        if(playerPrefTime!="-1")
        {
            btnFight.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);

            currentDate= System.DateTime.Now;

            strTime= (timeInPref-currentDate).ToString();
            strTime = strTime.Substring(3, (strTime).Length - 11);
            date.GetComponent<TextMeshProUGUI>().text=strTime;

            if(timeInPref<=currentDate)
            {
                playerPrefTime="-1";
                PlayerPrefs.SetString("WaitOneHour", "-1");
            }
        }
        else {

            //cuando no hay tiempo de espera
            btnFight.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);


            if(smoke!=null && timeMenu!=null)
            {
                Animator animator2 = smoke.GetComponent<Animator>();
                Animator animator3 = timeMenu.GetComponent<Animator>();

                if(animator2!=null && animator3!=null)
                {
                    if(animator2.GetBool("Open")==true && animator3.GetBool("Open")==true)
                    {
                        animator2.SetBool("Open", false);
                        animator3.SetBool("Open", false);
                    }

                }
            }
        }
         

     }

    public void playgame(string Level)
    {
        if(playerPrefTime!="-1")
        {
            if(smoke!=null && timeMenu!=null)
            {
                Animator animator2 = smoke.GetComponent<Animator>();
                Animator animator3 = timeMenu.GetComponent<Animator>();

                if(animator2!=null && animator3!=null)
                {
                    animator2.SetBool("Open", true);
                    animator3.SetBool("Open", true);
                }
            }
        }
        else {
            StartCoroutine(LoadYourAsyncScene(Level));
        }
    }

    public Animator BlackPanel;
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


    public void closeTimeMenu()
    {
        if(smoke!=null && timeMenu!=null)
        {
            Animator animator2 = smoke.GetComponent<Animator>();
            Animator animator3 = timeMenu.GetComponent<Animator>();
        
            if(animator2!=null && animator3!=null)
            {
                animator2.SetBool("Open", false);
                animator3.SetBool("Open", false);
            }
        }

    }


 
 
}