using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinShopScript : MonoBehaviour
{
    public GameObject smoke, shop, error, fight, table, arrow;
    private int coins, health, defence, damage;

    public TextMeshProUGUI healthText, defenceText, damageText;

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
    }


    public void Coin100Health()
    {
        if(countHealth>=4)
        {
            
        }
        else
        {
            if(health==100)
            {

            }
            else if(coins<100)
            {
                NoCoin();
            }
            else
            {
                if(health==100)
                {

                }
                else if(health>=91 && health<=99)
                {
                    coins-=100;
                    health=100;
                    countHealth+=1;
                }
                else
                {
                    coins-=100;
                    health+=10;
                    countHealth+=1;
                }
                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("health", health);

                healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
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
            if(defence==100)
            {

            }
            else if(coins<100)
            {
                NoCoin();
            }
            else
            {
                if(defence==100)
                {

                }
                else if(defence>=91 && defence<=99)
                {
                    coins-=100;
                    defence=100;
                    countDefence+=1;
                }
                else
                {
                    coins-=100;
                    defence+=10;
                    countDefence+=1;
                }
                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("defence", defence);

                defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
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
            if(damage==100)
            {

            }
            else if(coins<100)
            {
                NoCoin();
            }
            else
            {
                if(damage==100)
                {

                }
                else if(damage>=91 && damage<=99)
                {
                    coins-=100;
                    damage=100;
                    countDamage+=1;
                }
                else
                {
                    coins-=100;
                    damage+=10;
                    countDamage+=1;
                }
                Color();
                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("damage", damage);

                damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";
            }
        }
    }

    private void Color() {

        if(health>=0 && health<=49)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);

            btnHealth.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(health>=50 && health<=70)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);

            btnHealth.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(health>=71 && health<=100)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);

            btnHealth.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }

        if(health==100){
            btnHealth.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }


        if(defence>=0 && defence<=50)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);

            btnDefence.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(defence>=51 && defence<=70)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);

            btnDefence.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(defence>=71 && defence<=100)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);

            btnDefence.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }

        if(defence==100){
            btnDefence.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }


        if(damage>=0 && damage<=50)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 0, 5,255);

            btnDamage.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(damage>=51 && damage<=70)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 205, 66,255);

            btnDamage.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }
        else if(damage>=71 && damage<=100)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(125, 255, 0,255);

            btnDamage.GetComponent<Image>().color= new Color32(125, 255, 0,255);
        }

        if(damage==100){
            btnDamage.GetComponent<Image>().color= new Color32(255, 0, 5,255);
        }
        
    }


    public void OpenFight()
    {
        if(fight!=null && table!=null)
        {
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = table.GetComponent<Animator>();
            Animator animator4 = smoke.GetComponent<Animator>();
            Animator animator5 = shop.GetComponent<Animator>();
            Animator animator6 = arrow.GetComponent<Animator>();

            if(animator2!=null && animator3!=null && animator4!=null && animator5!=null)
            {
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
                animator4.SetBool("Open", true);
                animator5.SetBool("Open", true);
                animator6.SetBool("Open", true);
            }
        }
    }

    public void CloseFight()
    {
        if(fight!=null && table!=null)
        {
            Animator animator2 = fight.GetComponent<Animator>();
            Animator animator3 = table.GetComponent<Animator>();
            Animator animator4 = smoke.GetComponent<Animator>();
            Animator animator5 = shop.GetComponent<Animator>();
            Animator animator6 = arrow.GetComponent<Animator>();

            if(animator2!=null && animator3!=null && animator4!=null && animator5!=null)
            {
                animator2.SetBool("Open", false);
                animator3.SetBool("Open", false);
                animator4.SetBool("Open", false);
                animator5.SetBool("Open", false);
                animator6.SetBool("Open", false);
            }
        }
    }


    public void CloseShop()
    {
        if(smoke!=null && shop!=null)
        {
            Animator animator = smoke.GetComponent<Animator>();
            Animator animator2 = shop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
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

    public void playgame(string Level)
    {
        SceneManager.LoadScene(Level);
    }
}
