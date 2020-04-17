using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public TextMeshProUGUI ResultText;

    // Start is called before the first frame update
    void Start()
    {
        if (Results.GameOver) {
            if (Results.Win) {
                ResultText.text = "YOU WON!!!";
                ResultText.color = new Color(203f, 166f, 54f);
            }
            else {
                ResultText.text = "YOU LOOSE";
                ResultText.color = new Color(140f, 0, 0);
            }
        }
    }
}
