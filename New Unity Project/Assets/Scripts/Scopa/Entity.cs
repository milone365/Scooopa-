using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Entity : MonoBehaviour
{
    public Image hilightPlayer_img;
    public Image UsedCard;
    public bool isPlayer = false;
    public Sprite back;
    public Sprite green;
    public List<Card> collectedCards;
    Image[] hand_images;
    List<Card> hand = new List<Card>();
    public int scopa = 0;
    public int points=0;
    Table table;
    public Card playedCard;
    ScoponeManager scopone;

    public int getNumOfCard()
    {
        int num = 0;
        for (int i = 0; i <hand.Count;i++)
        {
            if(hand[i]!=null)
            {
                num++;
            }
        }
        return num;
    }

    public virtual void INIT(Table t)
    {
        hand_images = GetComponentsInChildren<Image>();
        table = t;
        HilightPlayer(false);
    }
    //init Multiplayer
    public virtual void INIT(ScoponeManager sc)
    {
        hand_images = GetComponentsInChildren<Image>();
        scopone = sc;
    }
    //remove card from hand and make in play
    public void PlayCard(int value)
    {
        if(hand[value] == null)return;
        playedCard = hand[value];
        //reset
        hand[value] = null;
        //make green image
        hand_images[value].sprite = green;
        //disabilititate button
        hand_images[value].GetComponent<Button>().enabled = false;
        if(scopone!=null)
        {

        }
        else
        {
            table.PlayCard(playedCard, this);
        }
        
    }
    
    public void PcPlayCard()
    {
        if(table==null)
        {
            table = FindObjectOfType<Table>();
        }
        int index = 0;
        List<Card> temp = new List<Card>();
        for(int i=0;i<hand.Count;i++)
        {
            List<Card>temp2=(StaticFunctions.getTakableCards(table.tableCards, hand[i].value));
            
            if (temp2 != null)
            {
                temp.AddRange(temp2);
                index = i;
                break;
            }
                
        }
        //checl possible combos
        if(temp.Count>0)
        {
            playedCard = hand[index];
            hand.RemoveAt(index);
            index = 0;

        }
        else
        {
            bool equalcard = false;
            //check if the are an equal card
            for (int i = 0; i < table.tableCards.Count; i++)
            {
                for (int j = 0; j < hand.Count; j++)
                {
                    if (hand[j].value == table.tableCards[i].value)
                    {
                        equalcard = true;
                        index = j;
                        break;
                    }
                }
            }
            if (!equalcard)
            {
                int rnd = 0;
                rnd = Random.Range(0, hand.Count);
                playedCard = hand[rnd];
                //remove from list
                hand.RemoveAt(rnd);
            }
            else
            {

                playedCard = hand[index];
                hand.RemoveAt(index);
                index = 0;
            }
        }
        //use effet for delete images
        for (int i = 0; i < hand.Count - 1; i++)
        {
            index++;
        }
        //make green image
        hand_images[index].sprite = green;
        table.PlayCard(playedCard, this);

    }

     //take new card , for player
    public void DrawCardFromDeck(Card c,int index)
    {
        hand.Add(c);
        if (index > hand_images.Length) return;
        //return back if not is player
        Sprite s = null;
        if(isPlayer)
        {
            s = c.img;
        }
        else
        {
            s = back;
        }
        hand_images[index].sprite =s;
    }

    //for pc
    public void DrawCardFromDeck(Card c)
    {
        hand.Add(c);
    }

    //hand comeBacktoZero
    public void resetCardList()
    {
        hand.Clear();
    }

    public void CollectCard(Card c)
    {
        collectedCards.Add(c);
    }

    public void HilightPlayer(bool value)
    {
       hilightPlayer_img.enabled = value;
       
    }

    public void CleanImage()
    {
        UsedCard.sprite = green;
    }

}

