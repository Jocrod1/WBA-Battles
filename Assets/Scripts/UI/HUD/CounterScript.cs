using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterScript : MonoBehaviour
{
    public GameplayManager GM;

    public Animator Anim;

    public TextMeshProUGUI Timer;

    // Start is called before the first frame update
    void Start()
    {
        Counter = 1;
        Timer.text = Counter.ToString();
        Anim  = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Counter;

    public void incr() {
        if (Counter >= 8)
            Anim.SetTrigger("Continue");
        else {
            Counter++;
            Timer.text = Counter.ToString();
        }
        
    }

    public void EndAnimation() {
        GM.Counting = false;
    }
}
