using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_UI : MonoBehaviour
{
    [SerializeField]
    Image lastCardImage=null;
    [SerializeField]
    Image deckImage=null;
    
    public void setLastCardImage(Sprite s)
    {
        lastCardImage.sprite = s;
    }
    public void clearImages()
    {
        lastCardImage.enabled=false;
        deckImage.enabled=false;
    }
    public void ShowEnd()
    {
        Debug.Log("END");
    }
    public void MultiplayerEnd()
    {

    }
    public void callingEnd()
    {

    }
}
