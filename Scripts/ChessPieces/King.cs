using UnityEngine;
using System.Collections;

public class King : ChessPiece {
    private int[] moves;    // Move description

    void Start() {
        /*
         * The King can only move 1 square in any direction 
         * from its current position. 
         * */
                          //Row Col
        moves = new int[6] { 0,  1,     // Column only
                             1,  0,     // Row only
                             1,  1 };   // Diagonal
    }

    override public int[] moveDescription() {
        return moves;
    }
}
