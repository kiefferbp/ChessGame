using UnityEngine;
using System.Collections.Generic;

public class Pawn : ChessPiece {

    private bool hasMoved = false; // Pawns can MoveOffset two spaces only at opening MoveOffset
    private List<MoveOffset> upMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> specialMoveOffsets = new List<MoveOffset>();

	void Start () {
        specialMoveOffsets.Add(new MoveOffset(1, 1));
        specialMoveOffsets.Add(new MoveOffset(1, -1));
    }

    override public List<MoveOffset> getUpMoveOffsets() {
        upMoveOffsets = new List<MoveOffset>();

        // unless a piece is blocking it, the pawn can always move up a row
        upMoveOffsets.Add(new MoveOffset(1, 0));

        // if the pawn hasn't moved yet, it can also move up 2 rows
        if (!hasMoved) {
            upMoveOffsets.Add(new MoveOffset(2, 0));
        }

        return upMoveOffsets;
    }

    // the pawn's diagonal attacking moves
    override public List<MoveOffset> getSpecialMoveOffsets() {
        return specialMoveOffsets;
    }

    public void markAsMoved() {
        hasMoved = true;
    }

    public bool hasMovedYet() {
        return hasMoved;
    }
}
