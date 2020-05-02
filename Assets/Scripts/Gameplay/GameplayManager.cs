using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;



public class GameplayManager : Manager {

    [System.Serializable]
    public class RoundResults {
        public List<TextMeshProUGUI> ResultJudges;
    }
    [System.Serializable]
    public class JudgeResult {
        public int Player;
        public int Enemy;

        public JudgeResult(int player, int enemy)
        {
            Player = player;
            Enemy = enemy;
        }
    }

    public class ResultsbyRound {
        public JudgeResult Judge1;
        public JudgeResult Judge2;
        public JudgeResult Judge3;

        public ResultsbyRound(JudgeResult judge1, JudgeResult judge2, JudgeResult judge3)
        {
            Judge1 = judge1;
            Judge2 = judge2;
            Judge3 = judge3;
        }
    }

    public bool IsGameOver = false;
    public static bool Wait;
    [Header("Round Management")]
    public int NumersOfRounds;
    public int CurrentRound = 1;
    public Animator Bars;
    public Animator Spawners;
    public Animator ResultsPoster;
    public Button ContinueButton;
    public float WaitSecForKO;


    [Header("Results Management")]
    public List<RoundResults> listResultsUI;
    public List<TextMeshProUGUI> FinalResults = new List<TextMeshProUGUI>();
    public List<ResultsbyRound> ResultsTable = new List<ResultsbyRound>();
    public ResultsbyRound FinalResultsTable;
    public float TimeWaitingAnim;
    public TextMeshProUGUI FinalResult;
    public Color WinColor;
    public Color DefeatColor;
    public float Speed;

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

        int idp = PlayerPrefs.GetInt("IDPlayer", 0);
        int ide = PlayerPrefs.GetInt("IDEnemy", 0);

        IDPlayer = idp - 1;
        IDEnemy = 7 - ide;

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

        Player.MaxKnockouts = 3;
        enemy.MaxKnockouts = 3;

        enemy.Player = Player;

        HudManager.PlayerBars.Player = Player;
        HudManager.EnemyBars.Player = enemy;

        PlayerTxt.text = Player.Name;
        EnemyTxt.text = enemy.Name;

        Player.Wait = true;
        enemy.enabled = false;

        float initialtime = TimeCount * 60;

        TimeSpan span = new TimeSpan(0, 0, Mathf.RoundToInt(initialtime));

        string seconds = "";
        if (span.Seconds < 10)
            seconds += "0";

        seconds += span.Seconds.ToString();

        TimerTxt.text = span.Minutes + ":" + seconds;
    }

    public void SetTable() {
        float PlyrHealthPerc = (Player.CurrentHealth / Player.MaxHealth);
        float EnmyHealthPerc = (enemy.CurrentHealth / enemy.MaxHealth);

        int plyrKOs = Player.CurrentKnockouts;
        int EnmyKOs = enemy.CurrentKnockouts;

        float PlyrRate = PlyrHealthPerc + plyrKOs;
        float EnmyRate = EnmyHealthPerc + EnmyKOs;

        float Diff = PlyrRate - EnmyRate;

        int PlyrPointsBase = 0;
        int EnmyPointBase = 0;

        int points = 0;

        if(Math.Abs(Diff) <= 0.5f) {
            points = Mathf.RoundToInt(UnityEngine.Random.value);
        }

        switch (EnmyKOs) {
            case 0: {
                    PlyrPointsBase = 8;
                    break;
            }
            case 1: {
                    PlyrPointsBase = 9;
                    break;
                }
            case 2: {
                    PlyrPointsBase = 10;
                    break;
                }
            default: break;
        }


        switch (plyrKOs) {
            case 0:
                {
                    EnmyPointBase = 8;
                    break;
                }
            case 1:
                {
                    EnmyPointBase = 9;
                    break;
                }
            case 2:
                {
                    EnmyPointBase = 10;
                    break;
                }
            default: break;
        }

        JudgeResult jdge1 = new JudgeResult(PlyrPointsBase, EnmyPointBase);
        JudgeResult jdge2 = new JudgeResult(PlyrPointsBase, EnmyPointBase);
        JudgeResult jdge3 = new JudgeResult(PlyrPointsBase, EnmyPointBase);

        if (Mathf.Abs(Diff) <= 0.2f && plyrKOs == EnmyKOs)
        {
            if (Diff > 0)
            {
                jdge1.Enemy -= addPoints(0.333f, 1);
                jdge2.Enemy -= addPoints(0.333f, 1);
                jdge3.Enemy -= addPoints(0.333f, 1);
            }
            else
            {
                jdge1.Player -= addPoints(0.333f, 1);
                jdge2.Player -= addPoints(0.333f, 1);
                jdge3.Player -= addPoints(0.333f, 1);
            }
        }
        else if (Mathf.Abs(Diff) <= 0.5f && plyrKOs == EnmyKOs)
        {
            if (Diff > 0)
            {
                jdge3.Enemy -= addPoints(0.333f, 1);
            }
            else
            {
                jdge3.Player -= addPoints(0.333f, 1);
            }
        }
        else if(plyrKOs == EnmyKOs) {
            if (Diff > 0)
            {
                jdge1.Enemy -= 1;
                jdge2.Enemy -= 1;
                jdge3.Enemy -= 1;
            }
            else
            {
                jdge1.Player -= 1;
                jdge2.Player -= 1;
                jdge3.Player -= 1;
            }
        }

        ResultsTable.Add(new ResultsbyRound(jdge1, jdge2, jdge3));
        JudgeResult finalResult1 = new JudgeResult(0, 0);
        JudgeResult finalResult2 = new JudgeResult(0, 0);
        JudgeResult finalResult3 = new JudgeResult(0, 0);
        foreach (ResultsbyRound item in ResultsTable) {
            JudgeResult J1 = item.Judge1;
            JudgeResult J2 = item.Judge2;
            JudgeResult J3 = item.Judge3;


            finalResult1 = new JudgeResult(finalResult1.Player + J1.Player, finalResult1.Enemy + J1.Enemy);
            finalResult2 = new JudgeResult(finalResult2.Player + J2.Player, finalResult2.Enemy + J2.Enemy);
            finalResult3 = new JudgeResult(finalResult3.Player + J3.Player, finalResult3.Enemy + J3.Enemy);
        }
        float count = (float)ResultsTable.Count;
        finalResult1 = new JudgeResult(Rounding(finalResult1.Player / count), Rounding(finalResult1.Enemy / count));
        finalResult2 = new JudgeResult(Rounding(finalResult2.Player / count), Rounding(finalResult2.Enemy / count));
        finalResult3 = new JudgeResult(Rounding(finalResult3.Player / count), Rounding(finalResult3.Enemy / count));
        FinalResultsTable = new ResultsbyRound(finalResult1, finalResult2, finalResult3);
        CleanTableUI();
        FillTableUI();
    }

    int Rounding(float value) {
        return (int)Math.Round((decimal)value, MidpointRounding.AwayFromZero);
    }

    public void CleanTableUI() {
        foreach(RoundResults item in listResultsUI) {
            foreach (TextMeshProUGUI Txts in item.ResultJudges) {
                Txts.text = "";
            }
        }

        foreach (TextMeshProUGUI Item in FinalResults) {
            Item.text = "";
        }
    }

    public void FillTableUI() {
        for (int i = 0; i < ResultsTable.Count; i++) {

            string result1 = ResultsTable[i].Judge1.Player + " - " + ResultsTable[i].Judge1.Enemy;
            string result2 = ResultsTable[i].Judge2.Player + " - " + ResultsTable[i].Judge2.Enemy;
            string result3 = ResultsTable[i].Judge3.Player + " - " + ResultsTable[i].Judge3.Enemy;

            listResultsUI[i].ResultJudges[0].text = result1;
            listResultsUI[i].ResultJudges[1].text = result2;
            listResultsUI[i].ResultJudges[2].text = result3;
        }
        string finalResult1 = FinalResultsTable.Judge1.Player + " - " + FinalResultsTable.Judge1.Enemy;
        string finalResult2 = FinalResultsTable.Judge2.Player + " - " + FinalResultsTable.Judge2.Enemy;
        string finalResult3 = FinalResultsTable.Judge3.Player + " - " + FinalResultsTable.Judge3.Enemy;
        FinalResults[0].text = finalResult1;
        FinalResults[1].text = finalResult2;
        FinalResults[2].text = finalResult3;
        FinalResult.text = "";
    }

    int addPoints(float prob, int value) {
        float rdn = UnityEngine.Random.value;

        if (rdn < prob)
        {
            return value;
        }

        return 0;
    }

    float timing = 0;

    public void RoundEnded() {
        timing = 0;
        Timer = (TimeCount * 60) - timing;
        SetTable();
        DisableCharacters();

        enemy.CurrentKnockouts = 0;
        Player.CurrentKnockouts = 0;

        enemy.Knocking();
        Player.Knocking();

        Bars.SetBool("Inside", false);
        Spawners.SetBool("Inside", false);

        StartCoroutine(LoadResults());
    }
    
    IEnumerator LoadResults() {
        yield return new WaitForSeconds(TimeWaitingAnim);

        Player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);

        ResultsPoster.gameObject.SetActive(true);
        ResultsPoster.SetBool("Inside", true);

        yield return new WaitForSeconds(1.1f);

        if (CurrentRound >= NumersOfRounds) {
            string Sentence = "";
            JudgeResult J1 = FinalResultsTable.Judge1;
            JudgeResult J2 = FinalResultsTable.Judge2;
            JudgeResult J3 = FinalResultsTable.Judge3;

            int PlayerScore = J1.Player + J2.Player + J3.Player;
            int EnemyScore = J1.Enemy + J2.Enemy + J3.Enemy;

            if (PlayerScore > EnemyScore)
            {
                Sentence = "VICTORY";
                FinalResult.color = WinColor;
                Results.Win = true;
            }
            else {
                Sentence = "DEFEAT";
                FinalResult.color = DefeatColor;
                Results.Win = false;
            }
            
            //Aqui debo determinar si gana o pierde el jugador
            //inserte codigo aqui
            //F comitada

            int i = 0;
            foreach (char letter in Sentence.ToCharArray())
            {
                FinalResult.text += letter;
                yield return new WaitForSeconds(Speed);
            }
        }

        ContinueButton.gameObject.SetActive(true);
    }

    public void Continue() {
        if (CurrentRound < NumersOfRounds) {
            CurrentRound++;
            StartCoroutine(NextRound());
        }
        else {
            Results.GameOver = true;
            loadResultPanel();
        }
        
    }

    IEnumerator NextRound() {
        ContinueButton.gameObject.SetActive(false);
        ResultsPoster.SetBool("Inside", false);

        Player.gameObject.SetActive(true);
        enemy.gameObject.SetActive(true);

        Player.CurrentHealth = Player.MaxHealth;
        Player.CurrentStamina = Player.MaxStamina;
        enemy.CurrentHealth = enemy.MaxHealth;
        enemy.CurrentStamina = enemy.MaxStamina;

        Player.unwait();
        enemy.unwait();

        float initialtime = TimeCount * 60;

        TimeSpan span = new TimeSpan(0, 0, Mathf.RoundToInt(initialtime));

        string seconds = "";
        if (span.Seconds < 10)
            seconds += "0";

        seconds += span.Seconds.ToString();

        TimerTxt.text = span.Minutes + ":" + seconds;

        yield return new WaitForSeconds(1.2f);

        Bars.SetBool("Inside", true);
        Spawners.SetBool("Inside", true);

        yield return new WaitForSeconds(TimeWaitingAnim);

        EnableCharacters();
    }



    public void PlayerKO() {
        DisableCharacters();
        Bars.SetBool("Inside", false);
        Spawners.SetBool("PlayerKO", true);
        StartCoroutine(CharacterKOd(true));
    }

    public void EnemyKO() {
        DisableCharacters();
        Bars.SetBool("Inside", false);
        Spawners.SetBool("EnemyKO", true);
        StartCoroutine(CharacterKOd(false));
    }

    IEnumerator CharacterKOd(bool Pl) {
        //while (!(Player.stateinfo.Waiting && enemy.stateinfo.Waiting)) {
        //    yield return null;
        //}
        yield return new WaitForSeconds(3);

        if (Pl)
            Player.RestoreHealth();
        else
            enemy.RestoreHealth();

        enemy.RestoreStamina();
        Player.RestoreStamina();

        Bars.SetBool("Inside", true);
        Spawners.SetBool("PlayerKO", false);
        Spawners.SetBool("EnemyKO", false);

        yield return new WaitForSeconds(TimeWaitingAnim);

        EnableCharacters();
        Player.unwait();
        enemy.unwait();
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

        if(Timer <= 0 && ActivateTimer) {
            RoundEnded();
        }

        if (enemy.KnockedOut && !IsGameOver) {
            enemy.KnockedOut = false;
            Player.Knocking();
            EnemyKO();

        }
        if (Player.KnockedOut && !IsGameOver) {
            Player.KnockedOut = false;
            enemy.Knocking();
            PlayerKO();
        }

        if (enemy.IsDefeated && !IsGameOver) {
            Player.Win();
            HudManager.GameOver("Winner");
            IsGameOver = true;
            Results.Win = true;


            DisableCharacters();
            Bars.SetBool("Inside", false);
            Spawners.SetBool("EnemyKO", true);
        }
        if (Player.IsDefeated && !IsGameOver) {
            enemy.Win();
            HudManager.GameOver("Defeated");
            IsGameOver = true;
            Results.Win = false;

            DisableCharacters();
            Bars.SetBool("Inside", false);
            Spawners.SetBool("EnemyKO", true);
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
        Player.Wait = false;
        enemy.enabled = true;
        ActivateTimer = true;
    }

    public void DisableCharacters() {
        Player.Wait = true;
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