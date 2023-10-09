using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    public Slider musicVolume;
    public Slider sfxVolume;

    private float previousMusic;
    private float previousSFX;

    /// <summary>
    /// Initializes the audioManager
    /// </summary>
    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    void Start() {
        this.musicVolume.value = audioManager.GetMusic();
        this.sfxVolume.value = audioManager.GetSFX();

        this.musicVolume.onValueChanged.AddListener((Music) => {
            audioManager.SetMusic(musicVolume.value);
        });

        this.sfxVolume.onValueChanged.AddListener((SFX) => {
            audioManager.SetSFX(sfxVolume.value);
        });
    }

    public void DisplayAudio() {
        gameObject.SetActive(true);
        this.musicVolume.value = audioManager.GetMusic();
        this.sfxVolume.value = audioManager.GetSFX();
        this.previousMusic = this.musicVolume.value;
        this.previousSFX = this.sfxVolume.value;
    }

    public void Back() {
        audioManager.Play("Ok");
        gameObject.SetActive(false);
    }

    public void SaveAndExit() {
        Back();
    }

    public void Cancel() {
        audioManager.SetMusic(this.previousMusic);
        audioManager.SetSFX(this.previousSFX);
        Back();
    }
}
