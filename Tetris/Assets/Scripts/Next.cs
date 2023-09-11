using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Next : MonoBehaviour
{
    public Board board;
    public Tile tile { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position;

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    public void displayNext(TetrominoData data) {
        
        Clear();

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
        this.tile = data.tile;

        Set(data);
    }

    private void Clear() {
        //unsets the tile
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Set(TetrominoData data) {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
