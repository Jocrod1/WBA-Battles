using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnShop : MonoBehaviour
{
    public float speed;

    public GameObject Shop, Options, Menu;

    private Vector3 shopPosition, optionsPosition, menuPosition;

    private bool openShop, closeShop, openOptions, closeOptions, moving=false;

    private void Start() {

        shopPosition=Shop.transform.position;
        optionsPosition=Options.transform.position;
        menuPosition=Menu.transform.position;

    }

    public void OpenShop()
    {

        if(Shop!=null && Options!=null && Menu!=null && moving==false)
        {
            if(menuPosition==Menu.transform.position)
            {
                openShop=true;
            }
        }
    }

    public void CloseShop()
    {
        if(Shop!=null && Options!=null && Menu!=null && moving==false)
        {
            if(menuPosition==Shop.transform.position)
            {
                closeShop=true;
            }
        }
    }

    public void OpenOptions()
    {
        if(Shop!=null && Options!=null && Menu!=null && moving==false)
        {
            if(menuPosition==Menu.transform.position)
            {
                openOptions=true;
            }
        }
    }

    public void CloseOptions()
    {
        if(Shop!=null && Options!=null && Menu!=null && moving==false)
        {
            if(menuPosition==Options.transform.position)
            {
                closeOptions=true;
            }
        }
    }

    private void FixedUpdate() {

        if(openShop)
        {
            moving=true;
            Shop.transform.position=Vector3.MoveTowards(Shop.transform.position, new Vector3(menuPosition.x, Shop.transform.position.y, 0), speed * Time.deltaTime);
            Options.transform.position=Vector3.MoveTowards(Options.transform.position, new Vector3(optionsPosition.x, Options.transform.position.y, 0), speed * Time.deltaTime);
            Menu.transform.position=Vector3.MoveTowards(Menu.transform.position, new Vector3(optionsPosition.x, Menu.transform.position.y, 0), speed * Time.deltaTime);

            if(Shop.transform.position.x==menuPosition.x)
            {
                moving=false;
                openShop=false;

            }
        }
        if(closeShop)
        {
            moving=true;
            Shop.transform.position=Vector3.MoveTowards(Shop.transform.position, new Vector3(shopPosition.x, Shop.transform.position.y, 0), speed * Time.deltaTime);
            Options.transform.position=Vector3.MoveTowards(Options.transform.position, new Vector3(optionsPosition.x, Options.transform.position.y, 0), speed * Time.deltaTime);
            Menu.transform.position=Vector3.MoveTowards(Menu.transform.position, new Vector3(menuPosition.x, Menu.transform.position.y, 0), speed * Time.deltaTime);

            if(Menu.transform.position.x==menuPosition.x)
            {
                moving=false;
                closeShop=false;
            }
        }



        
        if(openOptions)
        {
            moving=true;
            Shop.transform.position=Vector3.MoveTowards(Shop.transform.position, new Vector3(shopPosition.x, Shop.transform.position.y, 0), speed * Time.deltaTime);
            Options.transform.position=Vector3.MoveTowards(Options.transform.position, new Vector3(menuPosition.x, Options.transform.position.y, 0), speed * Time.deltaTime);
            Menu.transform.position=Vector3.MoveTowards(Menu.transform.position, new Vector3(shopPosition.x, Menu.transform.position.y, 0), speed * Time.deltaTime);

            if(Options.transform.position.x==menuPosition.x)
            {
                moving=false;
                openOptions=false;

            }
        }

        if(closeOptions)
        {
            moving=true;
            Options.transform.position=Vector3.MoveTowards(Options.transform.position, new Vector3(optionsPosition.x, Options.transform.position.y, 0), speed * Time.deltaTime);
            Shop.transform.position=Vector3.MoveTowards(Shop.transform.position, new Vector3(shopPosition.x, Shop.transform.position.y, 0), speed * Time.deltaTime);
            Menu.transform.position=Vector3.MoveTowards(Menu.transform.position, new Vector3(menuPosition.x, Menu.transform.position.y, 0), speed * Time.deltaTime);

            if(Menu.transform.position.x==menuPosition.x)
            {
                moving=false;
                closeOptions=false;
            }
        }
    }
}
