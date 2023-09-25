using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour
{
    public Board board;
    public Tile tile { get; private set; }
    public Tilemap tilemap { get; private set; }
    public int CurrentScore = 0;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CurrentScore = CalculateScore(board.ClearLines(), 0);
    }

    public int CalculateScore(int LinesCleared, int Level)
    {
        if(LinesCleared == 1){
            CurrentScore += (Level + 1) * 40;
        } else if (LinesCleared == 2){
            CurrentScore += (Level + 1) * 100;
        } else if (LinesCleared == 3){
            CurrentScore += (Level + 1) * 300;
        } else {
            CurrentScore += (Level + 1) * 1200;
        }
        
        return CurrentScore;
    }
}
