using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    Entity player;
    Entity pc;
    string winner = "Pc";
    int playerPoints = 0;
    int pcPoints;
    private void Start()
    {
        player = FindObjectOfType<Table>().player;
        pc = FindObjectOfType<Table>().pc;
    }

    //calcualte all points but not calculate premiere,that is in a different function  
    public void calculatePoints(Entity e,List<Card>collected)
    {
        int totalscore = 0;
        //adding scopa point
        totalscore += e.scopa;
        //adding gold points
        totalscore += getGolds(collected);
        //adding cards point
        totalscore += getCardsPoint(collected);
        if(e==player)
        {
            playerPoints += totalscore;
        }
        else
        {
            pcPoints += totalscore;
        }
    }
     //caluclate point for gold seed
    int getGolds(List<Card> collected)
    {
        List<Card> golds = StaticFunctions.getAllCardOfSeed(collected, StaticStrings.gold);
        int points = 0;
        if(golds.Count>5)
        {
            points++;
        }
        foreach(var c in golds)
        {
            if(c.value==7)
            {
                points++;
            }
        }
        return points;
    }
    //get point if have more cards of opponent
    int getCardsPoint(List<Card> collected)
    {
        int point = 0;
        int cards = StaticFunctions.getCountOfCards(collected);
        if(cards>20)
        {
            point++;
        }
        return point;
    }
    //
    //premiere
    public void calcualtePremiere()
    {

    }

}
