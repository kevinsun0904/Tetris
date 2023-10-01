using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Next : MonoBehaviour
{
    public Image background;
    public Sprite tetrominoI;
    public Sprite tetrominoO;
    public Sprite tetrominoZ;
    public Sprite tetrominoS;
    public Sprite tetrominoL;
    public Sprite tetrominoJ;
    public Sprite tetrominoT;

    /// <summary>
    /// Displays the next tetromino in the queue
    /// </summary>
    /// <param name="data">The next tetromino in queue</param>
    public void displayNext(TetrominoData data) {
        
        /*
        Clear();

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
        this.tile = data.tile;

        Set(data);
        */

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
