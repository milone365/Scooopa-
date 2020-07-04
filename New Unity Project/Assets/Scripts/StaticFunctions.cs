using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFunctions : MonoBehaviour
{
    //return equal card list index
    public static int getTableIndex(List<Card> tableCards, Card c)
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
    
    //return a single card to take if value is equal to played card value
    public static Card getEqualCard(List<Card>tableCards,int value)
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

    public static List<Card>getTakableCards(List<Card> tableCards, int valor)
    {
        List<Card> temp = new List<Card>();
        for(int i=0;i<tableCards.Count;i++)
        {
            for(int j=0;j<tableCards.Count;j++)
            {
               if (i == j) continue;
               int v1 = tableCards[i].value;
               int v2 = tableCards[j].value;
               if(v1+v2==valor)
                {
                    temp.Add(tableCards[i]);
                    temp.Add(tableCards[j]);
                    return temp;
                }
               for(int k=0;k<tableCards.Count;k++)
                {
                    if (i == j || i == k || k == j) continue;
                    int a1= tableCards[i].value;
                    int a2= tableCards[j].value;
                    int a3= tableCards[k].value;
                    if (a1 + a2 +a3 == valor)
                    {
                        temp.Add(tableCards[i]);
                        temp.Add(tableCards[j]);
                        temp.Add(tableCards[k]);
                        return temp;
                    }
                    for (int w = 0; w < tableCards.Count; w++)
                    {
                        if (i == j || i == k || k == j||
                            w==i||w==k||w==j) continue;
                        int b1 = tableCards[i].value;
                        int b2 = tableCards[j].value;
                        int b3 = tableCards[k].value;
                        int b4 = tableCards[w].value;
                        if (b1 + b2 + b3 +b4 == valor)
                        {
                            temp.Add(tableCards[i]);
                            temp.Add(tableCards[j]);
                            temp.Add(tableCards[k]);
                            temp.Add(tableCards[w]);
                            return temp;
                        }

                    }
                }
            }
        }

        return null;
    }
}
