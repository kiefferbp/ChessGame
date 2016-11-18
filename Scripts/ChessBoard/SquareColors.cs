using UnityEngine;
using System.Collections;

public class SquareColors : MonoBehaviour {

    public string whiteColor, blackColor;
	IEnumerator Start () {
        yield return new WaitForSeconds(0.01f);

        GameObject[] whiteSquares = GameObject.FindGameObjectsWithTag("WhiteSquare");
        GameObject[] blackSquares = GameObject.FindGameObjectsWithTag("BlackSquare");

        Renderer rend;
        Color white = getColor(whiteColor);
        Color black = getColor(blackColor);

        foreach (GameObject g in whiteSquares)
        {
            rend = g.GetComponent<Renderer>();
            rend.material.color = white;
        }

        foreach (GameObject g in blackSquares)
        {
            rend = g.GetComponent<Renderer>();
            rend.material.color = black;
        }
    }

    public Color getColor(string color)
    {
        Color c;
        switch(color.ToLower())
        {
            case "black":
                c = Color.black; break;
            case "blue":
                c = Color.blue; break;
            case "cyan":
                c = Color.cyan; break;
            case "gray":
                c = Color.gray; break;
            case "magenta":
                c = Color.magenta; break;
            case "red":
                c = Color.red; break;
            case "white":
                c = Color.white; break;
            case "yellow":
                c = Color.yellow; break;
            default:
                c = Color.clear; break;
        }
        return c;
    }
}
