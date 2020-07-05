using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopaManager
{ 
    Table table;
    int currentTurn = 0;

    public void Init(Table t)
    {
        table =t;
    }
    //change turn
    public void goToNextTurn()
    {
        //if players don't have more card draw 3 
        if (table.player.getNumOfCard() == 0 && table.pc.getNumOfCard() == 0)
        {
            table.deck.newTurn();
        }
        currentTurn++;
        if (currentTurn % 2 == 0)
        {
            //enable button for play cards
            table.currentPlayer = table.player;
            table.activePlayerButtons(true);
        }
        else
        {
            table.currentPlayer = table.pc;
            //player can not play card's on enemy turn
            table.activePlayerButtons(false);
            pcTurnManagement();
        }
    }

    void pcTurnManagement()
    {
        //reset timer
        table.pc.PcPlayCard();
    }
}
