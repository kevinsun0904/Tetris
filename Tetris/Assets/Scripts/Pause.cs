using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    public void Resume() {
        audioManager.Play("Ok");
        audioManager.UnPause("Theme");
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadLevel() {
        audioManager.Play("Ok");
        audioManager.Stop("Theme");
        SceneManager.LoadScene("Tetris");
    }

    public void LoadTitle() {
        audioManager.Play("Ok");
        audioManager.Stop("Theme");
        SceneManager.LoadScene("Title");
    }

    public void PauseGame() {
        this.audioManager = AudioManager.instance;
        audioManager.Play("Pause");
        Time.timeScale = 0;
        gameObject.SetActive(true);
        audioManager.Pause("Theme");
    }
}
