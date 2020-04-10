using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnShop : MonoBehaviour
{
    public float speed;

    public GameObject Shop, Options, Menu;

    private Vector3 shopPosition, optionsPosition, menuPosition;

    private bool openShop, openOptions, openMenu, moving;

    private void Start() {

        shopPosition=Shop.transform.position;
        optionsPosition=Options.transform.position;
        menuPosition=Menu.transform.position;

    }

    public void OpenShop()
    {

        if(Shop!=null && Options!=null && Menu!=null)
        {
            if(menuPosition==Menu.transform.position)
            {
                openShop=true;
            }
        }
    }

    private void FixedUpdate() {
        if(openShop)
        {
            Shop.transform.position=Vector3.MoveTowards(Shop.transform.position, new Vector3(menuPosition.x, Shop.transform.position.y, 0), speed * Time.deltaTime);
            Options.transform.position=Vector3.MoveTowards(Options.transform.position, new Vector3(optionsPosition.x, Options.transform.position.y, 0), speed * Time.deltaTime);
            Menu.transform.position=Vector3.MoveTowards(Menu.transform.position, new Vector3(optionsPosition.x, Menu.transform.position.y, 0), speed * Time.deltaTime);


        }
    }
}
