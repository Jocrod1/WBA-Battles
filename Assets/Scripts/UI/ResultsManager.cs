using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    [Serializable]
    public class MotivationPanel {
        public List<Sprite> Motivation;
    }

    [Serializable]
    public class PlayersPanels {
        public MotivationPanel ArlenSmithMP;
        public MotivationPanel DagaJoharMP;
        public MotivationPanel IrinaJonesMP;
        public MotivationPanel AngelisNadaiMP;
    }

    public GameObject imagePanel;

    public TextMeshProUGUI ResultText;

    private DateTime oneHour, timeNextFight;
    private TimeSpan duration;

    private int idEnemy;

    [Header("Panel List")]
    public List<MotivationPanel> playersLoosePanels;
    public List<MotivationPanel> playersWinPanels;

    public Sprite[] winChamp1, winChamp2, winChamp3, winChamp4, loseChamp1, loseChamp2,loseChamp3, loseChamp4;

    private bool winZone;
    private string Table;


    public enum Result {
        Win,
        Defeat
    }

    // Start is called before the first frame update
    void Start()
    {
        Results.GameOver = false;
        idEnemy = PlayerPrefs.GetInt("IDEnemy");
        PlayerPrefs.SetInt("WinZoneInLose", 0);

        //agregar una hora al tiempo en que termino la pelea
        oneHour = System.DateTime.Now;
        duration = new System.TimeSpan(0, 0, 0, 20);
        timeNextFight = oneHour.Add(duration);

        if (Results.Win)
        {
            ResultText.text = "YOU WON";
            ResultText.color = new Color(203f, 166f, 54f);

            SetPanel(true);

            if (idEnemy >= 10 && idEnemy <= 12)
            {
                idEnemy = idEnemy - 1;
                PlayerPrefs.SetInt("IDEnemy", idEnemy);
                PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());
            }
            else if (idEnemy == 0)
            {
                //PlayerPrefs.SetInt("IDEnemy", 10);
            }
            else if (idEnemy >= 1 && idEnemy <= 7)
            {
                idEnemy = idEnemy - 1;
                PlayerPrefs.SetInt("IDEnemy", idEnemy);
            }

            if (idEnemy >= 1 && idEnemy <= 3)
            {
                PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());
            }


            //BACKGROUND
            
                if(PlayerPrefs.GetInt("IDPlayer")==1)
                    {
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp1[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp1[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp1[2];
                        }

                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==2)
                    {   
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp2[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp2[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp2[2];
                        }
                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==3)
                    {
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp3[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp3[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp3[2];
                        }
                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==4)
                    { 
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp4[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp4[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = winChamp4[2];
                        }
                    }


        }
        else
        {
            ResultText.text = "YOU LOSE";
            ResultText.color = new Color(140f, 0, 0);

            //PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());

            SetPanel(false);

            
            if (idEnemy >= 1 && idEnemy <= 3)
            {
                PlayerPrefs.SetInt("WinZoneInLose", 1);
                PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());
            }
            else {
                PlayerPrefs.SetInt("WinZoneInLose", 0);
                PlayerPrefs.SetInt("IDEnemy", 7);
            }





            //BACKGROUND
                if(PlayerPrefs.GetInt("IDPlayer")==1)
                    {
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp1[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp1[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp1[2];
                        }

                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==2)
                    {   
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp2[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp2[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp2[2];
                        }
                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==3)
                    {
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp3[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp3[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp3[2];
                        }
                    }
                    else if(PlayerPrefs.GetInt("IDPlayer")==4)
                    { 
                        if(PlayerPrefs.GetInt("IDMotivation")==1)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp4[0];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==2)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp4[1];
                        }
                        else if(PlayerPrefs.GetInt("IDMotivation")==3)
                        {
                            imagePanel.GetComponent<Image>().sprite = loseChamp4[2];
                        }
                    }
        }

        PlayerPrefs.SetInt("health", 0);
        PlayerPrefs.SetInt("stamina", 0);
        PlayerPrefs.SetInt("damage", 0);


    }

    public int Idp;
    public int idm;

    public void SetPanel(bool win) {
        int IdPlayer = PlayerPrefs.GetInt("IDPlayer", 0);
        int IdMotivation = PlayerPrefs.GetInt("IDMotivation", 0);

        Idp = IdPlayer - 1;
        idm = IdMotivation - 1;

        if (IdMotivation == 0 || IdPlayer == 0) {
            print("ERROR: Id nulo");
            return;
        }

        IdPlayer--;
        IdMotivation--;
    }

    public void goTable()
    {
        if (Results.Win)
        {
            if(idEnemy>=0 && idEnemy<=7)
            {
                Table="Table";
            }
            else if(idEnemy>=10 && idEnemy<=12)
            {
                Table="Championship";
            }
            else if(idEnemy==9)
            {
                Table="Victory";  
            }
            playgame(Table);
        }
        else
        {
            if(PlayerPrefs.GetInt("WinZoneInLose")==1)
            {
                string Table="Table";
                playgame(Table);
            }
            else
            {
                PlayerPrefs.SetInt("IDEnemy", -1);
                string Table="Menu";
                playgame(Table);
            }
        }

    }

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
