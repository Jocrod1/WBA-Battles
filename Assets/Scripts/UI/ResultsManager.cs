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

    public Image ImagePanel;

    public TextMeshProUGUI ResultText;

    private DateTime oneHour, timeNextFight;
    private TimeSpan duration;

    private int idEnemy;

    [Header("Panel List")]
    public List<MotivationPanel> playersLoosePanels;
    public List<MotivationPanel> playersWinPanels;

    public enum Result {
        Win,
        Defeat
    }

    // Start is called before the first frame update
    void Start()
    {
        Results.GameOver = false;
        idEnemy = PlayerPrefs.GetInt("IDEnemy");

        //agregar una hora al tiempo en que termino la pelea
        oneHour = System.DateTime.Now;
        duration = new System.TimeSpan(0, 0, 0, 60);
        timeNextFight = oneHour.Add(duration);

        if (Results.Win)
        {
            ResultText.text = "YOU WON!!!";
            ResultText.color = new Color(203f, 166f, 54f);

            PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());

            SetPanel(true);

            if (idEnemy >= 10 && idEnemy <= 12)
            {
                idEnemy = idEnemy - 1;
                PlayerPrefs.SetInt("IDEnemy", idEnemy);
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
        }
        else
        {
            ResultText.text = "YOU LOOSE";
            ResultText.color = new Color(140f, 0, 0);

            PlayerPrefs.SetString("WaitOneHour", timeNextFight.ToString());

            SetPanel(false);

            idEnemy = 7;
            PlayerPrefs.SetInt("IDEnemy", idEnemy);
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

        if (!win)
            ImagePanel.sprite = playersLoosePanels[Idp].Motivation[idm];
        else
            ImagePanel.sprite = playersWinPanels[Idp].Motivation[idm];
    }

    public void goTable()
    {
        if(idEnemy>=0 && idEnemy<=7)
        {
            string Table="Table";
            playgame(Table);
        }
        else if(idEnemy>=10 && idEnemy<=13)
        {
            string Table="Championship";
            playgame(Table);
        }

    }

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
