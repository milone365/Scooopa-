using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoponeManager 
{
    Table table;
    int currentTurn = 0;

    public void INIT(Table t)
    {
        table = t;
    }
    public void goToNextTurn()
    {
        int endgame = 0;
        for(int i=0;i<table.allPlayers.Length;i++)
        {
            int num = table.allPlayers[i].getNumOfCard();
            if (num==0)
            {
                endgame ++;
            }
           
        }
        if(endgame>3)
        {
            table.goToTheEnd();
            return;
        }
        table.allPlayers[currentTurn].HilightPlayer(false);
        //go to next player
        currentTurn++;
        if(currentTurn>3)
        {
            currentTurn = 0;
        }
        table.currentPlayer = table.allPlayers[currentTurn];

        if (table.currentPlayer == table.player)
        {
            table.activePlayerButtons(true);
            
        }
        else
        {
            //player can not play card's on enemy turn
            table.activePlayerButtons(false);
            //reset timer
            table.allPlayers[currentTurn].PcPlayCard();
        }
        table.allPlayers[currentTurn].HilightPlayer(true);
       
    }

}
