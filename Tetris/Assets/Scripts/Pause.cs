using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    public Board board;

    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    public void Resume() {
        audioManager.Play("Ok");
        audioManager.UnPause("Theme");
        gameObject.SetActive(false);
        this.board.paused = false;
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
        this.board.paused = true;
        this.audioManager = AudioManager.instance;
        audioManager.Play("Pause");
        gameObject.SetActive(true);
        audioManager.Pause("Theme");
    }
}
