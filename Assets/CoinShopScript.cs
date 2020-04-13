using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShopScript : MonoBehaviour
{
    public GameObject Smoke, Shop, Error;
    private int coins;

    private void Start() {
        coins=PlayerPrefs.GetInt("money");
    }


    public void Coin100()
    {
        if(coins<100)
        {
            NoCoin();
        }
        else
        {
            coins-=100;

            PlayerPrefs.SetInt("money", coins);
        }
    }

    public void Coin200()
    {
        if(coins<200)
        {
            NoCoin();
        }
        else
        {
            coins-=200;

            PlayerPrefs.SetInt("money", coins);
        }
    }

    public void Coin500()
    {
        if(coins<500)
        {
            NoCoin();
        }
        else
        {
            coins-=500;

            PlayerPrefs.SetInt("money", coins);
        }
    }

    private void NoCoin()
    {
        if(Smoke!=null && Error!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Error.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }


    public void OpenShop()
    {
        if(Smoke!=null && Shop!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Shop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", true);
                animator2.SetBool("Open", true);
            }
        }
    }

    public void CloseShop()
    {
        if(Smoke!=null && Shop!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Shop.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }

    public void CloseError()
    {
        if(Smoke!=null && Error!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Error.GetComponent<Animator>();

            if(animator!=null && animator2!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }
}
