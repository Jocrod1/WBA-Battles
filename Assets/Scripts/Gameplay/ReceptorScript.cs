using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptorScript : MonoBehaviour
{
    public bool Recieve;
    public PunchInfo Info;
    public Character Charctr;

    public void Blocked() {
        Charctr.BlockedSound();
    }

    public void Punched() {
        Charctr.PunchedSound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Reciever")
        {
            Recieve = true;
            Charctr.PunchEnded = true;
        }
    }
}
[System.Serializable]
public class PunchInfo
{
    public Vector2 PunchRawLocal { get; set; }
    public bool Hard;
    public float Damage;
    public float Stamina;

    public PunchInfo(Vector2 punchRawLocal, bool hard, float damage = 0)
    {
        PunchRawLocal = punchRawLocal;
        Hard = hard;
        Damage = damage;
    }
}