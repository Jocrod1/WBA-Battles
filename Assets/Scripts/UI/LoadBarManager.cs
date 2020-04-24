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

        while (operation.progress < 0.9f) {
            float ScaledPerc = Mathf.Clamp01(0.5f * operation.progress / 0.9f);
            print(operation.progress);

            slider.value = ScaledPerc;
            yield return null;
        }

        operation.allowSceneActivation = true;
        float perc = 0.5f;

        while (!operation.isDone) {
            yield return null;
            perc = Mathf.Lerp(perc, 1f, 0.005f);
            slider.value = perc;
        }

        slider.value = 1f;
        yield return new WaitForSeconds(0.2f);
    }
}
