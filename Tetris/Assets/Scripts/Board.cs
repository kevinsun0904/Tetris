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

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }

        tetrominoQueue = new Queue<TetrominoData>();

        this.level = 1;
        this.linesCleared = 0;

        PopulateQueue();
    }

    private void Start() {
        audioManager = AudioManager.instance;
        SpawnPiece();
        audioManager.PlayDelayed("Theme", .5f);
    }

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

    private void GameOver() {
        audioManager.Stop("Theme");
        audioManager.Play("GameOver");
        gameOverScreen.SetUp();
    }

    public void Set(Piece piece) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece) {
        //unsets the tile
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

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

    public void ClearLines() {
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
    }

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
        if (this.linesCleared % 10 == 0) {
            this.level++; //increase level every 10 lines cleared
        }
    }

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

    public void Hold() {
        TetrominoData previousHold = this.hold.currentHold;
        bool hasTetromino = this.hold.HasTetromino();
        this.hold.Set(this.activePiece.data);

        Clear(this.activePiece);

        SpawnHold(previousHold, hasTetromino);
    }

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