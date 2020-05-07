using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieverScript : MonoBehaviour
{
    public bool Recieve;
    public Character Charctr;
    public Targets Target;


    [System.Serializable]
    public enum Targets {
        Player,
        Enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Recieve = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Receptor" && collision.transform.parent.parent.parent.name.Contains(Target.ToString()))
        {
            Recieve = true;
            Charctr.PunchRecieved = true;
            ReceptorScript RS = collision.transform.GetComponent<ReceptorScript>();
            PunchInfo info = RS.Info;
            if (gameObject.transform.parent.name.Contains("Blocking")) {
                Charctr.Blocked(info);
                RS.Blocked();
                return;
            }
            Charctr.Damaged(info);
            RS.Punched();
        }
    }
}

