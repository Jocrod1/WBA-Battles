using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HUDManager : MonoBehaviour
{
    [Header("Player")]
    public Bars PlayerBars;

    [Header("Enemy")]
    public Bars EnemyBars;

    public Animator GameOverAnimator;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerBars.Manage();
        EnemyBars.Manage();

    }

    public void GameOver(string trigger) {
        GameOverAnimator.SetTrigger(trigger);
    }

    [System.Serializable]
    public class Bars
    {
        [Header("Health Bar")]
        public RectTransform Bar;
        public Character Player;

        [HideInInspector]
        public float Maxbar;

        public float SmoothBar;
        [HideInInspector]
        public float refbar;


        public RectTransform RemainBar;
        public float RSmoothBar;
        [HideInInspector]
        public float Rrefbar;

        [Header("Stamina Bar")]
        public RectTransform StaminaBar;
        [HideInInspector]
        public float MaxStaminaBar;
        public float StaminaSmoothBAr;
        [HideInInspector]
        public float StaminaRefBar;


        public RectTransform RStaminaBar;
        public float RSSmoothBar;
        [HideInInspector]
        public float RSRefBar;

        public void Manage()
        {
            Bar.localScale = new Vector2(Mathf.SmoothDamp(Bar.localScale.x, Mathf.Clamp01(Player.CurrentHealth / Player.MaxHealth), ref refbar, RSmoothBar),
                                                                        Bar.localScale.y);

            RemainBar.localScale = new Vector2(Mathf.SmoothDamp(RemainBar.localScale.x, Mathf.Clamp01(Player.CurrentHealth / Player.MaxHealth), ref Rrefbar, SmoothBar), RemainBar.localScale.y);

            //Stamina Bar
            StaminaBar.localScale = new Vector2(Mathf.SmoothDamp(StaminaBar.localScale.x, Mathf.Clamp01(Player.CurrentStamina / Player.MaxStamina), ref StaminaRefBar, StaminaSmoothBAr),
                                                    StaminaBar.localScale.y);
            RStaminaBar.localScale = new Vector2(Mathf.SmoothDamp(RStaminaBar.localScale.x, Mathf.Clamp01(Player.CurrentStamina / Player.MaxStamina), ref RSRefBar, RSSmoothBar),
                                                    RStaminaBar.localScale.y);
        }

    }
}
