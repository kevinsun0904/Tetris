using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;
    public Board board;

    /// <summary>
    /// Initialize audioManager
    /// </summary>
    void Start() {
        this.audioManager = AudioManager.instance;
    }

    /// <summary>
    /// Load the tetris scene again to restart
    /// </summary>
    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Tetris");
    }

    /// <summary>
    /// Load title screen
    /// </summary>
    public void LoadTitle() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// Display the game over screen and pause the game
    /// </summary>
    public void SetUp() {
        this.board.paused = true;
        gameObject.SetActive(true);
    }
}
