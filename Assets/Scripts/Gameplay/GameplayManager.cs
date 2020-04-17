using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        AnimatorStateInfo stateInfo = Manager.GameOverAnimator.GetCurrentAnimatorStateInfo(0);

        if (Enemy.IsDefeated && !IsGameOver) {
            Player.Win();
            Manager.GameOver("Winner");
            IsGameOver = true;
            Results.Win = true;
        }
        if (Player.IsDefeated && !IsGameOver) {
            Enemy.Win();
            Manager.GameOver("Defeated");
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