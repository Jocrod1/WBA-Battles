using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    public TextMeshProUGUI ResultText;

    private int idEnemy;

    // Start is called before the first frame update
    void Start()
    {
        if (Results.GameOver) {

            idEnemy= PlayerPrefs.GetInt("IDEnemy");

            if (Results.Win) {
                ResultText.text = "YOU WON!!!";
                ResultText.color = new Color(203f, 166f, 54f);

                if(idEnemy>=10 && idEnemy<=12)
                {
                    idEnemy=idEnemy+1;
                    PlayerPrefs.SetInt("IDEnemy", idEnemy);
                }
                else if(idEnemy==0)
                {
                    //PlayerPrefs.SetInt("IDEnemy", 10);
                }
                else if(idEnemy>=1 && idEnemy<=7)
                {
                    idEnemy=idEnemy-1;
                    PlayerPrefs.SetInt("IDEnemy", idEnemy);
                }
            }
            else {
                ResultText.text = "YOU LOOSE";
                ResultText.color = new Color(140f, 0, 0);
            }
        }

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
