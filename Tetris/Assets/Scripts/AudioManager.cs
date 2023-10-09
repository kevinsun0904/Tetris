using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// This class manages the audio played during the duration of the program.
    /// </summary>
    public Sound[] sounds;

    public static AudioManager instance;
    private float musicVolume;
    private float sfxVolume;

    /// <summary>
    /// Initializes all the AudioClips into AudioSources
    /// Check if there is an audio manager from previous scenes and destroy this if there is
    /// </summary>
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

            if (sound.name.Equals("Theme")) this.musicVolume = sound.volume;
            else this.sfxVolume = sound.volume;
        }
    }

    void Start() {
        //sound effect to play at the start
    }

    /// <summary>
    /// Plays an audio clip
    /// </summary>
    /// <param name="name">name of the audio clip</param>
    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Play();
    }

    /// <summary>
    /// Play an audio clip with delay
    /// </summary>
    /// <param name="name">name of audio clip</param>
    /// <param name="delayTime">time to delay</param>
    public void PlayDelayed (string name, float delayTime) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.PlayDelayed(delayTime);
    }

    /// <summary>
    /// Pause an audio clip
    /// </summary>
    /// <param name="name">Name of audio clip</param>
    public void Pause (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Pause();
    }

    /// <summary>
    /// Unpause an audio clip
    /// </summary>
    /// <param name="name">Name of audio clip</param>
    public void UnPause (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.UnPause();
    }

    /// <summary>
    /// Stop an audio clip
    /// </summary>
    /// <param name="name">Name of audio clip</param>
    public void Stop (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name); //find sound if sound.name == name

        if (s == null) {
            Debug.LogWarning("Sound: " + name + " was not found");
            return;
        }

        s.source.Stop();
    }

    public void SetMusic(float volume) {
        if (volume > 1 || volume < 0) return;

        this.musicVolume = volume;

        foreach (Sound sound in this.sounds) {
            if (sound.name.Equals("Theme")) {
                sound.source.volume = volume;
                sound.volume = volume;
            }
        }
    }

    public float GetMusic() {
        return this.musicVolume;
    }

    public void SetSFX(float volume) {
        if (volume > 1 || volume < 0) return;

        this.sfxVolume = volume;

        foreach (Sound sound in this.sounds) {
            if (!sound.name.Equals("Theme")) {
                sound.source.volume = volume;
                sound.volume = volume;
            }
        }
    }

    public float GetSFX() {
        return this.sfxVolume;
    }
}
