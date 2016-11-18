using UnityEngine;
using System.Collections;

public class Knight : ChessPiece {
    private int[] moves;    // Move description

    void Start() {
        /*
         * Knights can move in "L"-shapes. They can move 2 spaces 
         * along a row or column, then 1 space perpendicularly.
         * */
                          //Row Col
        moves = new int[4] { 2,  1,
                             1,  2};
    }

    override public int[] moveDescription() {
        return moves;
    }
}
