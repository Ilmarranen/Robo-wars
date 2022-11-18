using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;
    public int deckIndex;
    private DeckManager deckManager;
    private GridManager gridManager;
    public CharacterColors cardColor;
    //public bool[,] cardMap = new bool[3,3];

    [System.Serializable]
    public class MapRow
    {
        public bool[] row;
    }
    public MapRow[] cardMap;

    // Start is called before the first frame update
    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        gridManager = FindObjectOfType<GridManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if(hasBeenPlayed == false)
        {
            foreach(Card card in deckManager.hand)
            {
                card.CancelPlay();
            }

            transform.localScale = new Vector3(7f, 7f);

            gridManager.ClearPossibleTurnHighLight();
            gridManager.SetPossibleTurnHighlight(this);

            hasBeenPlayed = true;
            //deckManager.availableHandSlots[handIndex] = true;
            //Invoke("DiscardCard", .5f);
        }
    }

    public void CancelPlay()
    {
        hasBeenPlayed = false;
        transform.localScale = new Vector3(4.5f, 4.5f);
    }

    void DiscardCard()
    {
        deckManager.DiscardCard(this);
    }
}
