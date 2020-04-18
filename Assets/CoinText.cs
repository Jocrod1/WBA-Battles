﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinText : MonoBehaviour
{

    private int coins;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text=GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        coins = GlobalManager.Money;

        text.text=coins.ToString();
    }
}
