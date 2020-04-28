using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalManager {
    public static class SettingsData
    {
        public static float Volume { get; set; }
    }

    public static class GameplayData
    {
        public static int IDPlayer { get; set; }
        public static int IDEnemy { get; set; }
        public static bool IsLeague { get; set; }
    }

    public static int Money { get; set; }
    //public static GameData GameplayData { get; set; }
    //public static ConfigData SettingsData { get; set; }
}

public abstract class Manager : MonoBehaviour {
    void Awake() {
        LoadGlobalManager();
    }

    public virtual void LoadGlobalManager() {
        //Settings
        GlobalManager.SettingsData.Volume = PlayerPrefs.GetFloat("volume");

        //Money
        GlobalManager.Money = PlayerPrefs.GetInt("money", 0);

        //GameData
        GlobalManager.GameplayData.IDPlayer = PlayerPrefs.GetInt("IDPlayer", -1);
        GlobalManager.GameplayData.IDEnemy = PlayerPrefs.GetInt("IDEnemy", -1);
        int Valid = PlayerPrefs.GetInt("IsLeague", -1);
        GlobalManager.GameplayData.IsLeague = Valid == 1;
    }

    public virtual void SaveGlobalManager() {
        //Settings

        //ESTA MIERDA QUEDO SUSPENDIDA HASTA QUE SE PUEDA HACER BIEN NOJODA
    }

    private void OnApplicationQuit()
    {
        
    }

    private void OnApplicationFocus(bool focus)
    {
        
    }

    private void OnApplicationPause(bool pause)
    {
        
    }
}