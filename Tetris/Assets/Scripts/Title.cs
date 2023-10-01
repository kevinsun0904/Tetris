using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    /// <summary>
    /// Initializes the audioManager
    /// </summary>
    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    /// <summary>
    /// Starts the game by loading the Tetris scene
    /// </summary>
    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Tetris");
    }
}
