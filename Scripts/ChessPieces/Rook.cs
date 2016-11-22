using UnityEngine;
using System.Collections.Generic;

public class Rook : ChessPiece {
    private const int MAX_MOVES = 7;

    private List<MoveOffset> upMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> downMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> rightMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> leftMoveOffsets = new List<MoveOffset>();

    void Start() {
        for (int i = 1; i <= MAX_MOVES; i++) {
            upMoveOffsets.Add(new MoveOffset(i, 0));
            downMoveOffsets.Add(new MoveOffset(-i, 0));
            rightMoveOffsets.Add(new MoveOffset(0, i));
            leftMoveOffsets.Add(new MoveOffset(0, -i));
        }
    }

    override public List<MoveOffset> getUpMoveOffsets() {
        return upMoveOffsets;
    }

    override public List<MoveOffset> getDownMoveOffsets() {
        return downMoveOffsets;
    }

    override public List<MoveOffset> getRightMoveOffsets() {
        return rightMoveOffsets;
    }

    override public List<MoveOffset> getLeftMoveOffsets() {
        return leftMoveOffsets;
    }
}
