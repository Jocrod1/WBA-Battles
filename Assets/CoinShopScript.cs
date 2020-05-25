using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinShopScript : MonoBehaviour
{
    public GameObject smoke, error, fight, table, wall, realShop;
    private int coins, health, stamina, damage;

    public TextMeshProUGUI healthText, defenceText, damageText, coinText, coin2Text, healthCount, defenceCount, damageCount;

    public Button btnHealth, btnDefence, btnDamage;

    private int countHealth, countStamina, countDamage;

    float MaxHealth, CurrentHealth;

    private void Start() {

        countHealth=0;
        countStamina=0;
        countDamage=0;

        coins=PlayerPrefs.GetInt("money");
        health=PlayerPrefs.GetInt("health", 0);
        stamina=PlayerPrefs.GetInt("stamina", 0);
        damage=PlayerPrefs.GetInt("damage", 0);

        Color();

        healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health", -1) + "/100";
        defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("stamina", -1) + "/100";
        damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage", -1) + "/100";

        coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
        coin2Text.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";

        healthCount.GetComponent<TextMeshProUGUI>().text= health + "/4";
        defenceCount.GetComponent<TextMeshProUGUI>().text= stamina + "/4";
        damageCount.GetComponent<TextMeshProUGUI>().text= damage + "/4";
    }


    public void Coin100Health()
    {
        if(health >= 4)
        {
            
        }
        else
        {
            if(coins<100)
            {
                NoCoin();
            }
            else
            {
                coins-=100;
                health += 1;


                Color();

                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("health", health);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                coin2Text.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                healthCount.GetComponent<TextMeshProUGUI>().text= health + "/4";
            }
        }
    }

    public void Coin100Defence()
    {
        if(stamina >= 4)
        {
            
        }
        else
        {
            if(coins<100)
            {
                NoCoin();
            }
            else
            {
                coins-=100;
                stamina += 1;

                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("stamina", stamina);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                coin2Text.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                defenceCount.GetComponent<TextMeshProUGUI>().text= stamina + "/4";
            }
        }
    }

    public void Coin100Damage()
    {
        if(damage >= 4)
        {
            
        }
        else
        {
            if(coins<100)
            {
                NoCoin();
            }
            else
            {
                coins-=100;
                damage += 1;

                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("damage", damage);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                coin2Text.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                damageCount.GetComponent<TextMeshProUGUI>().text= damage + "/4";
            }
        }
    }

    private void Color() {
        
        if(health>=0 && health<=49)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);
        }
        else if(health>=50 && health<=70)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);
        }
        else if(health>=71 && health<=100)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);
        }

        if(stamina>=0 && stamina<=50)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);
        }
        else if(stamina>=51 && stamina<=70)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);
        }
        else if(stamina>=71 && stamina<=100)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);
        }
        
        if(damage>=0 && damage<=50)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);
        }
        else if(damage>=51 && damage<=70)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);
        }
        else if(damage>=71 && damage<=100)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);
        }
        
        if(countHealth==4)
        {
            btnHealth.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(coins<=99)
        {
            btnHealth.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }
        else{
            btnHealth.GetComponent<Image>().color= new Color32(255, 255, 255,255);
        }

        if(countStamina==4)
        {
            btnDefence.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(coins<=99)
        {
            btnDefence.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }
        else{
            btnDefence.GetComponent<Image>().color= new Color32(255, 255, 255,255);
        }

        if(countDamage==4)
        {
            btnDamage.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(coins<=99)
        {
            btnDamage.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }
        else{
            btnDamage.GetComponent<Image>().color= new Color32(255, 255, 255,255);
        }
        
    }


    public void OpenFight()
    {
        if(fight!=null && table!=null)
        {
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = table.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
            }
        }
    }

    public void CloseFight()
    {
        if(fight!=null && table!=null)
        {
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = table.GetComponent<Animator>();

            if(animator2!=null && animator3!=null)
            {
                animator2.SetBool("Open", false);
                animator3.SetBool("Open", false);
            }
        }
    }

    private void NoCoin()
    {
        if(smoke!=null && error!=null)
        {
            Animator animator = smoke.GetComponent<Animator>();
            Animator animator2 = error.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }
    public void CloseError()
    {
        if(smoke!=null && error!=null)
        {
            Animator animator = smoke.GetComponent<Animator>();
            Animator animator2 = error.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }

    
    public void OpenShop()
    {
        if(smoke!=null && realShop!=null)
        {
            Animator animator = smoke.GetComponent<Animator>();
            Animator animator2 = realShop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                smoke.transform.SetSiblingIndex(1);

                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseShop()
    {
        if(smoke!=null && realShop!=null)
        {
            Animator animator = smoke.GetComponent<Animator>();
            Animator animator2 = realShop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                smoke.transform.SetSiblingIndex(0);

                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }

}
