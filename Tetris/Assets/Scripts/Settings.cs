using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    public Pause pause;
    public Audio audioControl;

    /// <summary>
    /// Initializes the audioManager
    /// </summary>
    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    public void LoadAudio() {
        audioManager.Play("Ok");
        audioControl.DisplayAudio();
    }

    public void LoadControls() {
        audioManager.Play("Ok");
    }

    public void Back() {
        audioManager.Play("Ok");
        gameObject.SetActive(false);
    }

    public void DisplaySettings() {
        gameObject.SetActive(true);
    }
}
