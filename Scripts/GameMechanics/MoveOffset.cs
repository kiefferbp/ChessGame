using UnityEngine;
using System.Collections;

public class MoveOffset {
    private int row;
    private int col;

	public MoveOffset(int row, int col) {
        this.row = row;
        this.col = col;
    }

    public int getRow() {
        return row;
    }

    public int getCol() {
        return col;
    }
}
