using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Score : MonoBehaviour
{
    public Board board;
    public Tile tile { get; private set; }
    public Tilemap tilemap { get; private set; }
    public int CurrentScore;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateScore(){

    }
}
