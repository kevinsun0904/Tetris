using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Piece : MonoBehaviour {
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay;
    public float lockDelay = 0.2f;

    private float stepTime;
    private float lockTime;

    /// <summary>
    /// Used to initialize a piece whenever a new piece is created
    /// piece is created according to the TetrominoData and cells are located in terms of their offset from Data.cs
    /// </summary>
    /// <param name="board">Board of the tetris game</param>
    /// <param name="position">Position to spawn the piece</param>
    /// <param name="data">the type of tetromino</param>
    public void Initialize(Board board, Vector3Int position, TetrominoData data) {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepDelay = (float) Math.Pow(0.75, this.board.level - 1);
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;
        Time.timeScale = 1;

        if (this.cells == null) {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++) {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    /// <summary>
    /// Runs every frame and checks for the key pressed by the player, and calls other methods accordingly
    /// Checks if step time is reached and calls Step() if it is reached
    /// </summary>
    private void Update() {
        if (this.board.paused == true) return;

        this.board.Clear(this);

        this.lockTime += Time.deltaTime; //deltatime is the time interval from the previous frame

        if (Input.GetKeyDown(KeyCode.Z)) { //rotate counter clockwise
            board.audioManager.Play("Rotate");
            Rotate(-1);
        } else if (Input.GetKeyDown(KeyCode.X)) { //rotate clockwise
            board.audioManager.Play("Rotate");
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) { //move left
            board.audioManager.Play("Move");
            Move(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) { //move right
            board.audioManager.Play("Move");
            Move(Vector2Int.right);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow)) { //soft drop
            board.audioManager.Play("SoftDrop");
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space)) { //hard drop
            board.audioManager.Play("HardDrop");
            HardDrop();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) { //hold
            board.audioManager.Play("Hold");
            this.board.Hold();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { //pause
            this.board.pause.PauseGame();
        }

        if (Time.time >= this.stepTime) { //whenever a steptime is reached, call step to move the piece down
            Step();
        }

        this.board.Set(this); //set all tiles after changes
    }

    /// <summary>
    /// Called when ever a stepTime is reached and needs to automatically move tetromino down
    /// Increments the step time to accomodate for the next step
    /// Moves the tetromino down
    /// Checks if lockDelay (0.2s) is reached. If reached lock the tetromino in place by calling Lock()
    /// </summary>
    private void Step() {
        this.stepTime = Time.time + stepDelay; //push step time to the future by step delay whenever it is reached

        Move(Vector2Int.down); //locktime wont reset when tetromino is at the bottom since move will fail

        if (this.lockTime >= this.lockDelay) { //rotate and move resets locktime
            board.audioManager.Play("Landing");
            Lock(false); //a piece is locked whenever it isnt interacted with for lockDelay amount of time
        }
    }

    /// <summary>
    /// Sets the tetromino in place
    /// Calls ClearLines() in board
    /// Spawns a new piece after the lines are cleared
    /// </summary>
    /// <param name="hardDrop">bool of whether the lines are cleared during a hard drop</param>
    private void Lock(bool hardDrop) {
        this.board.Set(this);
        this.board.ClearLines(hardDrop);
        this.board.SpawnPiece();
    }

    /// <summary>
    /// Moves the tetromino down until it reaches the bottom
    /// Call lock
    /// </summary>
    private void HardDrop() {
        while (Move(Vector2Int.down)) {
            continue;
        }

        Lock(true);
    }

    /// <summary>
    /// Moves the Tetromino in a given direction
    /// Checks if the position is a valid position and resets the lock time after moving to the new position
    /// Resets the position variable which is Set() in Update()
    /// </summary>
    /// <param name="translation">Direction of movement</param>
    /// <returns>Whether the new position is valid or not</returns>
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

    /// <summary>
    /// Rotates the cells of the tetromino by applying the rotation matrix
    /// Calls the TestWallKicks() method
    /// </summary>
    /// <param name="direction"></param>
    private void Rotate(int direction) {
        //Each tile has four states of rotation. Direction of 1 indicates a clockwise rotation, direction of -1 indicates an anti-clockwise rotation.
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4); 

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction)) { //The offset of testcases are applied if it is successful, no need to revert changes
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction); //return to original direction if wallkicks fail
        }
    }

    /// <summary>
    /// Applies the rotation matrix onto all of the cells according to the type of tetromino
    /// </summary>
    /// <param name="direction">direction of rotation (1 being clockwise and -1 being anticlockwise)</param>
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

    /// <summary>
    /// Test the wallkicks according to tests in Data.cs
    /// Uses the bool return of Move() to determine if the wallkick test passes
    /// Move() moves the tetromino in an offset if the location is valid
    /// </summary>
    /// <param name="rotationIndex">current rotation of the tetromino</param>
    /// <param name="rotationDirection">direction of rotation</param>
    /// <returns>whether one of the wallkicks pass or not</returns>
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

    /// <summary>
    /// Returns a wallkick index that determs which row of test cases to test depending on the current rotation of the tetromino and the direction
    /// </summary>
    /// <param name="rotationIndex">The current rotation of the tetromino</param>
    /// <param name="rotationDirection">The direction rotated</param>
    /// <returns></returns>
    private int GetWallKickIndex(int rotationIndex, int rotationDirection) {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0)); //return wallKickIndex depending on change in state and direction
    }

    /// <summary>
    /// Performs the remainder function using the modulo operator
    /// Works with negative input
    /// Used to wrap the rotation index to find the next stage of rotation after apply the direction
    /// </summary>
    /// <param name="input"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private int Wrap(int input, int min, int max) {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
}