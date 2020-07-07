using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Deck : MonoBehaviour
{
    public gameType GameType = gameType.two;
    Sprite[] cardImages;
    string[] seeds = { StaticStrings.cup, StaticStrings.gold, StaticStrings.wand, StaticStrings.sword };
    public List<Card> deck = new List<Card>();
    bool cardEnded=false;
    Card lastCard = null;
    B_Entity[] allPlayers;
    B_UI ui;
    B_Table table;
    [HideInInspector]
    public B_Entity player;
    bool Exit = false;
    private void Start()
    {
        ui = FindObjectOfType<B_UI>();
        allPlayers = FindObjectsOfType<B_Entity>();
        //set player
        foreach(var item in allPlayers)
        {
            if(item.isPlayer)
            {
                player = item;
            }
        }
        table = GetComponent<B_Table>();
        table.green = allPlayers[0].green;
        BuidDeck();
        if(GameType!=gameType.five)
        {
            normalPlaySetUp();
        }
        else
        {
            callingSetUp();
        }
        //initialize table for last beacause game start whit table
        table.INIT(this);
    }

    private void Update()
    {
        if(cardEnded&&!Exit)
        {
            GameOverTest();
        }
    }
    Card DrawedCard()
    {
        //temp card for return
        Card c;
        bool equal = true;
        int rnd = 0;
        while (equal)
        {
           rnd = Random.Range(0, deck.Count - 1);
            if(lastCard!=deck[rnd])
            {
                equal = false;
            }
        }
        //returning card became lick deck's card index
        c = deck[rnd];
        //remove from list and sort
        deck.RemoveAt(rnd);
        //return card
        return c;
    }
 
    //build deck when start the game
    void BuidDeck()
    {
        //load all images
        cardImages = Resources.LoadAll<Sprite>("Cards");
        //for every seed make loop, create card with informations
        for (int i = 0; i < 4; i++)
        {
            //set current seed name
            string path = seeds[i];
            //start from 1 to 10 create card
            for (int j = 1; j < 11; j++)
            {
                Card c = new Card();

                c.seed = path; //set name
                c.value = j; //set value
                string nameofCard = j + path; //create temp name for resource
                //if name of card is equal to the name of image set image
                foreach (var item in cardImages)
                {
                    if (nameofCard == item.name)
                    {
                        c.img = item;
                    }
                }
                //add card to the deck
                deck.Add(c);
            }
        }
        
    }
    //add card to payer hand
    void addCardToPlayer(B_Entity e)
    {
        if (cardEnded) return;
        e.AddCardToHand(DrawedCard());
       
    }
    //play if equal for 2 or 4 player
    void normalPlaySetUp()
    {
        int rnd = Random.Range(0, deck.Count - 1);
        lastCard = deck[rnd];
        ui.setLastCardImage(lastCard.img);
        //set briscola
        table.SetBriscola(lastCard.seed);
        
        //initialize players and pass table and briscola info
        foreach(var item in allPlayers)
        {
            item.INIT(table);
            item.Briscola = lastCard.seed;
        }
        //assign card to players
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < allPlayers.Length; j++)
            {
                addCardToPlayer(allPlayers[j]);
            }
        }

    }
    //call
    void callingSetUp()
    {

    }
    //
    public void nextTurn()
    {
        if (cardEnded)
        {
            table.UpdateMoves();
            return;
        }
        if(deck.Count<=allPlayers.Length)
        {
            cardEnded = true;
            table.lastTurn = true;
            for (int i=0;i<allPlayers.Length;i++)
            {
                allPlayers[i].AddCardToHand(deck[i]);
                allPlayers[i].lastTurn = true;
            }
            deck.Clear();
            ui.clearImages();
        }
        else
        {
            for (int j = 0; j < allPlayers.Length; j++)
            {
                addCardToPlayer(allPlayers[j]);
              
            }
        }
        //move player after drawing
        table.UpdateMoves();
    }

    void GameOverTest()
    {
        bool GameOver = true;

        foreach (var item in allPlayers)
        {
            if(!item.isPlayer)
            {
                if(item.hand.Count > 0)
                GameOver = false;
            }
            else
            {
                if(item.countdown>0)
                {
                    GameOver = false;
                }
            }
        }
        if (GameOver)
        {
            Exit = true;
            switch(GameType)
            {
                case gameType.two:
                    ui.ShowEnd();
                    Destroy(gameObject);
                    break;
                case gameType.four:
                    ui.MultiplayerEnd();
                    break;
                case gameType.five:
                    ui.MultiplayerEnd();
                    break;
            }
        }
    }
}

public enum gameType
{
    two,
    four,
    five
}