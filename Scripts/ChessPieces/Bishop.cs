using UnityEngine;
using System.Collections;

public class Bishop : ChessPiece {
    private int[] moves;    // Move description

    void Start() {
        /*
         * Bishops only move diagonlly from their current positions.
         * {8, 8} indicates that a rook can move between 1 and 8 spaces
         * along its current row and between 1 and 8 spaces along its 
         * current column in the same move. 
         * */
                          //Row Col
        moves = new int[2] { 8,  8 };
    }

    override public int[] moveDescription() {
        return moves;
    }
}
