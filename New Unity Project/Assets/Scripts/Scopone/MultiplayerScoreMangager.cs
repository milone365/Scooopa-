using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MultiplayerScoreMangager : MonoBehaviour
{
    Entity teamPlayer;
    Entity teamPc;
    List<Card> playerCards = new List<Card>();
    List<Card> pcCards = new List<Card>();
    int playerScore = 0;
    int pcScore = 0;
    UI_Manager manager;
    private void Start()
    {
        manager = FindObjectOfType<UI_Manager>();
        manager.INIT();
    }

    public void ShowScores(Table t)
    {
        //create a single group for 2 player
       foreach(var item in t.allPlayers)
        {
            if(item==t.player||item==t.ally)
            {
                playerCards.AddRange(item.collectedCards);
                //add scopa points
                playerScore += item.scopa;
            }
            else
            {
                pcCards.AddRange(item.collectedCards);
                //add scopa points
                pcScore += item.scopa;
            }
        }
        //add gold points
        playerScore += getGolds(playerCards);
        pcScore += getGolds(pcCards);
        //add cards points
        playerScore += getCount(playerCards);
        pcScore += getCount(pcCards);
        //
        PremiereCheck();
        //active panel adn show text
        if (manager == null) return;
        manager.openEndPanel();
        manager.playerScoreTxt.text = playerScore.ToString();
        manager.pcScoreTxt.text = pcScore.ToString();
    }
    int getCount(List<Card> cards)
    {
        int r=0;
        if(cards.Count>20)
        {
            r = 1;
        }
        return r;
    }
    int getGolds(List<Card>cards)
    {
        List<Card>temp=StaticFunctions.getAllCardOfSeed(cards, StaticStrings.gold);
        //sort list for valor
        Card[] lista =temp.OrderBy(x => x.value).ToArray();
        int value = 0;
        if (temp.Count>0)
        {
            int scale =1;
            for (int i=0;i<lista.Length;i++)
            {
                //count scale
                if(scale==lista[i].value)
                {
                    scale++;
                }
                //get 7gold point
                if(7== lista[i].value)
                {
                    value++;
                }
            }
            //add scale ponts
            if(scale>=3)
            {
                value += scale;
            }
            //if have more gold than opponet add point
            if(lista.Length>5)
            {
                value++;
            }
        }
       
        return value;
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
        int playerScore = calcualtePremiere(playerCards);
        int pcScore = calcualtePremiere(pcCards);
        if (playerScore > pcScore)
        {
            playerScore++;
        }
        else if (playerScore < pcScore)
        {
            pcScore++;
        }
    }
}
