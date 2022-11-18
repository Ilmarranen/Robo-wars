using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public Character occupyingCharacter;
    public PathNode cameFromNode;

    public Dictionary<string, GameObject> Highlights = new Dictionary<string, GameObject>();

    public GameObject nodeSprite;
    public GameManager gameManager;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;

        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public void SetHighlight(string highlightName, bool setState = true)
    {
        if (Highlights.ContainsKey(highlightName))
        {
            Highlights[highlightName].SetActive(setState);
        }
    }

    public bool GetHighlight(string highlightName)
    {
        if (Highlights.ContainsKey(highlightName))
        {
            return Highlights[highlightName].activeSelf;
        }
        else return false;
    }

    public void OnMouseDown()
    {
        //If current tile is part of possible route
        if (GetHighlight("TileHighlight"))
        {
            gameManager.PlayerTurn(x, y);
        }
    }

}
