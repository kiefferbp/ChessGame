using UnityEngine;
using System.Collections.Generic;

public class Queen : ChessPiece {
    private const int MAX_MOVES = 7;

    private List<MoveOffset> upMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> downMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> rightMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> leftMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> NEDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> NWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SWDiagMoveOffsets = new List<MoveOffset>();
    private List<MoveOffset> SEDiagMoveOffsets = new List<MoveOffset>();

    void Start() {
        for (int i = 1; i <= MAX_MOVES; i++) {
            upMoveOffsets.Add(new MoveOffset(i, 0));
            downMoveOffsets.Add(new MoveOffset(-i, 0));
            rightMoveOffsets.Add(new MoveOffset(0, i));
            leftMoveOffsets.Add(new MoveOffset(0, -i));
            NEDiagMoveOffsets.Add(new MoveOffset(i, i));
            NWDiagMoveOffsets.Add(new MoveOffset(i, -i));
            SWDiagMoveOffsets.Add(new MoveOffset(-i, -i));
            SEDiagMoveOffsets.Add(new MoveOffset(-i, i));
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
