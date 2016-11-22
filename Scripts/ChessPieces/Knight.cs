using UnityEngine;
using System.Collections.Generic;

public class Knight : ChessPiece {
    private List<MoveOffset> knightMoveOffsets = new List<MoveOffset>();

    void Start() {
        knightMoveOffsets.Add(new MoveOffset(2, 1));
        knightMoveOffsets.Add(new MoveOffset(2, -1));
        knightMoveOffsets.Add(new MoveOffset(-2, 1));
        knightMoveOffsets.Add(new MoveOffset(-2, -1));
        knightMoveOffsets.Add(new MoveOffset(1, 2));
        knightMoveOffsets.Add(new MoveOffset(1, -2));
        knightMoveOffsets.Add(new MoveOffset(-1, 2));
        knightMoveOffsets.Add(new MoveOffset(-1, -2));
    }

    override public List<MoveOffset> getKnightMoveOffsets() {
        return knightMoveOffsets;
    }
}
