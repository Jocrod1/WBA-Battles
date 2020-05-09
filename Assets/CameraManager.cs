using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Animator BarsAnimator;

    public Animator SpawnersAnimator;

    public GameplayManager manager;

    void Start() {
        manager.cam = this;
    }

    public void SetIntroBars() {
        BarsAnimator.SetBool("Inside", true);
    }

    public void SetIntroSpawners() {
        SpawnersAnimator.SetBool("Inside", true);
        manager.PublicCheering(false);
    }

    public void EnableCharacters() {
        manager.EnableCharacters();
    }

    public bool Shaking;

    public Vector3 OriginalPos;

    public IEnumerator Shake(float duration, float magnitude)
    {

        GameObject Cam = Camera.main.gameObject;
        OriginalPos = Cam.transform.localPosition;

        float elapsed = 0f;

        Shaking = true;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Cam.transform.localPosition = new Vector3(OriginalPos.x + x, OriginalPos.y + y, OriginalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Shaking = false;

        Cam.transform.localPosition = OriginalPos;

    }

    public void CamShake(float dur, float mag) {
        StartCoroutine(Shake(dur, mag));
    }

}
