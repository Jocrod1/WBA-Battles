using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    public bool IsGameOver = false;


    public Character Enemy;
    public Character Player;

    public HUDManager Manager;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Enemy.IsDefeated && !IsGameOver) {
            Player.Win();
            Manager.GameOver("Winner");
            IsGameOver = true;
        }
        if (Player.IsDefeated && !IsGameOver) {
            Enemy.Win();
            Manager.GameOver("Defeated");
            IsGameOver = true;
        }
    }
}
