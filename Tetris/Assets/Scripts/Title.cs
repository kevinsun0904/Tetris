using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string tetris;
    [HideInInspector]
    public AudioManager audioManager;

    void Start() {
        audioManager = AudioManager.instance;
    }

    public void LoadLevel() {
        audioManager.Play("Ok");
        SceneManager.LoadScene(tetris);
    }
}
