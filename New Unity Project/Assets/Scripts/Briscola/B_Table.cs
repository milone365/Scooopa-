using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_Table : MonoBehaviour
{
    [SerializeField]
    Mover mover = null;
    string Briscola;
    string commander;
    B_Deck deck;
    [HideInInspector]
    public Sprite green;
    public B_Entity[] players;
    bool firstCard = true;
    B_Entity taker;
    int turnIndex = 0;
    Image[] tablecardsimages;
    [HideInInspector]
    public bool lastTurn = false;
    public B_Entity currentTurn;

    public List<Card> groundCards = new List<Card>();

    public void INIT(B_Deck d)
    {
        deck = d;
        if(players==null)
        players = FindObjectsOfType<B_Entity>();
        tablecardsimages = GetComponentsInChildren<Image>();
        taker = players[0];
        UpdateMoves();
    }
   
    public void SetBriscola(string s)
    {
        Briscola = s;
    }
  
    
    public void PlayCard(B_Entity e,Card c)
    {
        StartCoroutine(Management(e, c));
    }
    IEnumerator Management(B_Entity e, Card c)
    {
        
        //if is the first card set the card seed like a commander
        if (firstCard)
        {
            firstCard = false;
            commander = c.seed;
            if(mover!=null)
            mover.setPosition(turnIndex);
        }
        //add card to the ground
        groundCards.Add(c);
        yield return new WaitForSeconds(0.8f);
        if (groundCards.Count>=players.Length)
        {
            TableUpdate();
            yield return new WaitForSeconds(0.8f);
            goToTheNextTurn();
        }
        else
        {
            UpdateMoves();
        }
    }

    public void UpdateMoves()
    {
        //il is the first card of turn, move last taker
        if(firstCard)
        {
            turnIndex = getTakerIndex();
        }
        else
        {
            turnIndex++;
            if (turnIndex >= players.Length) { turnIndex = 0; }
        }
        //moves
        if (!players[turnIndex].isPlayer)
        {
            players[turnIndex].PcPlayCard();
        }
        else
        {
            players[turnIndex].activeButtons();
        }
    }

    int getTakerIndex()
    {
        for(int i=0;i<players.Length;i++)
        {
            if(players[i]==taker)
            {
                return i;
            }
        }
        return 0;
    }

    //real card taking update
    void TableUpdate()
    {
        Card winCard;
        //add at temp list all played cards
        List<Card> playedcards=new List<Card>();
        for(int i=0;i<players.Length;i++)
        {
            playedcards.Add(players[i].playedCard);
        }
        //calculate winner card
        winCard = StaticFunctions.getWinnerCard(playedcards,Briscola,commander);
        taker = winCard.playerRef;
        //hiligth most strong card
        taker.HilightBorder(true);
        //add all ground cards to player
        foreach(var item in groundCards)
        {
            taker.collectedCards.Add(item);
        } 
    }
    //end turn
    void goToTheNextTurn()
    {
        firstCard = true;
        //de higligth cards borders
        taker.HilightBorder(false);
        //remove all card from field
        groundCards.Clear();
        //clear ground images
        for (int i = 0; i < tablecardsimages.Length; i++)
        {
            tablecardsimages[i].sprite = green;
        }
        //
        deck.nextTurn();
    }
   

}
