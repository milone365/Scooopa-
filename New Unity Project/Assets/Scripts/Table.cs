using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    public List<Card> tableCards = new List<Card>();
    Image[] tablecards_images;
    bool taking = false;
    Entity currentPlayer=null;
    Player player;
    CPU pc;
    int currentTurn = 0;

    bool canMove = false;
    float timerCounter = 0;
    float timeDelay = 1.5f;

    void Start()
    {
        tablecards_images = GetComponentsInChildren<Image>();
        foreach(var item in tablecards_images)
        {
            item.enabled = false;
        }
        //find objects
        player = FindObjectOfType<Player>();
        pc = FindObjectOfType<CPU>();
    }

    void Update()
    {
        Draw();
    }

    #region Remove

    //remove card and sort list
    void removeCard(int v)
    {
        //add card to current player
        if (currentPlayer != null)
            currentPlayer.AddCardToCollection(tableCards[v]);
        //remove from table list
        tableCards.RemoveAt(v);
        //use temp for resort table list
        List<Card> temp = new List<Card>();
        for (int i = 0; i < tableCards.Count; i++)
        {
            if (tableCards[i] != null)
                temp.Add(tableCards[i]);
        }
        tableCards = temp;
        //
        if (tableCards.Count == 0)
        {
            Scopa();
        }
    }

    public void removeCardFromtable(int value)
    {
        bool removed = false;
        for (int i = 0; i < tableCards.Count; i++)
        {
            //remove single intere card if is on the table
            if (tableCards[i].value == value)
            {
                removeCard(i);
                removed = true;
            }
        }
        if (removed) return;
        int index = tableCards.Count;

        //check combination 
        for (int i = 0; i < index; i++)
        {
            for (int j = 0; j < index; j++)
            {
                if (j != i)
                {
                    //sum of the cards
                    int v1 = tableCards[i].value;
                    int v2 = tableCards[j].value;

                    //remove both cards
                    if (v1 + v2 == value)
                    {
                        removeCard(v1);
                        removeCard(v2);
                        break;
                    }
                }

            }
        }

    }


    #endregion

    public void addCardToTable(Card c)
    {
        Card newcard = c;
        tableCards.Add(newcard);
        
    }

    public void Draw()
    {
        if (tableCards.Count < 0) return;
        //set cards
        for (int i = 0; i < tablecards_images.Length; i++)
        {
            if(i<=tableCards.Count)
            {
                tablecards_images[i].sprite = tableCards[i].img;
                tablecards_images[i].enabled = true;
            }
            else
            {
                tablecards_images[i].enabled = false;
            }
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
        if(currentTurn%2==0)
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
                        goToNextTurn();
                    }

                }
            
           
        }
    }
}
