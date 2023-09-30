using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour {
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition; 
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Queue<TetrominoData> tetrominoQueue { get; private set; }
    public int level { get; private set; }
    public int linesCleared { get; private set; }
    public Next next;
    public Hold hold;
    [HideInInspector]
    public AudioManager audioManager = AudioManager.instance;
    public GameOver gameOverScreen;
    public Pause pause;
    [HideInInspector]
    public bool paused;
    public Score score;

    /// <summary>
    /// A RectInt used to store the bounds of the board
    /// </summary>
    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    /// <summary>
    /// Runs before Start()
    /// Initializes all the tetrominos with the original cells and populates the queue of the next pieces
    /// </summary>
    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }

        tetrominoQueue = new Queue<TetrominoData>();

        this.level = 1;
        this.linesCleared = 0;
        this.paused = false;

        PopulateQueue();
    }

    /// <summary>
    /// Initializes audioManager
    /// Plays theme song
    /// starts the game by spawning the piece
    /// </summary>
    private void Start() {
        audioManager = AudioManager.instance;
        SpawnPiece();
        audioManager.PlayDelayed("Theme", .5f);
    }

    /// <summary>
    /// Initializes the piece
    /// Check if theres anything in the queue
    /// Check if the spawn position is taken, if it then game over
    /// </summary>
    public void SpawnPiece() {
        /*
        int random = UnityEngine.Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];
        */

        this.activePiece.Initialize(this, this.spawnPosition, tetrominoQueue.Dequeue());

        if (tetrominoQueue.Count == 0) {
            PopulateQueue();
        }

        next.displayNext(tetrominoQueue.Peek());

        if (IsValidPosition(this.activePiece, this.spawnPosition)) { //gameover if piece spawn location is not valid
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }

    /// <summary>
    /// Stop music and play sound effect
    /// Display game over screen
    /// </summary>
    private void GameOver() {
        audioManager.Stop("Theme");
        audioManager.Play("GameOver");
        gameOverScreen.SetUp();
    }

    /// <summary>
    /// Sets the cellsof the piece on the tilemap
    /// </summary>
    /// <param name="piece">the piece to set</param>
    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary>
    /// Clears the peice from the board by setting tilemap to null
    /// </summary>
    /// <param name="piece">the piece to clear</param>
    public void Clear(Piece piece) {
        //unsets the tile
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// Checks if a piece can be placed at a certain position by checking if the position is outside the bounds or contains a tile
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsValidPosition(Piece piece, Vector3Int position) { //check if each of the tiles are valid
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Check all of the lines and clear the line if the line is full
    /// Calculate the number of lines cleared in one movement
    /// Play audio
    /// Call CalculateScore()
    /// </summary>
    /// <param name="hardDrop">whether the drop is a hard drop</param>
    public void ClearLines(bool hardDrop) {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int linesCleared = 0;
    
        while (row < bounds.yMax) {
            if (IsLineFull(row)) {
                LineClear(row);
                linesCleared++;
            }
            else { //only increment row if line is not full, since the line will fall when it is full and the same row needs to be tested again
                row++;
            }
        }

        switch (linesCleared) {
            case 1:
            case 2:
                audioManager.Play("Single");
                break;
            case 3:
                audioManager.Play("Triple");
                break;
            case 4:
                audioManager.Play("Tetris");
                break;
        }
        
        if (linesCleared > 0){
            score.CalculateScore(linesCleared, this.level, hardDrop);
        }
    }

    /// <summary>
    /// Checks if the lis full by checking if there is an empty tile in each row
    /// </summary>
    /// <param name="row">the row that is being checked</param>
    /// <returns></returns>
    private bool IsLineFull(int row) {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++){
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position)) { //check if position contains tile
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Clear all of the tiles in the row to clear
    /// Move all of the lines above down by one line
    /// Record total lines cleared and calculate level accordingly
    /// </summary>
    /// <param name="row">The row to clear</param>
    public void LineClear(int row) {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++){
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax) {
            for (int col = bounds.xMin; col < bounds.xMax; col++) {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }

        this.linesCleared++; //record total lines cleared
        CalculateLevel();
    }

    /// <summary>
    /// Calculates the level by checking whenever lines cleared reaches a multiple of 10
    /// </summary>
    public void CalculateLevel() {
        if (this.linesCleared % 10 == 0) {
            this.level++; //increase level every 10 lines cleared
        }
    }

    /// <summary>
    /// Populates the queue of tetrominos by adding tetrominos to an array list, shuffling the array, and then adding to the queue
    /// </summary>
    public void PopulateQueue() {
        TetrominoData[] list = new TetrominoData[14];
        for (int i = 0, j = 0; i < 7; i++, j++) {
            list[j] = this.tetrominos[i];
            list[++j] = this.tetrominos[i];
        }

        System.Random rnd = new System.Random();
        rnd.Shuffle(list);

        for (int i = 0; i < 14; i++) {
            this.tetrominoQueue.Enqueue(list[i]);
        }
    }

    /// <summary>
    /// Adds the current tetromino to the hold and clears this piece on the screen
    /// Spawns the previous tetromino held
    /// </summary>
    public void Hold() {
        TetrominoData previousHold = this.hold.currentHold;
        bool hasTetromino = this.hold.HasTetromino();
        this.hold.Set(this.activePiece.data);

        Clear(this.activePiece);

        SpawnHold(previousHold, hasTetromino);
    }

    /// <summary>
    /// Checks if there is a previous hold, if there isnt spawn new piece
    /// Spawn the previous hold and check if it is game over
    /// </summary>
    /// <param name="data">The data for the previous hold</param>
    /// <param name="hasTetromino">Whether there is a previous hold</param>
    public void SpawnHold(TetrominoData data, bool hasTetromino) {
        if (!hasTetromino) {
            SpawnPiece();
            return;
        }

        this.activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) { //gameover if piece spawn location is not valid
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }
}