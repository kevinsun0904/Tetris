using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    void Start() {
        audioManager = AudioManager.instance;
    }

    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Tetris");
    }

    public void loadTitle() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Title");
    }

    public void SetUp() {
        gameObject.SetActive(true);
    }
}
