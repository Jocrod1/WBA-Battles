using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{ 
    public static bool GameIsPaused = false;

    public GameObject PausePanel;

    // Start is called before the first frame update
    void Start()
    {
        GameIsPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume() {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
