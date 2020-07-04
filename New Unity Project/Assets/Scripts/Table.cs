using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Table : MonoBehaviour
{
    public Image[] tablecardBorders;
    public List<Card> tableCards = new List<Card>();
    Image[] tablecards_images;
    Entity currentPlayer=null;
    public Entity player;
    public Entity pc;
    int currentTurn = 0;
    [SerializeField]
    Image playerUsedCard = null;
    [SerializeField]
    Image pcUsedCard = null;
    bool canMove = false;
    float timerCounter = 0;
    float timeDelay = 1.5f;
    DeckController deck;
    Button[] playerButtons;
    void Start()
    {
        tablecards_images = GetComponentsInChildren<Image>();
        
       //deactive used card images
       playerUsedCard.enabled = false;
       pcUsedCard.enabled = false;
        playerButtons = player.GetComponentsInChildren<Button>();
        //deactive yellow borders
        foreach(var b in tablecardBorders)
        {
            b.enabled = false;
        }
        //initialize deck and pass this reference
        deck =GetComponentInChildren<DeckController>();
        deck.Init(this);
    }

    void Update()
    {
        Draw();
    }

    #region Remove
    void removeCardFromTable(int v)
    {
        tableCards.RemoveAt(v);
    }
    #endregion

    public void addCardToTable(Card c)
    {
        Card newcard = c;
        tableCards.Add(newcard);
    }

    public void Draw()
    {
        if (tableCards.Count < 1) return;
        //set cards
        for (int i = 0; i < tablecards_images.Length-1; i++)
        {
           tablecards_images[i].enabled = false;
        }
        for(int i=0;i<tableCards.Count;i++)
        {
            tablecards_images[i].enabled = true;
            tablecards_images[i].sprite = tableCards[i].img;
        }
    }

    void Scopa()
    {
        currentPlayer.scopa++;
    }
    //change turn
    void goToNextTurn()
    {
        currentTurn++;
        if (currentTurn%2==0)
        {
            currentPlayer = player; 
        }
        else
        {
            //set pc turn
            currentPlayer = pc;
            canMove = true;
        }
    }
    void TurnManagement()
    {
        if(currentPlayer==pc)
        {
              if (canMove)
                {
                    timerCounter += Time.deltaTime;
                    //reset timer
                    if (timerCounter >= timeDelay)
                    {
                        canMove = false;
                        timerCounter = 0;
                        int rnd = Random.Range(0, 2);
                        pc.PlayCard(rnd);
                        StartCoroutine(playedCardUpdate(pc.playedCard,currentPlayer));
                        
                    }
               }
        }
    }
   

    public void PlayCard(Card c, Entity e)
    {
        StartCoroutine(playedCardUpdate(c, e));
    }

   IEnumerator playedCardUpdate(Card c,Entity e)
    {
        //for see the played card in red or blue zone
        if(e==player)
        {
            playerUsedCard.enabled = true;
            playerUsedCard.sprite = c.img;
        }
        else
        {
            pcUsedCard.enabled = true;
            pcUsedCard.sprite = c.img;
        }
        HightLightEqualCard(c.value);
        yield return new WaitForSeconds(1.5f);
        DeHighLighAllCards();
        //if can not make combo add card to table
        yield return new WaitForSeconds(1f);
        //create temp card for check
        Card equal = getEqualCard(c.value);
        //add both cards if equals
        if(equal==null)
        {
            addCardToTable(c);
           
        }
        else
        {
            takeCard(equal, e);
            takeCard(c, e);
            removeCardFromTable(TableCardsIndex(equal));
        }
        
        
        yield return new WaitForSeconds(1.5f);
        //goToNextTurn();
    }


    public void activePlayerButtons()
    {
        foreach(var b in playerButtons)
        {
            b.enabled = true;
        }
    }

    #region Taking
    //return a single card to take if is eqaul to played card
    Card getEqualCard(int value)
    {
        for (int i = 0; i < tableCards.Count; i++)
        {
            if (value == tableCards[i].value)
            {
                
                return tableCards[i];
               
            }
        }
        return null;
    }
    void takeCard(Card c, Entity e)
    {
        e.CollectCard(c);
    }
    #endregion

    #region HighLight
    void DeHighLighAllCards()
    {
        //make cards blanck
        pcUsedCard.enabled = false;
        playerUsedCard.enabled = false;
        for (int i = 0; i < tableCards.Count; i++)
        {
          tablecardBorders[i].enabled = false;
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
    #endregion

    #region Returning
    int TableCardsIndex(Card c)
    {
        for (int i = 0; i < tableCards.Count; i++)
        {
            if (c == tableCards[i])
            {
                return i;
            }
        }
        return 0;
    }
    #endregion
}
