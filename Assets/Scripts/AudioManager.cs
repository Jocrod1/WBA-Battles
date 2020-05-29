using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {

    public AudioMixerGroup mixerGroup;

    public AudioSource Source;

    public string Name;
    public AudioClip Clip;

    [Range(0, 1)]
    public float volume = 1;
    [Range(-3, 3)]
    public float pitch = 1;

    public bool Loop = false;
    public bool PlayOnAwake = false;

    public void SetSource(AudioSource Src) {
        Source = Src;
        Source.clip = Clip;
        Source.pitch = pitch;
        Source.volume = volume;
        Source.playOnAwake = PlayOnAwake;
        Source.loop = Loop;
        Source.outputAudioMixerGroup = mixerGroup;
    }

    public void Play() {
        Source.Play();
    }
}

public class AudioManager : MonoBehaviour {

    [SerializeField]
    List<Sound> Sounds;

    private void Start()
    {
        for (int i = 0; i < Sounds.Count; i++) {
            GameObject obj = new GameObject("Sound_" + i + "_" + Sounds[i].Name);
            obj.transform.SetParent(transform);
            Sounds[i].SetSource(obj.AddComponent<AudioSource>());
        }

        PlaySound("BgMusic");
    }

    public Sound PlaySound(string name) {
        for (int i = 0; i < Sounds.Count; i++) {
            if(Sounds[i].Name == name) {
                Sounds[i].Play();
                return Sounds[i];
            }
        }

        return null;
    }

}
