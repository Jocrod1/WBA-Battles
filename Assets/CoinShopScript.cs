using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinShopScript : MonoBehaviour
{
    public GameObject smoke, shop, error, fight, table;
    private int coins, health, defence, damage;

    public TextMeshProUGUI healthText, defenceText, damageText;

    private void Start() {
        coins=PlayerPrefs.GetInt("money");

        health=PlayerPrefs.GetInt("health");
        defence=PlayerPrefs.GetInt("defence");
        damage=PlayerPrefs.GetInt("damage");

        healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
        defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
        damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";
    }


    public void Coin100Health()
    {
        if(coins<100)
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
            }
            else
            {
                coins-=100;
                health+=10;
            }

            PlayerPrefs.SetInt("money", coins);
            PlayerPrefs.SetInt("health", health);

            healthText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("health") + "/100";
        }
    }

    public void Coin100Defence()
    {
        if(coins<100)
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
            }
            else
            {
                coins-=100;
                defence+=10;
            }

            PlayerPrefs.SetInt("money", coins);
            PlayerPrefs.SetInt("defence", defence);

            defenceText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("defence") + "/100";
        }
    }

    public void Coin100Damage()
    {
        if(coins<100)
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
            }
            else
            {
                coins-=100;
                damage+=10;
            }

            PlayerPrefs.SetInt("money", coins);
            PlayerPrefs.SetInt("damage", damage);

            damageText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("damage") + "/100";
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

            if(animator2!=null && animator3!=null && animator4!=null && animator5!=null)
            {
                animator2.SetBool("Open", true);
                animator3.SetBool("Open", true);
                animator4.SetBool("Open", true);
                animator5.SetBool("Open", true);
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
}
