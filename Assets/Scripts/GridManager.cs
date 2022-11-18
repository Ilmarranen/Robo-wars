using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    private Pathfinding pathfinding;
    private Grid<PathNode> grid;
    public List<PathNode> sideNodes = new List<PathNode>();
    [SerializeField] private GameObject nodePrefab;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(width, height);
        grid = pathfinding.GetGrid();
        DrawGrid();
        //FillSideNodes();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DrawGrid()
    {
        float cellsize = grid.GetCellSize();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PathNode node = grid.GetGridObject(x, y);
                node.nodeSprite = Instantiate(nodePrefab, new Vector3((float)x + .5f, (float)y + .5f) * cellsize, Quaternion.identity);
                node.nodeSprite.name = "Tile" + x + y;
                node.Highlights.Add("Damage1", node.nodeSprite.transform.Find("Damage1").gameObject);
                node.Highlights.Add("Damage2", node.nodeSprite.transform.Find("Damage2").gameObject);
                node.Highlights.Add("Damage3", node.nodeSprite.transform.Find("Damage3").gameObject);
                node.Highlights.Add("TileHighlight", node.nodeSprite.transform.Find("TileHighlight").gameObject);
                node.Highlights.Add("SawGreen", node.nodeSprite.transform.Find("SawGreen").gameObject);
                node.Highlights.Add("SawPink", node.nodeSprite.transform.Find("SawPink").gameObject);
                node.Highlights.Add("SawRed", node.nodeSprite.transform.Find("SawRed").gameObject);
                node.Highlights.Add("PredictionGreen", node.nodeSprite.transform.Find("PredictionGreen").gameObject);
                node.Highlights.Add("PredictionGreen1", node.nodeSprite.transform.Find("PredictionGreen1").gameObject);
                node.Highlights.Add("PredictionGreen2", node.nodeSprite.transform.Find("PredictionGreen2").gameObject);
                node.Highlights.Add("PredictionGreen3", node.nodeSprite.transform.Find("PredictionGreen3").gameObject);
                node.Highlights.Add("PredictionPink", node.nodeSprite.transform.Find("PredictionPink").gameObject);
                node.Highlights.Add("PredictionPink1", node.nodeSprite.transform.Find("PredictionPink1").gameObject);
                node.Highlights.Add("PredictionPink2", node.nodeSprite.transform.Find("PredictionPink2").gameObject);
                node.Highlights.Add("PredictionPink3", node.nodeSprite.transform.Find("PredictionPink3").gameObject);
                node.Highlights.Add("PredictionRed", node.nodeSprite.transform.Find("PredictionRed").gameObject);
                node.Highlights.Add("PredictionRed1", node.nodeSprite.transform.Find("PredictionRed1").gameObject);
                node.Highlights.Add("PredictionRed2", node.nodeSprite.transform.Find("PredictionRed2").gameObject);
                node.Highlights.Add("PredictionRed3", node.nodeSprite.transform.Find("PredictionRed3").gameObject);

                node.Highlights.Add("Unwalkable", node.nodeSprite.transform.Find("Unwalkable").gameObject);
                node.Highlights.Add("Occupied", node.nodeSprite.transform.Find("Occupied").gameObject);

                PathNodeVisuals nodeVisuals = node.nodeSprite.GetComponent<PathNodeVisuals>();
                nodeVisuals.node = node;

                //node.damage1Highlight = node.nodeSprite.transform.Find("Damage1").gameObject;
                //node.damage2Highlight = node.nodeSprite.transform.Find("Damage2").gameObject;
                //node.damage3Highlight = node.nodeSprite.transform.Find("Damage3").gameObject;
                //node.TileHighlight = node.nodeSprite.transform.Find("TileHighlight").gameObject;
                //node.SawGreenHighlight = node.nodeSprite.transform.Find("SawGreen").gameObject;
                //node.SawPinkHighlight = node.nodeSprite.transform.Find("SawPink").gameObject;
                //node.SawRedHighlight = node.nodeSprite.transform.Find("SawRed").gameObject;
                //node.PredictionGreenHighlight = node.nodeSprite.transform.Find("PredictionGreen").gameObject;
                //node.PredictionPinkHighlight = node.nodeSprite.transform.Find("PredictionPink").gameObject;
                //node.PredictionRedHighlight = node.nodeSprite.transform.Find("PredictionRed").gameObject;
            }
        }
    }

    public void FillSideNodes()
    {
        sideNodes.Clear();

        //Left and right side
        for (int i = 0; i < height; i++)
        {
            PathNode node1 = pathfinding.GetNode(0, i);
            PathNode node2 = pathfinding.GetNode(width - 1, i);
            //Debug.Log(node1.x + "," + node1.y + "; " + node2.x + "," + node2.y);
            sideNodes.Add(node1);
            sideNodes.Add(node2);
        }

        //Up and down
        for (int i = 1; i < width - 1; i++)
        {
            PathNode node1 = pathfinding.GetNode(i, 0);
            PathNode node2 = pathfinding.GetNode(i, height - 1);
            //Debug.Log(node1.x + "," + node1.y + "; " + node2.x + "," + node2.y);
            sideNodes.Add(node1);
            sideNodes.Add(node2);
        }
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition, int characterType)
    {
        if (characterType == 2) pathfinding.SetDiagonalCost(21);
        List<Vector3> path = pathfinding.FindPath(startWorldPosition, endWorldPosition);
        if (characterType == 2) pathfinding.SetDiagonalCost(14);
        return path;
    }

    public Vector2 GetRandomSideCoordinates()
    {
        int randomIndex = Random.Range(0, sideNodes.Count);
        Vector2 result = new Vector2(sideNodes[randomIndex].x, sideNodes[randomIndex].y);
        sideNodes.RemoveAt(randomIndex);
        return result;
    }

    public void SetSpawnPrediction(GameObject spawnPrefab, Vector2 spawnPosition, CharacterColors spawnColor, bool value = true)
    {
        int spawnType = spawnPrefab.GetComponent<Character>().characterType;
        PathNode node = pathfinding.GetNode((int)spawnPosition.x, (int)spawnPosition.y);
        node.SetHighlight("Prediction"+spawnColor.ToString()+spawnType, value);
    }

    public void SetIsWalkable(Vector2 nodePosition, bool value = true)
    {
        PathNode node = pathfinding.GetNode((int)nodePosition.x, (int)nodePosition.y);
        //node.SetHighlight("Unwalkable", !value); //Debug
        node.isWalkable = value;
    }

    public void SetIsWalkable(Vector3 worldPosition, bool value = true)
    {
        grid.GetXY(worldPosition, out int x, out int y);
        SetIsWalkable(new Vector2(x, y), value);
    }

    public void SetIsWalkable(Character character = null)
    {
        PathNode node, nodeNeighbour;
        Character monster;

        ResetIsWalkable();

        node = grid.GetGridObject(gameManager.player.transform.position);
        node.isWalkable = false;
        //node.SetHighlight("Unwalkable", true); //Debug
        node.occupyingCharacter = gameManager.player.GetComponent<Character>();

        foreach(GameObject monsterObject in gameManager.monsters)
        {
            monster = monsterObject.GetComponent<Character>();
            node = grid.GetGridObject(monster.transform.position);
            node.isWalkable = false;
            //node.SetHighlight("Unwalkable", true); //Debug
            node.occupyingCharacter = monster;

            if(monster.characterType == 2 && monster != character) //saws don't count for monster with saws
            {
                nodeNeighbour = grid.GetGridObject(node.x-1, node.y);
                if(nodeNeighbour != null)
                {
                    nodeNeighbour.isWalkable = false;
                    //nodeNeighbour.SetHighlight("Unwalkable", true); //Debug
                    node.occupyingCharacter = monster;
                }
                nodeNeighbour = grid.GetGridObject(node.x+1, node.y);
                if (nodeNeighbour != null)
                {
                    nodeNeighbour.isWalkable = false;
                    //nodeNeighbour.SetHighlight("Unwalkable", true); //Debug
                    node.occupyingCharacter = monster;
                }
                nodeNeighbour = grid.GetGridObject(node.x, node.y-1);
                if (nodeNeighbour != null)
                {
                    nodeNeighbour.isWalkable = false;
                    //nodeNeighbour.SetHighlight("Unwalkable", true); //Debug
                    node.occupyingCharacter = monster;
                }
                nodeNeighbour = grid.GetGridObject(node.x, node.y+1);
                if (nodeNeighbour != null)
                {
                    nodeNeighbour.isWalkable = false;
                    //nodeNeighbour.SetHighlight("Unwalkable", true); //Debug
                    node.occupyingCharacter = monster;
                }
            }

        }
    }

    public void ResetIsWalkable()
    {
        PathNode node;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                node = pathfinding.GetNode(x, y);
                node.isWalkable = true;
                //node.SetHighlight("Unwalkable", false); //Debug
                node.occupyingCharacter = null;
            }
        }
    }

    public bool GetIsWalkable(Vector2 nodePosition)
    {
        PathNode node = pathfinding.GetNode((int)nodePosition.x, (int)nodePosition.y);
        return node.isWalkable;
    }

    public bool GetIsWalkable(Vector3 worldPosition)
    {
        grid.GetXY(worldPosition, out int x, out int y);
        return GetIsWalkable(new Vector2(x, y));
    }

    public void SetPossibleTurnHighlight(Card card)
    {
        CharacterColors predictionColor = card.cardColor;
        if (predictionColor == CharacterColors.None)
        {
            Character player = gameManager.player.GetComponent<Character>();
            predictionColor = player.characterColor;
        }

        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                bool directionPossible = card.cardMap[j].row[i];
                //if (directionPossible) Debug.Log("Direction: "+i + "," + j);
                if (directionPossible) SetDirectionHighlight(i, j, predictionColor);
            }
        }
    }

    public void SetDirectionHighlight(int xDirection, int yDirection, CharacterColors characterColor)
    {
        //0 = -1 1 = 0 2 = +1
        xDirection -= 1;
        yDirection -= 1;

        grid.GetXY(gameManager.player.transform.position, out int xBegin, out int yBegin);

        int x = xBegin, y = yBegin;
        PathNode node;
        Character player = gameManager.player.GetComponent<Character>();

        while (x >= 0 && y >= 0 && x < width && y < height)
        {
            node = pathfinding.GetNode(x, y);

            //Node occupied by monster of different color - finish route
            if (node.occupyingCharacter != null && node.occupyingCharacter != player && node.occupyingCharacter.characterColor != characterColor) break;

            node.SetHighlight("TileHighlight");

            //Node occupied by monster of same color - finish route, but include current node
            if (node.occupyingCharacter != null && node.occupyingCharacter != player && node.occupyingCharacter.characterColor == characterColor) break;

            //No increment chosen - only starting node is selected
            if (xDirection == 0 && yDirection == 0) break;

            x = x + xDirection;
            y = y + yDirection;

            //Debug.Log(x + "," + y);
        }
    }

    public void ClearPossibleTurnHighLight()
    {
        PathNode node;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                node = pathfinding.GetNode(x, y);
                node.SetHighlight("TileHighlight", false);
            }
        }
    }

    public void SetOccupyingCharacter(Vector3 nodePosition, Character character)
    {
        PathNode node = grid.GetGridObject(nodePosition);
        node.SetHighlight("Occupied", character != null); //Debug
        node.occupyingCharacter = character;
    }

    public void SetDamageHighlight()
    {
        int[,] damageArray = new int[width, height];
        Character monster;
        PathNode node;
        int xBegin, yBegin, xCurrent, yCurrent;

        ClearDamageHighlight();

        //Predict damage
        foreach (GameObject monsterObject in gameManager.monsters)
        {
            monster = monsterObject.GetComponent<Character>();
            grid.GetXY(monsterObject.transform.position, out xBegin, out yBegin);
            xCurrent = xBegin;
            yCurrent = yBegin;
            switch (monster.characterType)
            {
                case 1:
                    {
                        ModifyArrayByDirection(xBegin, yBegin, -1, 0, damageArray); //left
                        ModifyArrayByDirection(xBegin, yBegin, 1, 0, damageArray); //right
                        ModifyArrayByDirection(xBegin, yBegin, 0, 1, damageArray); //up
                        ModifyArrayByDirection(xBegin, yBegin, 0, -1, damageArray); //down
                        break;
                    }
                case 3:
                    {
                        ModifyArrayByDirection(xBegin, yBegin, -1, 1, damageArray, 1); //up left
                        ModifyArrayByDirection(xBegin, yBegin, 0, 1, damageArray, 1); //up
                        ModifyArrayByDirection(xBegin, yBegin, 1, 1, damageArray, 1); //up right
                        ModifyArrayByDirection(xBegin, yBegin, 1, 0, damageArray, 1); //right
                        ModifyArrayByDirection(xBegin, yBegin, 1, -1, damageArray, 1); //down right
                        ModifyArrayByDirection(xBegin, yBegin, 0, -1, damageArray, 1); //down
                        ModifyArrayByDirection(xBegin, yBegin, -1, -1, damageArray, 1); //down left
                        ModifyArrayByDirection(xBegin, yBegin, -1, 0, damageArray, 1); //left
                        break;
                    }
            }
        }

        //Draw highlight
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int predictedDamage = damageArray[x, y];
                predictedDamage = predictedDamage > 3 ? 3 : predictedDamage;
                if (predictedDamage > 0)
                {
                    node = pathfinding.GetNode(x, y);
                    node.SetHighlight("Damage" + predictedDamage);
                }

            }
        }

        //Print2DArray<int>(damageArray);
    }

    public void ClearDamageHighlight()
    {
        PathNode node;
        //Clear highlights
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                node = pathfinding.GetNode(x, y);
                node.SetHighlight("Damage1", false);
                node.SetHighlight("Damage2", false);
                node.SetHighlight("Damage3", false);
            }
        }
    }

    public static void Print2DArray<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            string row = "";
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                row += matrix[j, i] + " ";
            }
            Debug.Log(row);
        }
    }

    void ModifyArrayByDirection(int xBegin, int yBegin, int xDirection, int yDirection, int[,] directionArray, int stepsNumber = 0)
    {
        int xCurrent = xBegin, yCurrent = yBegin, currentStep = 0;

        while (xCurrent >= 0 && yCurrent >= 0 && xCurrent < width && yCurrent < height)
        {
            if (xCurrent != xBegin || yCurrent != yBegin)
            {
                directionArray[xCurrent, yCurrent] += 1;
            }
            xCurrent += xDirection;
            yCurrent += yDirection;

            if (stepsNumber != 0 && currentStep == stepsNumber) break;

            currentStep++;
        }
    }

    public int GetDamage(Vector3 nodePosition)
    {
        PathNode node = grid.GetGridObject(nodePosition);
        if (node.GetHighlight("Damage3")) return 3;
        if (node.GetHighlight("Damage2")) return 2;
        if (node.GetHighlight("Damage1")) return 1;
        return 0;
    }

    public Vector3 GetGridCenter()
    {
        int x = Mathf.FloorToInt(width / 2);
        int y = Mathf.FloorToInt(height / 2);
        float cellsize = grid.GetCellSize();
        return new Vector3((x + .5f) * cellsize, (y + .5f) * cellsize);
    }

    public void SetSawHighlight()
    {
        int[,] sawArray = new int[width, height];
        int xBegin, yBegin;
        PathNode node;
        Character monster, character;
        List<GameObject> objectsToDamage = new List<GameObject>();

        ClearSawHighlight();

        foreach (GameObject monsterObject in gameManager.monsters)
        {
            monster = monsterObject.GetComponent<Character>();
            if (monster.characterType != 2) continue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    sawArray[x, y] = 0;
                }
            }

            grid.GetXY(monster.transform.position, out xBegin, out yBegin);
            ModifyArrayByDirection(xBegin, yBegin, -1, 0, sawArray, 1); //left
            ModifyArrayByDirection(xBegin, yBegin, 1, 0, sawArray, 1); //right
            ModifyArrayByDirection(xBegin, yBegin, 0, 1, sawArray, 1); //up
            ModifyArrayByDirection(xBegin, yBegin, 0, -1, sawArray, 1); //down

            //Draw highlight
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (sawArray[x, y] > 0)
                    {
                        node = pathfinding.GetNode(x, y);
                        node.SetHighlight("Saw" + monster.characterColor.ToString());
                        //SetIsWalkable(new Vector2(x,y),false);
                        if(node.occupyingCharacter != null) objectsToDamage.Add(node.occupyingCharacter.gameObject);
                        //Debug.Log(monster.characterColor.ToString());
                    }

                }
            }

        }

        foreach(GameObject characterObject in objectsToDamage)
        {
            character = characterObject.GetComponent<Character>();
            if(character.characterType == 0)
            {
                
            }
            else
            {
                gameManager.DestroyMonster(characterObject);
            }

        }

        //Print2DArray<int>(damageArray);
    }

    public void ClearSawHighlight()
    {
        PathNode node;
        //Clear highlights
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                node = pathfinding.GetNode(x, y);
                if (node.GetHighlight("SawGreen"))
                {
                    node.SetHighlight("SawGreen", false);
                    //SetIsWalkable(new Vector2(x, y));
                }
                if (node.GetHighlight("SawRed"))
                {
                    node.SetHighlight("SawRed", false);
                    //SetIsWalkable(new Vector2(x, y));
                }
                if (node.GetHighlight("SawPink"))
                {
                    node.SetHighlight("SawPink", false);
                    //SetIsWalkable(new Vector2(x, y));
                }
            }
        }
    }
}
