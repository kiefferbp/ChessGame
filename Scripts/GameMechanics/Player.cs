using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public GameController controller;           // Game controller
    public bool whitePlayer;                    // White player or black (not white)

    /*
     * At the player's turn, there are two different
     * states - choosing a piece to move and making a move. 
     * When the player is choosing a piece, the list of pieces
     * needs to be checked for a triggered piece, indicating a 
     * selection. When the player is making a move, the list of
     * possible moves needs to be checked for a triggered square, 
     * indicating a movement.
     * */
    private bool makingMove, newTurn;
    private List<GridSquare> pieces, moves;
    private GridSquare selectedPiece;

    void Start()
    {
        makingMove = false;
        newTurn = true;
        pieces = null;
        moves = null;
        selectedPiece = null;
    }

    void Update()
    {
        if(myTurn())
        {
            if (!makingMove)
            {
                if(newTurn) // Start of the new turn 
                {
                    pieces = controller.getPieces();    // Get players pieces
                    controller.squaresInPlay(pieces);   // Pieces are in play
                    newTurn = false;              
                }
                /*
                 * Check for a piece to be selected by the player
                 * */
                foreach (GridSquare g in pieces)
                {
                    if (g.isTriggered())    // Piece has been selected
                    {
                        selectedPiece = g;                      // Piece to be moved 
                        moves = controller.getMoves(g);         // Find where the piece can move
                        if (moves.Count > 0) {
                            makingMove = true;                      // Switch to move state
                            controller.squaresOutOfPlay(pieces);    // Pieces no longer in play
                            controller.squaresInPlay(moves);        // Move places in play
                        }
                        else
                            selectedPiece.deactivate();
                        break;
                    }
                }
            }
            else    //  Move state
            {
                /*
                 * Check where to selected piece can move
                 * */
                foreach (GridSquare g in moves)
                {
                    if (g.isTriggered())    // Move has been chosen
                    {
                        controller.makeMove(selectedPiece, g);       // Make move
                        // Set values for next turn;
                        controller.squaresOutOfPlay(moves);
                        makingMove = false;
                        newTurn = true;
                        controller.endTurn();           // End turn  
                    }
                } // end foreach
            } // end if(!makingMove)
        } // end if(myTurn())
    } // end Update()

    public bool isWhitePlayer()
    {
        return whitePlayer;
    }

    public bool myTurn()
    {
        return controller.isWhiteTurn() && whitePlayer ||
            !controller.isWhiteTurn() && !whitePlayer;
    }
}
