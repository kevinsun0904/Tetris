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

    /// <summary>
    /// Sets up the tilemap and creates the cells array
    /// </summary>
    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    /// <summary>
    /// Runs after the Update() method
    /// Clears the previous ghost piece
    /// Copies the cells from the active piece in board
    /// Drops it to the bottom of the board
    /// Sets the piece on the tilemap
    /// </summary>
    private void LateUpdate() {
        Clear();
        Copy();
        Drop();
        Set();
    }

    /// <summary>
    /// Clears the ghost piece by setting the tiles to null
    /// </summary>
    private void Clear() {
        //unsets the tile
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }
    
    /// <summary>
    /// Copies the cells of the active piece in board
    /// </summary>
    private void Copy() {
        for (int i = 0; i < this.cells.Length; i++) {
            this.cells[i] = this.trackingPiece.cells[i];
        }
    }

    /// <summary>
    /// Drops the piece to the bottom to display the ghost piece
    /// </summary>
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

    /// <summary>
    /// Set the piece on the tilemap for ghost piece
    /// </summary>
    private void Set() {
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }
}
