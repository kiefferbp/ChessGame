using UnityEngine;
using System.Collections;

public class Rook : ChessPiece {
    private int[] moves;    // Move description

    void Start() {
        /*
         * Rooks can move 8 spaces either along the row it's on or 
         * 8 spaces along the column it's on. The decription gives 
         * ranges - ex. {0, 8} semantically means the piece can move 
         * between 1 and 8 spaces along the column.
         * */
                          //Row Col
        moves = new int[4] { 0,  8,
                             8,  0 };
    }

    override public int[] moveDescription() {
        return moves;
    }
}
