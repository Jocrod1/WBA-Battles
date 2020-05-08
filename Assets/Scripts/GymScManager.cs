using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GymScManager : MonoBehaviour {

    public SpriteRenderer Motivation, Player, FightPoster;

    public List<Sprite> Players;

    public List<Sprite> Fights;

    public List<Sprite> FamilyMotivation;
    public Sprite FameMotivation, MoneyMotivation;

    // Start is called before the first frame update
    void Start()
    {
        int IdP = PlayerPrefs.GetInt("IDPlayer");
        int IdM = PlayerPrefs.GetInt("IDMotivation");

        if (IdP == 0 || IdM == 0) {
            print("ID NULOOOOO");
            return;
        }

        int id = IdP - 1;

        Player.sprite = Players[id];

        FightPoster.sprite = Fights[id];

        if (IdM == 2)
        {
            Motivation.sprite = FamilyMotivation[id];
        }
        else if (IdM == 1)
        {
            Motivation.sprite = MoneyMotivation;
        }
        else if (IdM == 3) {
            Motivation.sprite = FameMotivation;
        }
    }

    public void LoadTable() {
        SceneManager.LoadScene("Table");
    }
}
