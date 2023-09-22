using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake() {
        
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in this.sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void Start() {
        //sound effect to play at the start
    }

    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Play();
    }

    public void PlayDelayed (string name, float delayTime) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.PlayDelayed(delayTime);
    }

    public void Pause (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Pause();
    }

    public void UnPause (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.UnPause();
    }

    public void Stop (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Stop();
    }
}
