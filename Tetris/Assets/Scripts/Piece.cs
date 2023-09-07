using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Piece : MonoBehaviour {
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data) {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update() {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime; //deltatime is the time interval from the previous frame

        if (Input.GetKeyDown(KeyCode.Z)) {
            Rotate(-1);
        } else if (Input.GetKeyDown(KeyCode.X)) {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Move(Vector2Int.right);
        }

        //soft drop action
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            HardDrop();
        }

        if (Time.time >= this.stepTime) { //whenever a steptime is reached
            Step();
        }

        this.board.Set(this); //set all tiles after changes
    }

    private void Step() {
        this.stepTime = Time.time + stepDelay; //push step time to the future by step delay whenever it is reached

        Move(Vector2Int.down); //locktime wont reset when tetromino is at the bottom since move will fail

        if (this.lockTime >= this.lockDelay) { //rotate and move resets locktime
            Lock(); //a piece is locked whenever it isnt interacted with for lockDelay amount of time
        }
    }

    private void Lock() {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private void HardDrop() {
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock();
    }

    private bool Move(Vector2Int translation) {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid) {
            this.position = newPosition; //set center position
            this.lockTime = 0f; //resets lock time
        }

        return valid;
    }

    private void Rotate(int direction) {
        //Each tile has four states of rotation. Direction of 1 indicates a clockwise rotation, direction of -1 indicates an anti-clockwise rotation.
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4); 

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction)) {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction) {
        //Calculates the new coordinates of the tile based on the given direction and rotation matrix to complete the rotation. 
        for (int i = 0; i < this.cells.Length; i++) {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino) { //depending on the case apply rotation matrix to each tile
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    //matrix multiplication
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection) {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++) { //wallKicks is a 2d array. run through the horizontal cells for 5 tests
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i]; //wallkick array contains an offset and move tests whether the offset is possible
            //true is returned on the first successful offset
            if (Move(translation)) { //move returns a bool of whether movement is successfull
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection) {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0)); //return wallKickIndex depending on change in state and direction
    }

    private int Wrap(int input, int min, int max) {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
}