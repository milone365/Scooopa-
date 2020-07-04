using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Table : MonoBehaviour
{
    GameObject Scopatxt;
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
        Scopatxt = GameObject.Find("Scopa");
        Scopatxt.SetActive(false);
        currentPlayer = player;
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
    //change turn
    void goToNextTurn()
    {
        //if players don't have more card draw 3 
        if (player.getNumOfCard() == 0 && pc.getNumOfCard() == 0)
        {
            deck.newTurn();
        }
        currentTurn++;
        if (currentTurn%2==0)
        {
            //enable button for play cards
            currentPlayer = player;
            activePlayerButtons(true);
        }
        else
        {
            currentPlayer = pc;
            //player can not play card's on enemy turn
            activePlayerButtons(false);
            pcTurnManagement();
        }  
    }
    void pcTurnManagement()
    {
        //reset timer
        pc.PcPlayCard();
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
                yield return new WaitForSeconds(1.5f);
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
        yield return new WaitForSeconds(1.5f);
        //scopa check, if are not cardon table after taking add one point
       if (tableCards.Count<1)
        {
            Scopatxt.SetActive(true);
            Scopa();
            yield return new WaitForSeconds(1.5f);
            Scopatxt.SetActive(false);
        }
       goToNextTurn();
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
    }
    #endregion

    #region HighLight
    void DeHighLighAllCards()
    {
        //make cards blanck
        pcUsedCard.enabled = false;
        playerUsedCard.enabled = false;
        for (int i = 0; i < tablecards_images.Length; i++)
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

}
