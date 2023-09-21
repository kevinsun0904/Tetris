using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public string tetris;

    public void LoadLevel() {
        SceneManager.LoadScene(tetris);
    }
}
