using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [HideInInspector]
    public AudioManager audioManager;

    void Awake() {
        this.audioManager = AudioManager.instance;
    }

    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene("Tetris");
    }
}
