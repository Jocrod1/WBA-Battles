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

    public TextMeshProUGUI healthText, defenceText, damageText, coinText, coin2Text;

    public Button btnHealth, btnDefence, btnDamage;

    public Sprite btnYellow, btnGreen;

    float MaxHealth, CurrentHealth;

    public int valueCoins;

    public GameObject[] textButton, activatedButton;

    private void Start() {

        coins=PlayerPrefs.GetInt("money");
        health=PlayerPrefs.GetInt("health", 0);
        stamina=PlayerPrefs.GetInt("stamina", 0);
        damage=PlayerPrefs.GetInt("damage", 0);

        Color();

        coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money").ToString();
        coin2Text.GetComponent<TextMeshProUGUI>().text=  PlayerPrefs.GetInt("money").ToString();
    }


    public void Coin100Health()
    {
        if(health >= 1)
        {
            
        }
        else
        {
            if(coins<valueCoins)
            {
                NoCoin();
            }
            else
            {
                coins-=valueCoins;
                health += 1;


                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("health", health);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                Color();


                coinText.GetComponent<TextMeshProUGUI>().text=  PlayerPrefs.GetInt("money").ToString();
                coin2Text.GetComponent<TextMeshProUGUI>().text=  PlayerPrefs.GetInt("money").ToString();
            }
        }
    }

    public void Coin100Defence()
    {
        if(stamina >= 1)
        {
            
        }
        else
        {
            if(coins<valueCoins)
            {
                NoCoin();
            }
            else
            {
                coins-=valueCoins;
                stamina += 1;


                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("stamina", stamina);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                Color();

                coinText.GetComponent<TextMeshProUGUI>().text=  PlayerPrefs.GetInt("money").ToString();
                coin2Text.GetComponent<TextMeshProUGUI>().text=  PlayerPrefs.GetInt("money").ToString();
            }
        }
    }

    public void Coin100Damage()
    {
        if(damage >= 1)
        {
            
        }
        else
        {
            if(coins<valueCoins)
            {
                NoCoin();
            }
            else
            {
                coins-=valueCoins;
                damage += 1;


                PlayerPrefs.SetInt("money", coins);
                PlayerPrefs.SetInt("damage", damage);
                PlayerPrefs.SetString("WaitOneHour", "-1");

                Color();

                coinText.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money").ToString();
                coin2Text.GetComponent<TextMeshProUGUI>().text= PlayerPrefs.GetInt("money").ToString();
            }
        }
    }

    private void Color() {
        
        if(health==1)
        {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnHealth.GetComponent<Image>().sprite = btnGreen;
            textButton[0].SetActive(false);
            activatedButton[0].SetActive(true);
        }
        else {
            healthText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnHealth.GetComponent<Image>().sprite = btnYellow;
            textButton[0].SetActive(true);
            activatedButton[0].SetActive(false);
        }

        if(stamina==1)
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnDefence.GetComponent<Image>().sprite = btnGreen;
            textButton[1].SetActive(false);
            activatedButton[1].SetActive(true);
        }
        else
        {
            defenceText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnDefence.GetComponent<Image>().sprite = btnYellow;
            textButton[1].SetActive(true);
            activatedButton[1].SetActive(false);
        }

        if(damage==1)
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnDamage.GetComponent<Image>().sprite = btnGreen;
            textButton[2].SetActive(false);
            activatedButton[2].SetActive(true);
        }
        else
        {
            damageText.GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            btnDamage.GetComponent<Image>().sprite = btnYellow;
            textButton[2].SetActive(true);
            activatedButton[2].SetActive(false);
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
