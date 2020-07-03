using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    public List<Card> tableCards = new List<Card>();
    Image[] tablecards_images;
    // Start is called before the first frame update
    void Start()
    {
        tablecards_images = GetComponentsInChildren<Image>();
        foreach(var item in tablecards_images)
        {
            item.enabled = false;
        }
    }

    void Update()
    {
        
    }

    public void addCardToTable(Card c)
    {
        Card newcard = c;
        tableCards.Add(newcard); 
    }
}
