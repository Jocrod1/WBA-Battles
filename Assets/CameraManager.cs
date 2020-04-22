using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Animator BarsAnimator;

    public Animator SpawnersAnimator;

    public GameplayManager manager;

    public void SetIntroBars() {
        BarsAnimator.SetTrigger("Intro");
    }

    public void SetIntroSpawners() {
        SpawnersAnimator.SetTrigger("Intro");
    }

    public void EnableCharacters() {
        manager.EnableCharacters();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
