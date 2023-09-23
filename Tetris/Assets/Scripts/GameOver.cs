using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    void Start() {
        this.audioManager = AudioManager.instance;
    }

    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Tetris");
    }

    public void LoadTitle() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Title");
    }

    public void SetUp() {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
}
