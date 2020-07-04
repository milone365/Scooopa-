using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DeckController : MonoBehaviour
{
    Sprite[] cardImages;
    string[] seeds = { StaticStrings.cup, StaticStrings.gold, StaticStrings.wand, StaticStrings.sword };
    public List<Card> deck = new List<Card>();
    Table table;
    Entity player;
    Entity pc;
    bool cardEnded = false;
    public void Init(Table t)
    {
        table = t;
        player = table.player;
        pc = table.pc;
        //load deck
        BuidDeck();
        //set up first 4 cards
        setTable();
        //initialize player and pc and pass table reference
        player.INIT(t);
        pc.INIT(t);
        //assign card to players
        giveCardToPlayers();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            newTurn();
        }
    }
    //set four card on the table
    void setTable()
    {
        int spawned = 0;
        while(spawned<4)
        {
           table.addCardToTable(DrawedCard());
           spawned++;
        }
        
    }
    
    Card DrawedCard()
    {
        //temp card for return
        Card c;
       //random card in the deck
        int rnd = Random.Range(0, deck.Count-1);
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

    //assign 3 cards for every player
    void giveCardToPlayers()
    {
        if (cardEnded) return;
            int spawned = 0;
        while (spawned < 3)
        {
            player.DrawCardFromDeck(DrawedCard(),spawned);
            pc.DrawCardFromDeck(DrawedCard(),spawned);
            spawned++;
        }

    }

    public void newTurn()
    {
        if(cardEnded)
        {
            GameOver();
            return;
        }
        //reset hands list
        player.resetCardList();
        pc.resetCardList();
        //give new cards
        giveCardToPlayers();
        //active player buttons
        table.activePlayerButtons();
        if(deck.Count<1)
        {
            cardEnded = true;
        }
    }
    void GameOver()
    {
        Debug.Log("GAMEOVER");
    }
}

[System.Serializable]
public class Card
{
    public int value = 0;
    public string seed = "";
    public Sprite img;
}
