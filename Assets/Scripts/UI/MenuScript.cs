using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
