using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinShopScript : MonoBehaviour
{
    private int coins;

    private void Start() {
        coins=PlayerPrefs.GetInt("money");
    }


    public void Coin100()
    {
        coins-=100;

        PlayerPrefs.SetInt("money", coins);
    }

}
