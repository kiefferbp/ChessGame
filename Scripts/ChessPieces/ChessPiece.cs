using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessPiece : MonoBehaviour {
    public virtual List<MoveOffset> getUpMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getDownMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getRightMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getLeftMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getNEDiagMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getNWDiagMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getSWDiagMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getSEDiagMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getKnightMoveOffsets() {
        return new List<MoveOffset>();
    }

    public virtual List<MoveOffset> getSpecialMoveOffsets() {
        return new List<MoveOffset>();
    }
}
