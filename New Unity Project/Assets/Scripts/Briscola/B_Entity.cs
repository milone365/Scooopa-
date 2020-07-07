using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_Entity : MonoBehaviour
{
    public bool isPlayer = false;
    public bool ally = false;
    public List<Card> hand;
    [HideInInspector]
    public string Briscola;
    Image[] images;
    public Sprite green;
    B_Table table;
    [SerializeField]
    Image playedCard_img=null;
    [SerializeField]
    Sprite back=null;
    [SerializeField]
    Image border=null;
    Button[] buttons;
    public Card playedCard;
    public List<Card> collectedCards;
    [HideInInspector]
    public bool lastTurn = false;
    [HideInInspector]
    public int countdown = 3; 
    //public Card 
    public void INIT(B_Table t)
    {
        images = GetComponentsInChildren<Image>();
        buttons = GetComponentsInChildren<Button>();
        table = t;
        HilightBorder(false);
    }
    public void AddCardToHand(Card c)
    {
       hand.Add(c);
       updateImages();
    }
  
    //change images to the cards
    void updateImages()
    {
        for (int i = 0; i < hand.Count; i++)
        {
           
            if (isPlayer)
            {
                images[i].sprite = hand[i].img;
            }
            else
            {
                
                images[i].sprite = back;
            }
        }
    }

    //use card
    public void PlayCard(int i)
    {
        if (images[i] == null) return;
        playedCard = hand[i];
        playedCard.playerRef = this;
        //set image
        playedCard_img.sprite = hand[i].img;
        //remove from list
        if(!lastTurn)
        {
            //if not is the last turn 
            hand.RemoveAt(i);
            //updateImages();
            deactiveButtons();
            images[i].sprite = green;
        }
        else
        {
            countdown--;
            //if not is the last turn 
            Destroy(images[i].gameObject);
        }
        
        //send to table
        table.PlayCard(this, playedCard);
        
    }
    public void PcPlayCard()
    {
        int rnd = Random.Range(0, hand.Count);
        if (hand.Count<1)
        {
            return;
        }
        playedCard = hand[rnd];
        playedCard.playerRef = this;
        //set image
        playedCard_img.sprite = hand[rnd].img;
        //remove from list
        hand.RemoveAt(rnd);
        images[rnd].sprite = green;
        //send to table
        table.PlayCard(this, playedCard);
        
    }
    public void SetBriscola(string s)
    {
        Briscola = s;
    }
    
    public void activeButtons()
    {
        for(int i=0;i<buttons.Length;i++)
        {
            if(buttons[i]!=null)
            buttons[i].enabled = true;
        }
    }
    void deactiveButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].enabled = false;
        }
    }
    //reset image of played card
   public void resetPlayerCard() { playedCard_img.sprite =green; }
   public void HilightBorder(bool var)
    {
        border.enabled = var;
    }

    public int getPoints()
    {
        int value = 0;
        foreach(var item in collectedCards)
        {
            value += StaticFunctions.ConvertToB_Point(item.value);
        }
        return value;
    }
}
