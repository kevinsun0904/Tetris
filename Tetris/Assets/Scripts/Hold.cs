using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hold : MonoBehaviour
{
    public Image background;
    public Sprite tetrominoI;
    public Sprite tetrominoO;
    public Sprite tetrominoZ;
    public Sprite tetrominoS;
    public Sprite tetrominoL;
    public Sprite tetrominoJ;
    public Sprite tetrominoT;
    public bool hasTetromino { get; private set; }
    public TetrominoData currentHold { get; private set; }

    public void Initialize() {
        this.hasTetromino = false;
    }

    public bool HasTetromino() {
        return hasTetromino;
    }

    public void Set(TetrominoData data) {
        this.currentHold = data;
        this.hasTetromino = true;

        switch(data.tetromino) {
            case Tetromino.I:
                background.sprite = tetrominoI;
                break;
            case Tetromino.O:
                background.sprite = tetrominoO;
                break;
            case Tetromino.S:
                background.sprite = tetrominoS;
                break;
            case Tetromino.Z:
                background.sprite = tetrominoZ;
                break;
            case Tetromino.J:
                background.sprite = tetrominoJ;
                break;
            case Tetromino.L:
                background.sprite = tetrominoL;
                break;
            default:
                background.sprite = tetrominoT;
                break;
        }
    }
}
