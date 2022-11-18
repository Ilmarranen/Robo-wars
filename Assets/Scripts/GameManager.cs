using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DeckManager deckManager;
    public SpawnManager spawnManager;
    public GridManager gridManager;
    public int turnNumber = 1;
    [SerializeField] int turnCounter = 3, maxTurnCounter = 3;
    [SerializeField] int currentPlayerHP = 3, maxPlayerHp = 3;
    [SerializeField] int currentKills = 0, maxKills = 6; 
    [SerializeField] TMP_Text turnCounterText, playerHPText, monsterKillsText;
    [SerializeField] GameObject victoryPanel, defeatPanel;
    public GameObject player;
    public List<GameObject> monsters;
    private float cellsize = 5;

    // Start is called before the first frame update
    void Start()
    {
        gridManager.SetIsWalkable();
        gridManager.SetDamageHighlight();
    }

    // Update is called once per frame
    void Update()
    {
        turnCounterText.text = "" + turnCounter;
        playerHPText.text = "HP: " + currentPlayerHP + "/" + maxPlayerHp;
        monsterKillsText.text = "Kills: " + currentKills + "/" + maxKills;
        if(currentPlayerHP <= 0)
        {
            defeatPanel.SetActive(true);
        }
        if(currentKills >= maxKills)
        {
            victoryPanel.SetActive(true);
        }
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        turnCounter = maxTurnCounter;

        gridManager.ClearPossibleTurnHighLight();
        deckManager.DiscardCards();
        deckManager.DrawCards();
        InflictDamage();
        yield return MonstersTurn();
        spawnManager.SpawnMonsters();
        gridManager.SetIsWalkable();
        gridManager.SetDamageHighlight();

        turnNumber++;

        spawnManager.PredictMonstersToSpawn();

        yield return null;
    }

    public void PlayerTurn(int x, int y)
    {
        Card card = deckManager.GetActiveCard();
        Character character = player.GetComponent<Character>();
        if (card.cardColor != CharacterColors.None)
        {
            character.characterColor = card.cardColor;
            character.SetHighlight(character.characterColor);
        }

        character.MoveTo(new Vector3(x*cellsize + cellsize / 2, y*cellsize + cellsize / 2, 0), .5f);

        gridManager.ClearPossibleTurnHighLight();
        deckManager.DiscardCard(card, true);
    }

    public void FinishTurn(Character character)
    {
        if(character.characterType == 0)
        {
            turnCounter -= 1;
            if (turnCounter <= 0)
            {
                EndTurn();
            }
        }
        else
        {

        }
    }

    public IEnumerator MonstersTurn()
    {
        Character monster;
        bool includeTarget;
        Vector3 target;
        GameObject monsterObject, currentMonsterObject = null;

        for (int monsterIndex = monsters.Count-1; monsterIndex >= 0; monsterIndex--)
        {
            if (monsterIndex > monsters.Count - 1) continue; //If monsters died - we skip their indexes
            //Debug.Log("Current: " + monsterIndex + " max " + (monsters.Count-1));
            monsterObject = monsters[monsterIndex];
            if (currentMonsterObject == monsterObject) continue; //No second turn for monster if someone died on his turn
            currentMonsterObject = monsterObject;
            monster = monsterObject.GetComponent<Character>();
            gridManager.SetIsWalkable(monster);
            if (monster.characterType == 2)
            {
                target = gridManager.GetGridCenter();
                includeTarget = gridManager.GetIsWalkable(target);
            }
            else
            {
                target = player.transform.position;
                includeTarget = false;
            }
            List<Vector3> path = gridManager.FindPath(monsterObject.transform.position, target, monster.characterType);
            //Debug.Log(target.x + ";" + target.y + ";" + target.z);
            if (path == null)
            {
                continue;
            }
            foreach (Vector3 currentTarget in path)
            {
                //Debug.Log(monsterObject.name + " " + currentTarget.x + ";" + currentTarget.y + ";" + currentTarget.z);
            }
            path.RemoveAt(0);
            if (!includeTarget && path.Count > 0) path.RemoveAt(path.Count-1);
            int stepsToRemove = path.Count - monster.speed;
            if (stepsToRemove > 0)
            {
                for(int i = 0; i < stepsToRemove; i++)
                {
                    path.RemoveAt(path.Count - 1);
                }
            }
            yield return monster.MoveToCoroutine(path, .5f);
            gridManager.SetIsWalkable();

            foreach (Vector3 currentTarget in path)
            {
                //Debug.Log(monsterObject.name + " " + currentTarget.x + ";" + currentTarget.y + ";" + currentTarget.z);
            }
        }
    }

    public void DestroyMonster(GameObject monster)
    {
        //Debug.Log("Destroying " + monster.name);
        //gridManager.SetOccupyingCharacter(monster.transform.position, null);
        monsters.Remove(monster);
        Destroy(monster);
        gridManager.SetIsWalkable();
        gridManager.SetDamageHighlight();
        AddKills(1);
    }

    public void InflictDamage()
    {
        currentPlayerHP -= gridManager.GetDamage(player.transform.position);
    }

    public void InflictDamage(int damage)
    {
        currentPlayerHP -= damage;
    }

    public void AddKills(int kills)
    {
        currentKills += kills;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextStage()
    {
        int maxSceneCount = SceneManager.sceneCountInBuildSettings-1;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        nextSceneIndex = nextSceneIndex > maxSceneCount ? 0 : nextSceneIndex;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
public enum CharacterColors
{
    None,
    Green,
    Red,
    Pink
}
