using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : Manager {

    public bool IsGameOver = false;

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
    public Text PlayerTxt;
    public Text EnemyTxt;

    [Header("GlobalData")]
    public int IDPlayer;
    public int IDEnemy;

    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = false;
        Results.Win = false;
        //IDPlayer = GlobalManager.GameplayData.IDPlayer - 1;
        //IDEnemy = GlobalManager.GameplayData.IDEnemy - 1 ;

        int idp = PlayerPrefs.GetInt("IDPlayer", 0);
        int ide = PlayerPrefs.GetInt("IDEnemy", 0);

        IDPlayer = idp - 1;
        IDEnemy = 7 - ide;

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
    }

    // Update is called once per frame
    void Update()
    {

        AnimatorStateInfo stateInfo = HudManager.GameOverAnimator.GetCurrentAnimatorStateInfo(0);

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