using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    [SerializeField] private List<Card> deck = new List<Card>();
    [SerializeField] private List<Card> discard = new List<Card>();
    [SerializeField] private Transform[] handSlots;
    [SerializeField] private Transform[] deckSlots;
    [SerializeField] private Transform[] discardSlots;
    public bool[] availableHandSlots;
    public bool[] availableDeckSlots;
    public bool[] availableDiscardSlots;


    public void DrawCards()
    {
        for (int i = 0; i < availableHandSlots.Length; i++)
        {
            if (availableHandSlots[i])
            {

                DrawCard();

            }
        }
    }
    public void DrawCard()
    {
        if(deck.Count == 0)
        {
            ShuffleCards();
        }

        Card randCard = deck[Random.Range(0, deck.Count)];
        for (int i = 0; i < availableHandSlots.Length; i++)
        {
            if (availableHandSlots[i])
            {
                randCard.handIndex = i;
                randCard.transform.position = handSlots[i].position;
                randCard.transform.localScale = new Vector3(4.5f, 4.5f);
                randCard.hasBeenPlayed = false;
                availableHandSlots[i] = false;
                availableDeckSlots[randCard.deckIndex] = true;
                deck.Remove(randCard);
                hand.Add(randCard);
                return;
            }
        }

    }

    public void DiscardCards()
    {
        foreach(Card card in hand)
        {
            DiscardCard(card);
        }
        hand.Clear();
    }

    public void DiscardCard(Card card, bool removeFromHand = false)
    {
        card.transform.position = discardSlots[card.deckIndex].position;
        card.transform.localScale = new Vector3(3f, 3f);
        availableHandSlots[card.handIndex] = true;
        availableDiscardSlots[card.deckIndex] = false;
        if(removeFromHand) hand.Remove(card);
        discard.Add(card);

        //if(hand.Count == 0)
        //{
        //    DrawCards();
        //}

    }

    public void ShuffleCards()
    {
        foreach(Card card in discard)
        {
            deck.Add(card);
            card.transform.position = deckSlots[card.deckIndex].position;
            availableDiscardSlots[card.deckIndex] = true;
            availableDeckSlots[card.deckIndex] = false;
        }
        discard.Clear();
    }

    public Card GetActiveCard()
    {
        foreach (Card card in hand)
        {
            if (card.hasBeenPlayed) return card;
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        DrawCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
