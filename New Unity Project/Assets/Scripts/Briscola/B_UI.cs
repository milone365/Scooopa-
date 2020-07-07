using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class B_UI : MonoBehaviour
{
    public GameObject endPanel;
    [SerializeField]
    Image lastCardImage=null;
    [SerializeField]
    Image deckImage=null;
    public Text playerScoreTxt;
    public Text pcScoreTxt;
    B_Entity[] allplayers;
    public void setLastCardImage(Sprite s)
    {
        lastCardImage.sprite = s;
    }
    public void clearImages()
    {
        lastCardImage.enabled=false;
        deckImage.enabled=false;
    }
    int playerScore = 0;
    int pcScore = 0;

    private void Start()
    {
        endPanel.SetActive(false);
        setPlayersRef();
    }
    public void ShowEnd()
    {
        
        foreach (var item in allplayers)
        {
            if(item.isPlayer)
            {
                playerScore = item.getPoints();
                playerScoreTxt.text = "Player Score: " + playerScore;
            }
            else
            {
                pcScore = item.getPoints();
                pcScoreTxt.text = "Pc Score: " + pcScore;
            }
        }
        endPanel.SetActive(true);
    }

    public void MultiplayerEnd()
    {
        foreach (var item in allplayers)
        {
            if (item.isPlayer|| item.ally)
            {
                playerScore += item.getPoints();
                playerScoreTxt.text = "Player Team Score: " + playerScore;
            }
            else
            {
                pcScore += item.getPoints();
                pcScoreTxt.text = "Pc Team Score: " + pcScore;
            }
        }
        endPanel.SetActive(true);
    }
    

    public void LoadStage(string s)
    {
        SceneManager.LoadScene(s);
    }
    public void setPlayersRef()
    {
        allplayers = FindObjectsOfType<B_Entity>();
    }
}
