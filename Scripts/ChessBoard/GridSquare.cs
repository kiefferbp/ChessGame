using UnityEngine;
using System.Collections;

public class GridSquare : MonoBehaviour {
    private Player player;              // Player that owns square
    private ChessPiece piece;           // Piece on square
    private bool trigger, isActive;     // Square state - active, triggered
    private int row, col;               // Square's location in controller matrix
    private Color c;                    // Color of each square 
    private Renderer rn;                // Renderer - handles color changes

    void Start()
    {
        rn = GetComponent<Renderer>();
        player = null;
        piece = null;
        trigger = false;
        isActive = false;
        row = -1; col = -1;
        c = rn.material.color;
    }

    /// <summary>
    /// A square is considered active only when it's possible for it 
    /// to be used by the player. For instance, when the player begins 
    /// its turn, the player should only be allowed to select its own 
    /// pieces. Once a piece is chosen, then the player can only move
    /// that piece to its destination.  
    /// </summary>
    public void activate()
    {
        //markOpenPath();
        isActive = true;
    }

    public void deactivate()
    {
        resetColor();
        isActive = false;
        trigger = false;
    }

    /*
     * Checks if a square is currently in play, or triggerable
     * */
    public bool inPlay()
    {
        return isActive;
    }

    /*
     * Gets mouse click input. Only pieces in play can be triggered.
     * */
    void OnMouseDown()
    {
        if (inPlay())
            triggerSquare();
    }

    public void triggerSquare()
    {
        trigger = true; 
    }

    public bool isTriggered()
    {
        return trigger;
    }

    /* 
     * Resets the square once a player has moves its piece
     * */
    public void reset()
    {
        player = null;
        piece = null;
        trigger = false;
        isActive = false;
        resetColor();
    }

    /*
     * Handles square colors for making moves: 
     * Mark open path   - squares without enemy pieces
     * Mark enemy       - squares with enemy pieces
     * Reset color      - reset square color 
     * */
    public void markOpenPath()
    {
        rn.material.color = Color.green;
    }

    public void markEnemy()
    {
        rn.material.color = Color.red;
    }

    public void resetColor()
    {
        rn.material.color = c;
    }

    /*
     * Accessor functions
     * */
    public void setCor(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setPiece(ChessPiece piece) {
        this.piece = piece;
    }

    public int getRow()
    {
        return row;
    }

    public int getCol()
    {
        return col;
    }

    public Player getPlayer()
    {
        return player;
    }

    public ChessPiece getPiece() {
        return piece;
    }
}
