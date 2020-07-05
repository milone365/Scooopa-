using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Entity : MonoBehaviour
{
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
        //
        table.PlayCard(playedCard, this);
    }
    
    public void PcPlayCard()
    {
        
        int rnd =0;
        rnd = Random.Range(0, hand.Count);
        playedCard = hand[rnd];
        int index = 0;
        //use effet for delete images
        for(int i=0;i<hand.Count-1;i++)
        {
            index++;
        }
        //remove from list
        hand.RemoveAt(rnd);
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
}

