using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece; //the actual falling piece on the board

    public Tilemap tilemap { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate() {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear() {
        //unsets the tile
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    
    private void Copy() {
        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    private void Drop() {
        Vector3Int position = this.trackingPiece.position;

        int current = position.y;
        int bottom = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingPiece); //clear the tracking piece so that it isnt taking up the space 

        for (int row = current; row >= bottom; row--) {
            position.y = row; //change the y axis of the position to be checked to the bottom

            if (this.board.IsValidPosition(this.trackingPiece, position)) { //check the original piece against the bottom position
                this.position = position; //update position of ghost piece
            }
            else {
                break;
            }
        }

        this.board.Set(this.trackingPiece);
    }

    private void Set() {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
