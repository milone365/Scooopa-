using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour, Icollect
{
    public List<Card> collectedCards;
    List<Card> hand = new List<Card>();
    public int scopa = 0;
    public int points=0;

    //remove card from hand and make in play
    public void PlayCard(int value)
    {
        hand.RemoveAt(value);
    }
    
    //add to collected card
    public void AddCardToCollection(Card c)
    {
        collectedCards.Add(c);
    }
    //take new card
    public void DrawCard(Card c)
    {
        hand.Add(c);
    }
}

public  interface Icollect
{
    void AddCardToCollection(Card c);
    void PlayCard(int value);
    void DrawCard(Card c);
}
