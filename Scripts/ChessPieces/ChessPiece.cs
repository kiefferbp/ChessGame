using UnityEngine;
using System.Collections;

abstract public class ChessPiece : MonoBehaviour {
    /*
     * Each chess piece was a description of how it can 
     * move. Decriptions are representded with 2 integer 
     * values: a row and column. These values indicate 
     * how many squares the piece the piece can from its
     * current square. Ex. { 1, 0 ) means that a piece 
     * can move 1 square along its current row and 0 
     * squares along its current column. Since some pieces
     * can move across the entire board, their moves are 
     * represented with a range. If a description contains,
     * an 8, this indicates a range from 1 to 8. Ex. { 8, 0 } 
     * means that a piece can from between 1 and 8 spaces along 
     * its current row and 0 spaces along its current column.
     * */  
    abstract public int[] moveDescription();
}
