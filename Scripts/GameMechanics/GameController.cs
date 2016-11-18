using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Container class for the chess piece prefabs
 * Made to declutter inspector view 
 * */
[System.Serializable]
public class PiecePrefabs {
    public GameObject wBishop, wKing, wKnight, wPawn, wQueen, wRook,
        bBishop, bKing, bKnight, bPawn, bQueen, bRook;
}

public class GameController : MonoBehaviour {
    /*
     * Constants
     * */
    private const float VEC_ZERO    = 0.0f;
    private const float SCALE_X     = 0.7f;     
    private const float SCALE_Y     = 0.3f;
    private const float SCALE_Z     = 0.7f;
    /*
     * Public variables
     * */
    public static GridSquare[,] grid = new GridSquare[8,8];
    public GameObject wPlayer, bPlayer;                         // Player Gameobjects
    public GameObject chessBoard;                               // Chess board Gameobjects
    public PiecePrefabs piecePrefabs;                          // Chess piece prefabs
    /*
     * Private variables
     * */
    private Player whitePlayer, blackPlayer;                    // Player scripts 
    private List<GridSquare> whitePieces, blackPieces;          // Player pieces 
    private bool whiteTurn, moveMade;

    enum Column { A, B, C, D, E, F, G, H };                     // Enumerated Columns

    IEnumerator Start()
    {
        // Gets player script components 
        whitePlayer = wPlayer.GetComponent<Player>();
        blackPlayer = bPlayer.GetComponent<Player>();
        // Initialize lists 
        whitePieces = new List<GridSquare>();
        blackPieces = new List<GridSquare>();

        whiteTurn = true;   // White player always starts 
        moveMade = false;
  
        yield return new WaitForSeconds(0.01f);
        // Initialize pieces
        initializePieces();
        // Set white in play
        squaresInPlay(whitePieces);
    }

    void Update()
    {
        if (moveMade)
            whiteTurn = !whiteTurn;
    }


    public List<GridSquare> getMoves(GridSquare g)
    {
        ChessPiece piece = g.getPiece();
        List<GridSquare> possibleMoves = new List<GridSquare>();
        int curRow = g.getRow();
        int curCol = g.getCol();
        //int[] moveDescriptions = piece.moveDescription();
        int checkRow, checkCol;
        GridSquare candidate;

        

        Player enemy = whiteTurn ? blackPlayer : whitePlayer;
        Player player = !whiteTurn ? blackPlayer : whitePlayer;
        int direction = whiteTurn ? 1 : -1;

        if (piece is Pawn) {
            Debug.Log("PawnSelected!");
            checkRow = curRow + direction;
            checkCol = curCol + 1;
            if (boundaryCheck(checkRow, checkCol)) {
                candidate = grid[checkRow, checkCol];
                if (candidate.getPlayer() == enemy)
                    possibleMoves.Add(candidate);
            } // Checks one diagonal
            checkCol = curCol - 1;
            if (boundaryCheck(checkRow, checkCol)) {
                candidate = grid[checkRow, checkCol];
                if (candidate.getPlayer() == enemy)
                    possibleMoves.Add(candidate);
            } // Checks other diagonal
            if (((Pawn)piece).hasNotMovedAsOfYet()) {                    // I do not like this :P
                possibleMoves.Add(grid[curRow + direction, curCol]);
                possibleMoves.Add(grid[curRow + 2 * direction, curCol]);
                ((Pawn)piece).move();
            }
            else if (grid[curRow + direction, curCol].getPlayer() != enemy)
                possibleMoves.Add(grid[curRow + direction, curCol]);
            else { }
        }
        else if (piece is Knight) {
            Debug.Log("KnightSelected!");
            Debug.Log(curRow + " "  + curCol);
            knightMoves(curRow, curCol, possibleMoves, player);
        }
        else if (piece is Rook) {
            Debug.Log("RookSelected!");
            Debug.Log(curRow + " " + curCol);
            goDownCol(curRow, curCol, possibleMoves, enemy);
            goDownRow(curRow, curCol, possibleMoves, enemy);
        }
        else if (piece is Bishop) {
            Debug.Log("BishopSelected!");
            goDownDiag(curRow, curCol, possibleMoves, enemy);
        }
        else if (piece is Queen) {
            Debug.Log("QueenSelected!");
            goDownCol(curRow, curCol, possibleMoves, enemy);
            goDownRow(curRow, curCol, possibleMoves, enemy);
            goDownDiag(curRow, curCol, possibleMoves, enemy);
        }

        foreach(GridSquare blah in possibleMoves) {
            if (blah.getPlayer() == enemy) {
                blah.markEnemy();
            }
            else
                blah.markOpenPath();
        }

        return possibleMoves;
    }

    public void makeMove(GridSquare from, GridSquare to) {

        Player enemy = whiteTurn ? blackPlayer : whitePlayer;
        Player player = !whiteTurn ? blackPlayer : whitePlayer;
        ChessPiece piece = from.getPiece();
        if(to.getPlayer() == enemy) {
            ChessPiece enemyPiece = to.getPiece();
            Destroy(enemyPiece.gameObject);
            if(whiteTurn)
                blackPieces.Remove(to);
            else
                whitePieces.Remove(to);
        }
        from.reset();
        to.reset();
        to.setPiece(piece);
        to.setPlayer(player);
        if (whiteTurn) {
            whitePieces.Remove(from);
            whitePieces.Add(to);
        }
        else {
            blackPieces.Remove(from);
            blackPieces.Add(to);
        }
        piece.gameObject.transform.parent = to.gameObject.transform;
        piece.gameObject.transform.localPosition = new Vector3(VEC_ZERO, VEC_ZERO, VEC_ZERO);
    }

    /*
     * Helper for move gernator
     * */
    public void goDownRow(int row, int col, List<GridSquare> moves, Player enemy) {
        GridSquare candidate;
        Player p;
        for (int i = col + 1; i < 8; i++) {
            candidate = grid[row, i];
            p = candidate.getPlayer();
            if (p == null) 
                moves.Add(candidate);
            else if (p == enemy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        }
        for (int i = col - 1; i >= 0; i--) {
            candidate = grid[row, i];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enemy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        }
    }

    public void goDownCol(int row, int col, List<GridSquare> moves, Player enermy) {
        GridSquare candidate;
        Player p;
        for (int i = row + 1; i < 8; i++) {
            candidate = grid[i, col];
            p = candidate.getPlayer();
            Debug.Log(p == null);
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        }
        for (int i = row - 1; i >= 0; i--) {
            candidate = grid[i, col];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        }
    }

    public void goDownDiag(int row, int col, List<GridSquare> moves, Player enermy) {
        GridSquare candidate;
        Player p;

        for (int i = 1; i < 8; i++) {               
            if (!boundaryCheck(row + i, col + i))
                break;
            candidate = grid[row + i, col + i];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        } // Upper Right
        for (int i = 1; i < 8; i++) {
            if (!boundaryCheck(row + i, col - i))
                break;
            candidate = grid[row + i, col - i];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        } // Upper Left
        for (int i = 1; i < 8; i++) {
            if (!boundaryCheck(row - i, col + i))
                break;
            candidate = grid[row - i, col + i];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        } // Lower Right
        for (int i = 1; i < 8; i++) {
            if (!boundaryCheck(row - i, col - i))
                break;
            candidate = grid[row - i, col - i];
            p = candidate.getPlayer();
            if (p == null)
                moves.Add(candidate);
            else if (p == enermy) {
                moves.Add(candidate);
                break;
            }
            else
                break;
        } // Lower Left
    }

    public void knightMoves(int row, int col, List<GridSquare> moves, Player player) {
        GridSquare candidate;
        if (boundaryCheck(row + 2, col + 1)) {
            candidate = grid[row + 2, col + 1];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row + 2, col - 1)) {
            candidate = grid[row + 2, col - 1];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row - 2, col + 1)) {
            candidate = grid[row - 2, col + 1];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row - 2, col - 1)) {
            candidate = grid[row - 2, col - 1];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row + 1, col + 2)) {
            candidate = grid[row + 1, col + 2];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row + 1, col - 2)) {
            candidate = grid[row + 1, col - 2];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row - 1, col + 2)) {
            candidate = grid[row - 1, col + 2];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
        if (boundaryCheck(row - 1, col - 2)) {
            candidate = grid[row - 1, col - 2];
            if (candidate.getPlayer() != player)
                moves.Add(candidate);
        }
    }

    

    public bool boundaryCheck(int row, int col) {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    public List<GridSquare> getPieces()
    {
        if (whiteTurn)
            return whitePieces;
        return blackPieces;
    }

    /*
     * Tells if it's white player's turn
     * */
    public bool isWhiteTurn()
    {
        return whiteTurn;
    }

    /*
     * Sets squares in or out of play - determines if they can be 
     * triggered or ignored. 
     * */
    public void squaresInPlay(List<GridSquare> squares)
    {
        foreach(GridSquare g in squares)
        {
            g.activate();
        }
    }

    public void squaresOutOfPlay(List<GridSquare> squares)
    {
        foreach (GridSquare g in squares)
        {
            g.deactivate();
        }
    }

    /*
     * Player ends turn 
     * */
    public void endTurn()
    {
        whiteTurn = !whiteTurn;
    }

    /*
     * Instantiates all chess pieces and sets to proper players.
     * */
    public void initializePieces() {
        // Create pawns
        for (int i = 0; i < 8; i++) {
            grid[1, i].setPiece((Instantiate(piecePrefabs.wPawn, grid[1, i].transform) 
                as GameObject).GetComponent<ChessPiece>());     // Instantiate white pawn and set
            grid[6, i].setPiece((Instantiate(piecePrefabs.bPawn, grid[6, i].transform) 
                as GameObject).GetComponent<ChessPiece>());     // Instantiate black pawn and set
        }
        // Create rooks
        grid[0, 0].setPiece((Instantiate(piecePrefabs.wRook, grid[0, 0].transform)
                as GameObject).GetComponent<ChessPiece>());     // White rook 1
        grid[0, 7].setPiece((Instantiate(piecePrefabs.wRook, grid[0, 7].transform)
            as GameObject).GetComponent<ChessPiece>());         // White rook 2
        grid[7, 0].setPiece((Instantiate(piecePrefabs.bRook, grid[7, 0].transform)
                as GameObject).GetComponent<ChessPiece>());     // Black rook 1
        grid[7, 7].setPiece((Instantiate(piecePrefabs.bRook, grid[7, 7].transform)
            as GameObject).GetComponent<ChessPiece>());         // Black rook 2
        // Create knights 
        grid[0, 1].setPiece((Instantiate(piecePrefabs.wKnight, grid[0, 1].transform)
                as GameObject).GetComponent<ChessPiece>());     // White knight 1
        grid[0, 6].setPiece((Instantiate(piecePrefabs.wKnight, grid[0, 6].transform)
            as GameObject).GetComponent<ChessPiece>());         // White knight 2
        grid[7, 1].setPiece((Instantiate(piecePrefabs.bKnight, grid[7, 1].transform)
                as GameObject).GetComponent<ChessPiece>());     // Black knight 1
        grid[7, 6].setPiece((Instantiate(piecePrefabs.bKnight, grid[7, 6].transform)
            as GameObject).GetComponent<ChessPiece>());         // Black knight 2
        // Create bishops
        grid[0, 2].setPiece((Instantiate(piecePrefabs.wBishop, grid[0, 2].transform)
                as GameObject).GetComponent<ChessPiece>());     // White bishop 1
        grid[0, 5].setPiece((Instantiate(piecePrefabs.wBishop, grid[0, 5].transform)
            as GameObject).GetComponent<ChessPiece>());         // White bishop 2
        grid[7, 2].setPiece((Instantiate(piecePrefabs.bBishop, grid[7, 2].transform)
                as GameObject).GetComponent<ChessPiece>());     // Black bishop 1
        grid[7, 5].setPiece((Instantiate(piecePrefabs.bBishop, grid[7, 5].transform)
            as GameObject).GetComponent<ChessPiece>());         // Black bishop 2
        // Create King and Queen
        grid[0, 3].setPiece((Instantiate(piecePrefabs.wQueen, grid[0, 3].transform)
                as GameObject).GetComponent<ChessPiece>());     // White Queen
        grid[0, 4].setPiece((Instantiate(piecePrefabs.wKing, grid[0, 4].transform)
            as GameObject).GetComponent<ChessPiece>());         // White King
        grid[7, 3].setPiece((Instantiate(piecePrefabs.bQueen, grid[7, 3].transform)
                as GameObject).GetComponent<ChessPiece>());     // Black Queen
        grid[7, 4].setPiece((Instantiate(piecePrefabs.bKing, grid[7, 4].transform)
            as GameObject).GetComponent<ChessPiece>());         // Black King
        /*
         * Assign players and add pieces to lists
         * */
        for (int i = 0; i < 8; i++) {
            // Assign Players to squares
            grid[0, i].setPlayer(whitePlayer); 
            grid[1, i].setPlayer(whitePlayer); 
            grid[6, i].setPlayer(blackPlayer);
            grid[7, i].setPlayer(blackPlayer);
            // Add pieces to player lists
            whitePieces.Add(grid[0, i]);
            whitePieces.Add(grid[1, i]);
            blackPieces.Add(grid[6, i]);
            blackPieces.Add(grid[7, i]);
        }
        /*
         * Reset positions and re-scale
         * */
        for(int i = 0; i < whitePieces.Count; i++) {
            configureTransform(whitePieces[i].getPiece().gameObject.transform);
            configureTransform(blackPieces[i].getPiece().gameObject.transform);
        } 
    }

    /*
     * Zero's out the position and re-scales.
     * */
    public void configureTransform(Transform t) {
        t.localPosition = new Vector3(VEC_ZERO, VEC_ZERO, VEC_ZERO);
        t.localScale = new Vector3(SCALE_X, SCALE_Y, SCALE_Z);
        t.localRotation = Quaternion.identity;                  // I dont like this
        t.Rotate(new Vector3(-90.0f, VEC_ZERO, VEC_ZERO));      // I dont like this
    }
}
