using UnityEngine;
using System.Collections;

public class Pawn : ChessPiece {

    private bool openingMove;       // Pawns can move two spaces only at opening move
    private int[] openingMoves;     // Move description of opening move
    private int[] moves;            // Move description after opening move

	void Start () {
        /*
         * Pawns can only move along the columns. They can move 
         * 1 space at a given turn or 2 at their opening move.
         * */
                                    //Row Col
        openingMoves    = new int[4] { 0,  1,
                                       0,  2 };
        moves           = new int[2] { 0,  1 };
        openingMove = true;
    }

    override public int[] moveDescription() {
        if(openingMove) {
            openingMove = false;     
            return openingMoves;
        }
        return moves;
    }

    public void move() {
        openingMove = false;
    }

    public bool hasNotMovedAsOfYet() {
        return openingMove;
    }
}
