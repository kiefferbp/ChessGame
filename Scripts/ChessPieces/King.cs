using UnityEngine;
using System.Collections.Generic;

public class King : ChessPiece {
    private List<MoveOffset> upMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> downMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> rightMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> leftMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> NEDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> NWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SEDiagMoveOffsets = new List<MoveOffset>();

    void Start() {
        upMoveOffsets.Add(new MoveOffset(1, 0));
        downMoveOffsets.Add(new MoveOffset(-1, 0));
        rightMoveOffsets.Add(new MoveOffset(0, 1));
        leftMoveOffsets.Add(new MoveOffset(0, -1));
        NEDiagMoveOffsets.Add(new MoveOffset(1, 1));
        NWDiagMoveOffsets.Add(new MoveOffset(1, -1));
        SWDiagMoveOffsets.Add(new MoveOffset(-1, -1));
        SEDiagMoveOffsets.Add(new MoveOffset(-1, 1));
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
