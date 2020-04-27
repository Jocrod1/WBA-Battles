using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameplayManager : Manager {

    public bool IsGameOver = false;
    [Header("Round Management")]
    public int NumersOfRounds;
    public int CurrentRound = 1;
    //public Animator Bars;
    //public Animator Spawners;

    [Header("Prefabs Lists")]
    public List<GameObject> Players;
    public List<GameObject> Enemies;

    [Header("Selected Prefabs")]
    public GameObject SelectedPlayer;
    public GameObject SelectedEnemy;

    GameObject RealPlayer;
    GameObject RealEnemy;

    [Header("Spawners")]
    public Transform PlayerSpawner;
    public Transform EnemySpawner;

    [Header("Components")]
    public Enemy enemy;
    public PlayerScript Player;

    public HUDManager HudManager;
    public TextMeshProUGUI PlayerTxt;
    public TextMeshProUGUI EnemyTxt;

    public Transform Ring;

    [Header("GlobalData")]
    public int IDPlayer;
    public int IDEnemy;

    [Header("Timer")]
    public TextMeshProUGUI TimerTxt;
    public float TimeCount;
    public float Timer;
    public bool ActivateTimer;
    float StartTime;

    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = false;
        Results.Win = false;
        //IDPlayer = GlobalManager.GameplayData.IDPlayer - 1;
        //IDEnemy = GlobalManager.GameplayData.IDEnemy - 1 ;

        //int idp = PlayerPrefs.GetInt("IDPlayer", 0);
        //int ide = PlayerPrefs.GetInt("IDEnemy", 0);

        //IDPlayer = idp - 1;
        //IDEnemy = 7 - ide;

        Vector2 scr = new Vector2(Screen.width, Screen.height);

        float dispersion = scr.x / scr.y;

        if (dispersion > 2.1f) {
            Ring.localScale = new Vector3(1.26225f, 1.791563f, 1);
        }


        if (IDPlayer < 0 || IDEnemy < 0)
            return;


        SelectedPlayer = Players[IDPlayer];
        SelectedEnemy = Enemies[IDEnemy];

        RealPlayer = Instantiate(SelectedPlayer, PlayerSpawner);
        RealPlayer.transform.localPosition = Vector3.zero;
        RealEnemy = Instantiate(SelectedEnemy, EnemySpawner);
        RealEnemy.transform.localPosition = Vector3.zero;

        Player = RealPlayer.GetComponent<PlayerScript>();
        enemy = RealEnemy.GetComponent<Enemy>();

        enemy.Player = Player;

        HudManager.PlayerBars.Player = Player;
        HudManager.EnemyBars.Player = enemy;

        PlayerTxt.text = Player.Name;
        EnemyTxt.text = enemy.Name;

        Player.enabled = false;
        enemy.enabled = false;

        float initialtime = TimeCount * 60;

        TimeSpan span = new TimeSpan(0, 0, Mathf.RoundToInt(initialtime));

        string seconds = "";
        if (span.Seconds < 10)
            seconds += "0";

        seconds += span.Seconds.ToString();

        TimerTxt.text = span.Minutes + ":" + seconds;
    }

    float timing = 0;

    public void RoundEnded() {
        timing = 0;
        DisableCharacters();

        

        CurrentRound++;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.GameIsPaused)
            return;

        AnimatorStateInfo stateInfo = HudManager.GameOverAnimator.GetCurrentAnimatorStateInfo(0);
        if (ActivateTimer) {
            timing += Time.deltaTime;
            Timer = (TimeCount * 60) - timing;

            TimeSpan span = new TimeSpan(0, 0, Mathf.RoundToInt(Timer));

            string seconds = "";
            if (span.Seconds < 10)
                seconds += "0";

            seconds += span.Seconds.ToString();

            TimerTxt.text = span.Minutes + ":" + seconds;
        }

        if(Timer <= 0) {
            RoundEnded();
        }

        if (enemy.IsDefeated && !IsGameOver) {
            Player.Win();
            HudManager.GameOver("Winner");
            IsGameOver = true;
            Results.Win = true;
        }
        if (Player.IsDefeated && !IsGameOver) {
            enemy.Win();
            HudManager.GameOver("Defeated");
            IsGameOver = true;
            Results.Win = false;
        }

        if ((stateInfo.IsName("WinnerF") || stateInfo.IsName("DefeatedF")) && !Results.GameOver) {
            Results.GameOver = true;
            loadResultPanel();
        }
    }
    public void loadResultPanel()
    {
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene());
    }

    public void EnableCharacters() {
        Player.enabled = true;
        enemy.enabled = true;
        ActivateTimer = true;
    }

    public void DisableCharacters() {
        Player.enabled = false;
        enemy.enabled = false;
        ActivateTimer = false;
    }

    IEnumerator LoadYourAsyncScene()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Results");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}


public static class Results {
    private static bool gameOver;
    private static bool win;

    public static bool Win { get => win; set => win = value; }
    public static bool GameOver { get => gameOver; set => gameOver = value; }
}