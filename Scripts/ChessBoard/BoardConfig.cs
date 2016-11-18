using UnityEngine;
using System.Collections;

public class BoardConfig : MonoBehaviour {
    private const float SQUARE_SCALE = 0.125f;      // 1/8 or .125 in relation to parent
    private const float STARTING_X = 0.4375f;      // -0.5 (left side of parent) + .125 (half square scale - offset)
    private const float STARTING_Y = 0.4375f;       // 0.5 (top side of parent) - .125 (half square scale - offset)

    public float squareDimension;
    public GameObject whiteSquare, blackSquare;
    public GameController controller;

    IEnumerator Start () {
        float gridDimension = squareDimension * 8;  // 8 squares in each column and row
        transform.localScale = new Vector3(gridDimension, gridDimension, 1.0f);

        // Start at the upper left corner 
        float x = STARTING_X;
        float y = STARTING_Y;
        GameObject square;
        for (int col = 0; col < 8; col++) {
            if (col % 2 != 0)               // odd numbered columns 
                square = whiteSquare;       // first square is white
            else                            // even numbered columns 
                square = blackSquare;       // first square is black

            for (int row = 0; row < 8; row++) {
                GameObject temp = Instantiate(square, transform) as GameObject;             // Create each square
                temp.transform.localRotation = Quaternion.identity;                         // Set rotation
                temp.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));                     // I dont like this
                temp.transform.localPosition = new Vector3(x, y, 0.0f);                     // Set position
                temp.transform.localScale = new Vector3(SQUARE_SCALE, SQUARE_SCALE, 1.0f);  // Set scale
                GridSquare g = temp.GetComponent<GridSquare>();                             // Get GridSquare              
                GameController.grid[row,col] = g;                                           // Add to controller matrix

                // Alternates squares going down each column
                if (square == whiteSquare)
                    square = blackSquare;
                else
                    square = whiteSquare;
                y -= SQUARE_SCALE; // Move down column
            }
            y = STARTING_Y;     // Reset to top of column 
            x -= SQUARE_SCALE;  // Move right to next column 
        }
        // Allow enough time for each square to finish Start() routine
        yield return new WaitForSeconds(0.01f);
        // Assign each square its row and column value
        for (int col = 0; col < 8; col++) {
            for (int row = 0; row < 8; row++) {
                GameController.grid[row, col].setCor(row, col);
            }
        }
    }
}
