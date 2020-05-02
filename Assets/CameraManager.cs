using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Animator BarsAnimator;

    public Animator SpawnersAnimator;

    public GameplayManager manager;

    public void SetIntroBars() {
        BarsAnimator.SetBool("Inside", true);
    }

    public void SetIntroSpawners() {
        SpawnersAnimator.SetBool("Inside", true);
    }

    public void EnableCharacters() {
        manager.EnableCharacters();
    }

}
