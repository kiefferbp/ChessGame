using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bishop : ChessPiece {
    private const int MAX_DIAG = 7;

    private List<MoveOffset> NEDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> NWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SEDiagMoveOffsets = new List<MoveOffset>();

    void Start() {
        // bishops can move between 1 and 7 spaces diagonally
        for (int i = 1; i <= MAX_DIAG; i++) {
            // append the ability to move i spaces along each diagonal to the move lists
            NEDiagMoveOffsets.Add(new MoveOffset(i, i));
            NWDiagMoveOffsets.Add(new MoveOffset(i, -i));
            SWDiagMoveOffsets.Add(new MoveOffset(-i, -i));
            SEDiagMoveOffsets.Add(new MoveOffset(-i, i));
        }
    }

    override public List<MoveOffset> getNEDiagMoveOffsets() {
        return NEDiagMoveOffsets;
    }

    override public List<MoveOffset> getNWDiagMoveOffsets() {
        return NWDiagMoveOffsets;
    }

    override public List<MoveOffset> getSWDiagMoveOffsets() {
        return SWDiagMoveOffsets;
    }

    override public List<MoveOffset> getSEDiagMoveOffsets() {
        return SEDiagMoveOffsets;
    }
}
