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
        Card aiCard = ChoseCard_WithAI();
        //if selected card is null chose random
        if(aiCard==null)
        {
            aiCard = hand[rnd];
        }
        //caluculte card position
        int index = getCardIndex(aiCard);
        playedCard = aiCard;
        playedCard.playerRef = this;
        //set image
        playedCard_img.sprite = aiCard.img;
        //remove from list
        hand.RemoveAt(index);
        images[index].sprite = green;
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

    Card  ChoseCard_WithAI()
    {
        int groundPoints = StaticFunctions.GetGroundPoints(table.groundCards);
        bool briscolaOnField = StaticFunctions.GetBriscolaOnField(table.groundCards, Briscola);
        
        if(groundPoints>10)
        {
            if(briscolaOnField)
            {
                //return most hight card if are more points
                if(groundPoints>15)
                {
                   Card c= GetCommanderSeed(table.groundCards,Briscola);
                    //if have briscola play that or use not point card
                    if(c!=null)
                    {
                        return c;
                    }
                    else
                    {
                        return getNotPointCard();
                    }
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                //return commander if have it or return briscola
                Card commander = GetCommanderSeed(table.groundCards, table.commander);
                if(commander!=null)
                {
                    return commander;
                }
                else
                {
                    Card br= getBriscolaFromHand();
                    if(br!=null)
                    {
                        return br;
                    }
                    else
                    {
                        return getNotPointCard();
                    }
                }
            }
        }
        return null;
    }

    //select briscola card from hand
    Card getBriscolaFromHand()
    {
        foreach(var item in hand)
        {
            if(item.seed==Briscola)
            {
                return item;
            }
        }
        return null;
    }
    //select not point card from hand
    Card getNotPointCard()
    {
        foreach (var item in hand)
        {
            int pt = StaticFunctions.ConvertToB_Point(item.value);
            if (pt == 0)
            {
                return item;
            }
        }
        return null;
    }
     
    //select top card with commander seed
    Card GetCommanderSeed(List<Card>ground,string s)
    {
        //create temporary list of commander card
        List<Card> commanders = new List<Card>();
        foreach(var item in hand)
        {
            if(item.seed==s)
            {
                commanders.Add(item);
            }
        }
        //confront two list
        for(int i=0;i<ground.Count;i++)
        {
            for(int j=0;j<commanders.Count;j++)
            {
                int comVal = StaticFunctions.ConvertToB_Point(commanders[j].value);
                int tbVal= StaticFunctions.ConvertToB_Point(ground[i].value);
                //return most height briscola value
                if(comVal>tbVal)
                {
                    return commanders[j];
                }
                //if value of card is zero return normal heighter card
                else if(comVal==tbVal)
                {
                    comVal = commanders[j].value;
                    tbVal = ground[i].value;
                    if(comVal>tbVal)
                    {
                        return commanders[j];
                    }
                }
            }
        }
        return null;
    }
    int getCardIndex(Card c)
    {
        for(int i=0;i<hand.Count;i++)
        {
            if(c==hand[i])
            {
                return i;
            }
        }
        return 0;
    }
}
