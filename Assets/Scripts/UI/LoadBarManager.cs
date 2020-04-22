using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadBarManager : MonoBehaviour
{
    Animator anim;
    public Slider slider;
    public GameObject Loader;
    public bool WithSlider;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LoadLevel(int sceneIndex) {
        StartCoroutine(LoadAscync(sceneIndex));
    }

    IEnumerator LoadAscync(int sceneIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");

        if(WithSlider)
            slider.gameObject.SetActive(true);

        Loader.SetActive(true);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            print(operation.progress);

            slider.value = progress;

            yield return null;
        }
    }
}
