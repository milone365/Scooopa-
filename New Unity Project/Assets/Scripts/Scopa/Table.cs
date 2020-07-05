using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Table : MonoBehaviour
{
    public bool Multiplayer = false;
    GameObject Scopatxt;
    public Image[] tablecardBorders;
    public List<Card> tableCards = new List<Card>();
    Image[] tablecards_images;
    public Entity currentPlayer=null;
    public Entity[] allPlayers;
    Entity lastTaked = null;
    public Entity player;
    public Entity pc;
    public Entity ally;
    public DeckController deck;
    Button[] playerButtons;

    ScopaManager scopa=null;
    ScoponeManager scopone=null;

  
    void Start()
    {
        
        if(Multiplayer)
        {
            scopone = new ScoponeManager();
            scopone.INIT(this);
            
        }
        else
        {
            scopa = new ScopaManager();
            scopa.Init(this);
        }
        tablecards_images = GetComponentsInChildren<Image>();
        //deactive used card images
        foreach (var p in allPlayers)
        {
            p.UsedCard.enabled = false;
            if(p.isPlayer)
            {
                player = p;
            }
            else
            {
                pc = p;
            }
        }
        playerButtons = player.GetComponentsInChildren<Button>();
        //deactive yellow borders
        foreach(var b in tablecardBorders)
        {
            b.enabled = false;
        }
        //initialize deck and pass this reference
        deck = GetComponentInChildren<DeckController>();
        if (!Multiplayer)
        {
            
            deck.Init(this);
            currentPlayer = player;
        }
        else
        {
            deck.MultiplayerInit(this,allPlayers);
            scopone.goToNextTurn();
        }
        Scopatxt = GameObject.Find("Scopa");
        Scopatxt.SetActive(false);
        
    }

    void Update()
    {
        Draw();
    }

    #region Add_Remove
    public void addCardToTable(Card c)
    {
        Card newcard = c;
        tableCards.Add(newcard);
    }
    void removeCardFromTable(int v)
    {
        tableCards.RemoveAt(v);
    }
    #endregion

    #region Graphic
    public void Draw()
    {
        //set cards
        for (int i = 0; i < tablecards_images.Length - 1; i++)
        {
            tablecards_images[i].enabled = false;
        }
        for (int i = 0; i < tableCards.Count; i++)
        {
            tablecards_images[i].enabled = true;
            tablecards_images[i].sprite = tableCards[i].img;
        }
    }
    #endregion

    void Scopa()
    {
        currentPlayer.scopa++;
    }
   
   public void PlayCard(Card c, Entity e)
    {
        StartCoroutine(playedCardUpdate(c, e));
    }

    IEnumerator playedCardUpdate(Card c,Entity e)
    {
        e.UsedCard.enabled = true;
        e.UsedCard.sprite = c.img;
        yield return new WaitForSeconds(0.5f);
        if (scopone!=null)
        {
            if(c.value==1)
            {
                HighLighAllCards();
                yield return new WaitForSeconds(0.75f);
                for (int i = 0; i < tableCards.Count; i++)
                {
                    takeCard(tableCards[i],e);
                }
                tableCards.Clear();
                DeHighLighAllCards();
                yield return new WaitForSeconds(0.75f);
                scopone.goToNextTurn();
                yield break;
            }
        }
        //create temp card for check
        Card equal = StaticFunctions.getEqualCard(tableCards,c.value);
        //add both cards if equals
        if(equal==null)
        {
           List<Card>temp= StaticFunctions.getTakableCards(tableCards, c.value);
            if(temp!=null)
            {
                //active yellow border
                foreach (var t in temp)
                {
                    HightLightSelectedCard(t);
                }
                //pause
                yield return new WaitForSeconds(1f);
                //take the cards
                foreach (var t in temp)
                {
                    takeCard(t);
                    removeCardFromTable(StaticFunctions.getTableIndex(tableCards,t));
                }
            }
            else
            {
                addCardToTable(c);
            }
           
        }
        else
        {
            //colorate border
            HightLightEqualCard(c.value);
            yield return new WaitForSeconds(1.5f);
            //add taked card to player pile
            takeCard(equal);
            takeCard(c);
            //remove card from table
            removeCardFromTable(StaticFunctions.getTableIndex(tableCards,equal));
        }
        DeHighLighAllCards();
        yield return new WaitForSeconds(1f);
        //scopa check, if are not cardon table after taking add one point
       if (tableCards.Count<1)
        {
            Scopatxt.SetActive(true);
            Scopa();
            yield return new WaitForSeconds(0.75f);
            Scopatxt.SetActive(false);
        }
       if(scopone==null)
        {
            scopa.goToNextTurn();
        }
       else
        {
            scopone.goToNextTurn();
        }
     
    }

    public void cleanTable()
    {
        for(int i=0;i<tableCards.Count;i++)
        {
            takeCard(tableCards[i],lastTaked);
            removeCardFromTable(i);
        }
    }

    public void activePlayerButtons(bool value)
    {
        foreach(var b in playerButtons)
        {
            b.enabled = value;
        }
    }

    #region Taking
    
   
    void takeCard(Card c)
    {
        currentPlayer.CollectCard(c);
        lastTaked = currentPlayer;
    }
    void takeCard(Card c,Entity e)
    {
        e.CollectCard(c);
    }
    #endregion

    #region HighLight
    void DeHighLighAllCards()
    {
        foreach(var p in allPlayers)
        {
            p.UsedCard.enabled = false;
        }
        for (int i = 0; i < tablecards_images.Length; i++)
        {
          tablecardBorders[i].enabled = false;
        }
    }
    void HighLighAllCards()
    {
        //make cards blanck
        foreach (var p in allPlayers)
        {
            p.UsedCard.enabled = false;
        }
        for (int i = 0; i < tableCards.Count; i++)
        {
            tablecardBorders[i].enabled = true;
        }
    }
    void HightLightEqualCard(int c)
    {
        int index = 0;
        for (int i = 0; i < tableCards.Count; i++)
        {
            if (c == tableCards[i].value)
            {
                index = i;
                tablecardBorders[index].enabled = true;
                break;
            }
        }
    }
    void HightLightSelectedCard(Card c)
    {
        int index = 0;
        for (int i = 0; i < tableCards.Count; i++)
        {
            if (c == tableCards[i])
            {
                index = i;
                tablecardBorders[index].enabled = true;
                break;
            }
        }
    }
    #endregion

    //Scopone Update

    public void goToTheEnd()
    {
        Debug.Log("fine");
        cleanTable();
        MultiplayerScoreMangager manager= GetComponent<MultiplayerScoreMangager>();
        manager.ShowScores(this);
    }
}
