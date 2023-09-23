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
        this.board.paused = true;
        gameObject.SetActive(true);
    }
}
