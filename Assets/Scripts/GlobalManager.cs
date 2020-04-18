using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalManager {
    public class ConfigData
    {
        public float Volume { get; set; }
    }

    public class GameData {
        public int IDPlayer { get; set; }
        public int IDEnemy { get; set; }
        public bool IsLeague { get; set; }
    }

    public static int Money { get; set; }
    public static GameData GameplayData { get; set; }
    public static ConfigData SettingsData { get; set; }
}

public abstract class Manager : MonoBehaviour {
    void Start() {
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
}