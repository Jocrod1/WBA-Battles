using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShopScript : MonoBehaviour
{
    public GameObject Smoke, Shop;
    private int coins;

    private void Start() {
        coins=PlayerPrefs.GetInt("money");
    }


    public void Coin100()
    {
        coins-=100;

        PlayerPrefs.SetInt("money", coins);
    }

    public void Coin200()
    {
        coins-=200;

        PlayerPrefs.SetInt("money", coins);
    }

        public void Coin500()
    {
        coins-=500;

        PlayerPrefs.SetInt("money", coins);
    }


    public void OpenShop()
    {
        if(Smoke!=null && Shop!=null)
        {
            Animator animator = Smoke.GetComponent<Animator>();
            Animator animator2 = Shop.GetComponent<Animator>();

            if(animator!=null)
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

            if(animator!=null)
            {
                animator.SetBool("Open", false);
                animator2.SetBool("Open", false);
            }
        }
    }
}
