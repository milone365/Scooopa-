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
    void Start()
    {
        table = FindObjectOfType<Table>();
        BuidDeck();
        setTable();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        //blanck temp list
        List<Card> temp = new List<Card>();
        //random card in the deck
        int rnd = Random.Range(0, deck.Count);
        //returning card became lick deck's card index
        c = deck[rnd];
        //remove from list and sort
        deck.RemoveAt(rnd);

        //use temporary list for sort the deck
        for (int i = 0; i < deck.Count; i++)
        {
            //id is not null add tu temporary
            if (deck[i] != null)
            {
                temp.Add(deck[i]);
            }
        }
        //deck became like temporary
        deck = temp;
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

}

[System.Serializable]
public class Card
{
    public int value = 0;
    public string seed = "";
    public Sprite img;
}
