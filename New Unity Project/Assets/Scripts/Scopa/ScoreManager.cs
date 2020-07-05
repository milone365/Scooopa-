using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    Entity player;
    Entity pc;
    
    public int playerPoints = 0;
    public int pcPoints;
    

    private void Start()
    {
        player = FindObjectOfType<Table>().player;
        pc = FindObjectOfType<Table>().pc;
    }

    //calcualte all points but not calculate premiere,that is in a different function  
    public void calculatePoints(Entity e)
    {
        List<Card> collected = e.collectedCards;
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
    int calcualtePremiere(List<Card> collected)
    {
        int point = 0;
        List<Card> golds = StaticFunctions.getAllCardOfSeed(collected, StaticStrings.gold);
        List<Card> cup = StaticFunctions.getAllCardOfSeed(collected, StaticStrings.cup);
        List<Card> sword = StaticFunctions.getAllCardOfSeed(collected, StaticStrings.sword);
        List<Card> wand = StaticFunctions.getAllCardOfSeed(collected, StaticStrings.wand);
        point += StaticFunctions.getHeightersPoint(golds);
        point += StaticFunctions.getHeightersPoint(cup);
        point += StaticFunctions.getHeightersPoint(sword);
        point += StaticFunctions.getHeightersPoint(wand);
        return point;
    }
    //premier check
    public void PremiereCheck()
    {
        int playerScore = calcualtePremiere(player.collectedCards);
        int pcScore = calcualtePremiere(pc.collectedCards);
        if(playerScore>pcScore)
        {
            playerPoints++;
        }
        else if(playerScore < pcScore)
        {
            pcPoints++;
        }
    }

    
}
