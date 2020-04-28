using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinShopScript : MonoBehaviour
{
    public GameObject smoke, error, fight, table, wall, realShop;
    private int coins, health, defence, damage;

    public TextMeshProUGUI healthText, defenceText, damageText, coinText, healthCount, defenceCount, damageCount;

    public Button btnHealth, btnDefence, btnDamage;

    private int countHealth, countDefence, countDamage;

    private void Start() {

        countHealth=0;
        countDefence=0;
        countDamage=0;

        coins=PlayerPrefs.GetInt("money");
        health=PlayerPrefs.GetInt("health");
        defence=PlayerPrefs.GetInt("defence");
        damage=PlayerPrefs.GetInt("damage");

        Color();

        healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
        defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
        damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";

        coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";

        healthCount.GetComponent<TextMeshProUGUI>().text= countHealth + "/4";
        defenceCount.GetComponent<TextMeshProUGUI>().text= countDefence + "/4";
        damageCount.GetComponent<TextMeshProUGUI>().text= countDamage + "/4";
    }


    public void Coin100Health()
    {
        if(countHealth>=4)
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
                health=100;
                countHealth+=1;

                Color();

                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("health", health);

                healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                healthCount.GetComponent<TextMeshProUGUI>().text= countHealth + "/4";
                defenceCount.GetComponent<TextMeshProUGUI>().text= countDefence + "/4";
                damageCount.GetComponent<TextMeshProUGUI>().text= countDamage + "/4";
            }
        }
    }

    public void Coin100Defence()
    {
        if(countDefence>=4)
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
                defence=100;
                countDefence+=1;

                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("defence", defence);

                defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                defenceCount.GetComponent<TextMeshProUGUI>().text= countDefence + "/4";
            }
        }
    }

    public void Coin100Damage()
    {
        if(countDamage>=4)
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
                damage=100;
                countDamage+=1;

                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("damage", damage);

                damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";
                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money") + " coins";
                damageCount.GetComponent<TextMeshProUGUI>().text= countDamage + "/4";
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

        if(defence>=0 && defence<=50)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);
        }
        else if(defence>=51 && defence<=70)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);
        }
        else if(defence>=71 && defence<=100)
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

        if(countDefence==4)
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

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
