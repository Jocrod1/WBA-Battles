using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class victory : MonoBehaviour
{

    public GameObject image, background;

    public Sprite[] champ, motivationchamp1, motivationchamp2, motivationchamp3, motivationchamp4;

    void Start()
    {
        
        if(PlayerPrefs.GetInt("IDPlayer")==1)
        {
            background.GetComponent<Image>().sprite = champ[0];

            if(PlayerPrefs.GetInt("IDMotivation")==1)
            {
                image.GetComponent<Image>().sprite = motivationchamp1[0];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==2)
            {
                image.GetComponent<Image>().sprite = motivationchamp1[1];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==3)
            {
                image.GetComponent<Image>().sprite = motivationchamp1[2];
            }

        }
        else if(PlayerPrefs.GetInt("IDPlayer")==2)
        {
            background.GetComponent<Image>().sprite = champ[1];
            
            if(PlayerPrefs.GetInt("IDMotivation")==1)
            {
                image.GetComponent<Image>().sprite = motivationchamp2[0];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==2)
            {
                image.GetComponent<Image>().sprite = motivationchamp2[1];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==3)
            {
                image.GetComponent<Image>().sprite = motivationchamp2[2];
            }
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==3)
        {
            background.GetComponent<Image>().sprite = champ[2];
            
            if(PlayerPrefs.GetInt("IDMotivation")==1)
            {
                image.GetComponent<Image>().sprite = motivationchamp3[0];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==2)
            {
                image.GetComponent<Image>().sprite = motivationchamp3[1];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==3)
            {
                image.GetComponent<Image>().sprite = motivationchamp3[2];
            }
        }
        else if(PlayerPrefs.GetInt("IDPlayer")==4)
        {
            background.GetComponent<Image>().sprite = champ[3];
            
            if(PlayerPrefs.GetInt("IDMotivation")==1)
            {
                image.GetComponent<Image>().sprite = motivationchamp4[0];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==2)
            {
                image.GetComponent<Image>().sprite = motivationchamp4[1];
            }
            else if(PlayerPrefs.GetInt("IDMotivation")==3)
            {
                image.GetComponent<Image>().sprite = motivationchamp4[2];
            }
        }
        
    }
}
