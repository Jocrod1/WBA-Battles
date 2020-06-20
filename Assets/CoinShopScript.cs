using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Digicrafts.IAP.Pro;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;
using Digicrafts.IAP.Pro.Events;
using Digicrafts.IAP.Pro.UI;

public class CoinShopScript : MonoBehaviour
{
    [Header("Prices")]
    public int price;

    [Header("Scenes")]
    public GameObject smoke;
    public GameObject error;
    public GameObject fight;
    public GameObject table;
    public GameObject wall;
    public GameObject realShop;

    private int health, stamina, damage;

    [Header("Button Text")]
    public GameObject[] textButton;

    [Header("Button Activated Text")]
    public GameObject[] activatedButton;

    [Header("Button PowerUp")]
    public Button[] powerButton;

    [Header("Button Sprite")]
    public Sprite btnYellow;
    public Sprite btnGreen;

    [Header("Audio")]
    public AudioManager AM;

    private void Start() {

        health=PlayerPrefs.GetInt("health", 0);
        stamina=PlayerPrefs.GetInt("stamina", 0);
        damage=PlayerPrefs.GetInt("damage", 0);

        Color();
    }


    public void Coin100Health()
    {
        AM.PlaySound("Btn");

        IAPCurrency currency = IAPInventoryManager.GetCurrency("coin");
        int amount = currency.amount;

        if(health==1)
        {

        }
        else if(amount>=price)
        {

            PlayerPrefs.SetInt("health", 1);
            PlayerPrefs.SetString("WaitOneHour", "-1");
            health=PlayerPrefs.GetInt("health");

            Color();
        }
        else
        {
            NoCoin();
        }

    }

    public void Coin100Defence()
    {
        AM.PlaySound("Btn");

        IAPCurrency currency = IAPInventoryManager.GetCurrency("coin");
        int amount = currency.amount;

        if(stamina==1)
        {
            
        }
        else if(amount>=price)
        {
            PlayerPrefs.SetInt("stamina", 1);
            PlayerPrefs.SetString("WaitOneHour", "-1");
            stamina=PlayerPrefs.GetInt("stamina");

            Color();
        }
        else
        {
            NoCoin();
        }
    }

    public void Coin100Damage()
    {
        AM.PlaySound("Btn");

        IAPCurrency currency = IAPInventoryManager.GetCurrency("coin");
        int amount = currency.amount;

        if(damage==1)
        {
            
        }
        else if(amount>=price)
        {
            PlayerPrefs.SetInt("damage", 1);
            PlayerPrefs.SetString("WaitOneHour", "-1");
            damage=PlayerPrefs.GetInt("damage");

            Color();
        }
        else
        {
            NoCoin();
        }
    }

    private void Color() {
        
        if(health==1)
        {
            textButton[0].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[0].GetComponent<Image>().sprite = btnGreen;
            textButton[0].SetActive(false);
            powerButton[0].GetComponent<Button>().enabled=false;
            activatedButton[0].SetActive(true);
        }
        else {
            textButton[0].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[0].GetComponent<Image>().sprite = btnYellow;
            textButton[0].SetActive(true);
            powerButton[0].GetComponent<Button>().enabled=true;
            activatedButton[0].SetActive(false);
        }

        if(stamina==1)
        {
            textButton[1].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[1].GetComponent<Image>().sprite = btnGreen;
            textButton[1].SetActive(false);
            powerButton[1].GetComponent<Button>().enabled=false;
            activatedButton[1].SetActive(true);
        }
        else
        {
            textButton[1].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[1].GetComponent<Image>().sprite = btnYellow;
            textButton[1].SetActive(true);
            powerButton[1].GetComponent<Button>().enabled=true;
            activatedButton[1].SetActive(false);
        }

        if(damage==1)
        {
            textButton[2].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[2].GetComponent<Image>().sprite = btnGreen;
            textButton[2].SetActive(false);
            powerButton[2].GetComponent<Button>().enabled=false;
            activatedButton[2].SetActive(true);
        }
        else
        {
            textButton[2].GetComponent<TextMeshProUGUI>().color= new Color32(255, 255, 255, 255);
            powerButton[2].GetComponent<Image>().sprite = btnYellow;
            textButton[2].SetActive(true);
            powerButton[2].GetComponent<Button>().enabled=true;
            activatedButton[2].SetActive(false);
        }
        
    }


    public void OpenFight()
    {
        AM.PlaySound("Btn");

        if (fight!=null && table!=null)
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
        AM.PlaySound("Return");

        if (fight!=null && table!=null)
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
        AM.PlaySound("Return");

        if (smoke!=null && error!=null)
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
        AM.PlaySound("Btn");

        if (smoke!=null && realShop!=null)
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
        AM.PlaySound("Return");

        if (smoke!=null && realShop!=null)
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
