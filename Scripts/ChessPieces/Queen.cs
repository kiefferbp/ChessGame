using UnityEngine;
using System.Collections;

public class Queen : ChessPiece {
    private int[] moves;    // Move description

    void Start() {
        /*
         * The queen moves in a similar manner to the King
         * but can move up to 8 spaces in any direction. 
         * */
        //Row Col
        moves = new int[6] { 0,  8,     // Column only 
                             8,  0,     // Row only
                             8,  8 };   // Diagonly 
    }

    override public int[] moveDescription() {
        return moves;
    }
}
