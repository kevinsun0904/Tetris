using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour
{
    public Board board;
    public Tile tile { get; private set; }
    public Tilemap tilemap { get; private set; }
    [HideInInspector]
    public int currentScore = 0;
    public TMP_Text scoreText;

    void Awake(){
        
    }
    
    public void CalculateScore(int LinesCleared, int Level, bool hardDrop)
    {
        //TODO: Add softdrop and combo features in Board
        if (LinesCleared == 1){
            currentScore += Level * 40;
        } else if (LinesCleared == 2){
            currentScore += Level * 100;
        } else if (LinesCleared == 3){
            currentScore += Level * 300;
        } else {
            currentScore += Level * 1200;
        }
        
        if (hardDrop){
            // Harddrop: 2 points per cell (all tetraminos have 4 cells)
            currentScore += 8;
        }
        
        scoreText.text = "Score: " + currentScore;
        // this.scoreText.SetText("Score: " + currentScore);
    }
}
