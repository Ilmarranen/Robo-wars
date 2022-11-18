using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private GameManager gameManager;
    private GridManager gridManager;
    public int characterType;
    public CharacterColors characterColor;
    public int speed;

    public Dictionary<CharacterColors, GameObject> Highlights = new Dictionary<CharacterColors, GameObject>();
    public Dictionary<CharacterColors, GameObject> SawHighlights = new Dictionary<CharacterColors, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridManager = FindObjectOfType<GridManager>();
        if(characterType != 0)
        {
            //gridManager.SetIsWalkable(transform.position, false);
        }
    }

    private void Awake()
    {
        Transform childHighlight;
        childHighlight = transform.Find("Green");
        if (childHighlight != null) Highlights.Add(CharacterColors.Green, childHighlight.gameObject);
        childHighlight = transform.Find("Pink");
        if (childHighlight != null) Highlights.Add(CharacterColors.Pink, childHighlight.gameObject);
        childHighlight = transform.Find("Red");
        if (childHighlight != null) Highlights.Add(CharacterColors.Red, childHighlight.gameObject);
        childHighlight = transform.Find("SawGreen");
        if (childHighlight != null) SawHighlights.Add(CharacterColors.Green, childHighlight.gameObject);
        childHighlight = transform.Find("SawPink");
        if (childHighlight != null) SawHighlights.Add(CharacterColors.Pink, childHighlight.gameObject);
        childHighlight = transform.Find("SawRed");
        if (childHighlight != null) SawHighlights.Add(CharacterColors.Red, childHighlight.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision to " + gameObject + " from " + collision.gameObject);

        if (characterType == 0)
        {
            if (collision.CompareTag("Saw"))
            {
                gameManager.InflictDamage(1);
            }
        }
        else
        {
            if (collision.CompareTag("Saw")) 
            {
                //Debug.Log("Saw collision");
                //No collison on mobs for saw
            }
            else if(collision.CompareTag("Player"))
            {
                gameManager.DestroyMonster(gameObject);
            }
            else
            {
                //gameManager.DestroyMonster(gameObject);
                gameManager.DestroyMonster(collision.gameObject);
            }


        }
    }

    public void SetHighlight(CharacterColors highlightType, bool setState = true, bool withSaw = false)
    {
        //Disable all highlights for consistency
        foreach (KeyValuePair<CharacterColors, GameObject> highlight in Highlights)
        {
            highlight.Value.SetActive(false);
        }
        foreach (KeyValuePair<CharacterColors, GameObject> highlight in SawHighlights)
        {
            highlight.Value.SetActive(false);
        }

        if (Highlights.ContainsKey(highlightType))
        {
            Highlights[highlightType].SetActive(setState);
        }
        if (withSaw && SawHighlights.ContainsKey(highlightType))
        {
            SawHighlights[highlightType].SetActive(setState);
        }
    }

    public void MoveTo(Vector3 pos, float dur)
    {
        //gridManager.SetOccupyingCharacter(transform.position, null);
        StartCoroutine(MoveToCoroutine(pos, dur));
        //gridManager.SetOccupyingCharacter(pos, this);
    }

    public void MoveTo(List<Vector3> pos, float dur)
    {
 
        StartCoroutine(MoveToCoroutine(pos, dur));

    }

    public IEnumerator MoveToCoroutine(List<Vector3> pos, float dur)
    {
        if (characterType != 0)
        {
            //gridManager.SetIsWalkable(transform.position);
        }
        //gridManager.SetOccupyingCharacter(transform.position, null);
        foreach (Vector3 currentPos in pos)
        {
            yield return StartCoroutine(MoveToCoroutine(currentPos, dur));
        }
        if (characterType != 0)
        {
            //gridManager.SetIsWalkable(transform.position, false);
        }
        //gridManager.SetOccupyingCharacter(transform.position, this);
    }

    public IEnumerator MoveToCoroutine(Vector3 pos, float dur)
    {
        float t = 0f;
        Vector3 start = this.transform.position;
        Vector3 v = pos - start;
        while (t < dur)
        {
            t += Time.deltaTime;
            this.transform.position = start + v * t / dur;
            yield return null;
            gridManager.SetIsWalkable();
            gridManager.SetDamageHighlight();
        }

        this.transform.position = pos;

        gameManager.FinishTurn(this);
    }
}
