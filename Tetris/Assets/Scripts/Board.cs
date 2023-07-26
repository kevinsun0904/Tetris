using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour {
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();

        for (int i = 0; i < this.tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start() {
        SpawnPiece();
    }

    public void SpawnPiece() {
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];
    }

    public void Set() {

    }
}