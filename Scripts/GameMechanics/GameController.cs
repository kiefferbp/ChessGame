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
    private const float VEC_ZERO = 0.0f;
    private const float SCALE_X = 0.7f; // 0.7f   
    private const float SCALE_Y = 0.05f;  // 0.3f
    private const float SCALE_Z = 0.7f; // 0.7f
    /*
     * Public variables
     * */
    public static GridSquare[,] grid = new GridSquare[8, 8];
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

    IEnumerator Start() {
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

    void Update() {
        if (moveMade)
            whiteTurn = !whiteTurn;
    }

    public List<GridSquare> getMoves(GridSquare g) {
        ChessPiece piece = g.getPiece();
        int curRow = g.getRow();
        int curCol = g.getCol();
        List<GridSquare> possibleMoves = new List<GridSquare>();
        Dictionary<MoveType, List<MoveOffset>> offsetMap = new Dictionary<MoveType, List<MoveOffset>>();
        Player friendly = (whiteTurn ? whitePlayer : blackPlayer);
        Player enemy = (whiteTurn ? blackPlayer : whitePlayer);
        Player playerOfPiece = g.getPlayer();
        GridSquare candidateMove;
        Player candidatePlayer;

        // add the move types of the piece to the map
        offsetMap.Add(MoveType.UP, piece.getUpMoveOffsets());
        offsetMap.Add(MoveType.DOWN, piece.getDownMoveOffsets());
        offsetMap.Add(MoveType.RIGHT, piece.getRightMoveOffsets());
        offsetMap.Add(MoveType.LEFT, piece.getLeftMoveOffsets());
        offsetMap.Add(MoveType.DIAG_NE, piece.getNEDiagMoveOffsets());
        offsetMap.Add(MoveType.DIAG_NW, piece.getNWDiagMoveOffsets());
        offsetMap.Add(MoveType.DIAG_SW, piece.getSWDiagMoveOffsets());
        offsetMap.Add(MoveType.DIAG_SE, piece.getSEDiagMoveOffsets());

        // handle the knights separately for simplicitly
        if (piece is Knight) {
            foreach (MoveOffset offset in piece.getKnightMoveOffsets()) {
                int moveRow = curRow + offset.getRow();
                int moveCol = curCol + offset.getCol();

                if (boundaryCheck(moveRow, moveCol)) {
                    candidateMove = grid[moveRow, moveCol];
                    candidatePlayer = candidateMove.getPlayer();

                    if (candidatePlayer == null || candidatePlayer == enemy) {
                        possibleMoves.Add(candidateMove);
                    }

                    if (candidatePlayer == null) {
                        candidateMove.markOpenPath();
                    }

                    if (candidatePlayer == enemy) {
                        candidateMove.markEnemy();
                    }
                }
            }

            // we do not need to consider the other move types
            return possibleMoves;
        }

        // add pawn attacking moves to possibleMoves
        if (piece is Pawn) {
            // check the possible attacking pawn moves
            foreach (MoveOffset offset in piece.getSpecialMoveOffsets()) {
                int moveRow = curRow + offset.getRow();
                int moveCol = curCol + offset.getCol();

                // black pawns move down the board
                if (playerOfPiece == blackPlayer) {
                    // so we need to correct the row offset
                    moveRow = curRow - offset.getRow();
                }

                // add the offseted square to the list of possible moves if
                // it contains an enemy
                if (boundaryCheck(moveRow, moveCol)) {
                    candidateMove = grid[moveRow, moveCol];
                    candidatePlayer = candidateMove.getPlayer();

                    if (candidatePlayer == enemy) {
                        possibleMoves.Add(candidateMove);
                        candidateMove.markEnemy();
                    }
                }
            }
        }

        // iterate through the non-special move types
        foreach (KeyValuePair<MoveType, List<MoveOffset>> kvp in offsetMap) {
            MoveType moveType = kvp.Key;
            List<MoveOffset> possibleOffsets = kvp.Value;

            foreach (MoveOffset offset in possibleOffsets) {
                int moveRow = curRow + offset.getRow();
                int moveCol = curCol + offset.getCol();

                // black pawns move down the board
                if (piece is Pawn && playerOfPiece == blackPlayer) {
                    // so we need to correct the row offset
                    moveRow = curRow - offset.getRow();
                }

                if (!boundaryCheck(moveRow, moveCol)) { // invalid square
                    // All squares beyond this square are also invalid.
                    // This isn't true in the case of a knight, so it is
                    // considered separately.
                    break;
                }

                candidateMove = grid[moveRow, moveCol];
                candidatePlayer = candidateMove.getPlayer();

                if (candidatePlayer == null) {
                    possibleMoves.Add(candidateMove);
                    candidateMove.markOpenPath();
                } else if (candidatePlayer == enemy) {
                    // when moving upwards, everything except the pawn can take a piece
                    if (!(piece is Pawn)) {
                        possibleMoves.Add(candidateMove);
                        candidateMove.markEnemy();
                    }

                    break;
                } else { // friendly piece
                    break;
                }
            }
        }

        return possibleMoves;
    }

    public void makeMove(GridSquare from, GridSquare to) {

        Player enemy = whiteTurn ? blackPlayer : whitePlayer;
        Player player = !whiteTurn ? blackPlayer : whitePlayer;
        ChessPiece piece = from.getPiece();
        if (piece is Pawn) {
            ((Pawn)piece).markAsMoved();
        }
        if (to.getPlayer() == enemy) {
            ChessPiece enemyPiece = to.getPiece();
            Destroy(enemyPiece.gameObject);
            if (whiteTurn)
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
        } else {
            blackPieces.Remove(from);
            blackPieces.Add(to);
        }
        piece.gameObject.transform.parent = to.gameObject.transform;
        piece.gameObject.transform.localPosition = new Vector3(VEC_ZERO, VEC_ZERO, VEC_ZERO);
    }

    public bool boundaryCheck(int row, int col) {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    public List<GridSquare> getPieces() {
        if (whiteTurn)
            return whitePieces;
        return blackPieces;
    }

    /*
     * Tells if it's white player's turn
     * */
    public bool isWhiteTurn() {
        return whiteTurn;
    }

    /*
     * Sets squares in or out of play - determines if they can be 
     * triggered or ignored. 
     * */
    public void squaresInPlay(List<GridSquare> squares) {
        foreach (GridSquare g in squares) {
            g.activate();
        }
    }

    public void squaresOutOfPlay(List<GridSquare> squares) {
        foreach (GridSquare g in squares) {
            g.deactivate();
        }
    }

    /*
     * Player ends turn 
     * */
    public void endTurn() {
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
        for (int i = 0; i < whitePieces.Count; i++) {
            configureTransform(whitePieces[i].getPiece().gameObject.transform);
            configureTransform(blackPieces[i].getPiece().gameObject.transform);
        }
    }

    /*
     * Zero's out the position and re-scales.
     * */
    public void configureTransform(Transform t) {
        t.localPosition = Vector3.zero;
        t.localScale = new Vector3(SCALE_X, SCALE_Y, SCALE_Z);
        t.localRotation = Quaternion.identity;                  // I dont like this
        t.Rotate(new Vector3(-90.0f, VEC_ZERO, VEC_ZERO));      // I dont like this
    }
}
