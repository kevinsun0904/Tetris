using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    public Board board;

    /// <summary>
    /// Initialize the audioManager
    /// </summary>
    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    /// <summary>
    /// Unpauses the game and removes the pause screen
    /// </summary>
    public void Resume() {
        audioManager.Play("Ok");
        audioManager.UnPause("Theme");
        gameObject.SetActive(false);
        this.board.paused = false;
    }

    /// <summary>
    /// Restarts the game by loading the scene again
    /// </summary>
    public void LoadLevel() {
        audioManager.Play("Ok");
        audioManager.Stop("Theme");
        SceneManager.LoadScene("Tetris");
    }

    /// <summary>
    /// Loads the title screen
    /// </summary>
    public void LoadTitle() {
        audioManager.Play("Ok");
        audioManager.Stop("Theme");
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// Called when the game is paused
    /// Pauses the Update() in piece
    /// Displays the pause screen
    /// </summary>
    public void PauseGame() {
        this.board.paused = true;
        this.audioManager = AudioManager.instance;
        audioManager.Play("Pause");
        gameObject.SetActive(true);
        audioManager.Pause("Theme");
    }
}
