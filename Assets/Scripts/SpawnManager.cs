using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private float cellsize = 5;
    public GameManager gameManager;
    public GridManager gridManager;

    //Starting position
    [SerializeField] Spawner player;
    [SerializeField] private List<Spawner> monstersToSpawn;

    //Algorithm for per turn monster spawn
    [System.Serializable]
    class MonsterSpawnArray
    {
        public Spawner[] monsterSpawner;
    }
    [SerializeField] MonsterSpawnArray[] monstersSpawnOnTurn;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn player
        gameManager.player = SpawnCharacter(player);

        //Spawn staring monsters
        SpawnMonsters();
        PredictMonstersToSpawn();
    }

    public void PredictMonstersToSpawn()
    {
        gridManager.FillSideNodes();

        int spawnCycle = gameManager.turnNumber % monstersSpawnOnTurn.Length;
        spawnCycle = spawnCycle == 0 ? monstersSpawnOnTurn.Length : spawnCycle;
        //Debug.Log("Spawn cycle: " + spawnCycle);
        foreach(Spawner monster in monstersSpawnOnTurn[spawnCycle - 1].monsterSpawner)
        {
            //Random.InitState(System.DateTime.Now.Millisecond);
            Vector2 spawnPosition = monster.spawnPosition == Vector2.zero ?  gridManager.GetRandomSideCoordinates() : monster.spawnPosition;
            CharacterColors spawncolor = monster.spawnColor == CharacterColors.None ? (CharacterColors)Random.Range(1, 4) : monster.spawnColor;
            monstersToSpawn.Add(new Spawner(monster.spawnPrefab, spawnPosition, spawncolor));
            gridManager.SetSpawnPrediction(monster.spawnPrefab,spawnPosition, spawncolor);
            //Debug.Log("Predicting: " + monster.spawnPrefab.name + " {" + spawnPosition.x  + "," + spawnPosition.y + "}");
        }
    }

    public void SpawnMonsters()
    {
        foreach(Spawner monster in monstersToSpawn)
        {
            //Debug.Log(gridManager.GetIsWalkable(monster.spawnPosition));
            if (gridManager.GetIsWalkable(monster.spawnPosition))
            {
                gameManager.monsters.Add(SpawnCharacter(monster));
                //gridManager.SetIsWalkable(monster.spawnPosition, false);
            }
            else
            {
                gameManager.AddKills(1);
            }

            gridManager.SetSpawnPrediction(monster.spawnPrefab,monster.spawnPosition, monster.spawnColor, false);
        }
        //gridManager.SetSawHighlight();
        monstersToSpawn.Clear();
    }

    GameObject SpawnCharacter(Spawner spawner)
    {
        return SpawnCharacter(spawner.spawnPrefab, spawner.spawnPosition, spawner.spawnColor);
    }

    GameObject SpawnCharacter(GameObject characterPrefab, Vector2 gridPosition, CharacterColors characterColor)
    {
        GameObject characterObject = Instantiate(characterPrefab, new Vector3(gridPosition.x * cellsize + cellsize / 2, gridPosition.y * cellsize + cellsize / 2, 0), Quaternion.identity);
        Character character = characterObject.GetComponent<Character>();
        character.characterColor = characterColor;
        character.SetHighlight(characterColor, true, character.characterType == 2);
        //gridManager.SetOccupyingCharacter(characterObject.transform.position, character);

        return characterObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    class Spawner
    {
        public GameObject spawnPrefab;
        public CharacterColors spawnColor;
        public Vector2 spawnPosition;

        public Spawner(GameObject spawnPrefab, Vector2 spawnPosition, CharacterColors spawnColor)
        {
            this.spawnPrefab = spawnPrefab;
            this.spawnPosition = spawnPosition;
            this.spawnColor = spawnColor;
        }
    }
}
