using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI_Manager : MonoBehaviour
{
    ScoreManager scoremanager;
    public string LevelToLoad = "";
    [SerializeField]
    GameObject helpPanel=null;
    [SerializeField]
    GameObject premierePanel = null;
    [SerializeField]
    GameObject endPanel = null;
    //
    public Text playerScoreTxt;
    public Text pcScoreTxt;
    public Text winner;
    //
    public void INIT()
    {
        scoremanager = FindObjectOfType<ScoreManager>();
        closeEndPanel();
        closePremierePanel();
        closeHelpPanel();
    }

    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void OpenPremierePanel()
    {
        premierePanel.SetActive(true);
    }
    //
    public void closeHelpPanel()
    {
        if (helpPanel != null)
            helpPanel.SetActive(false);
    }

    public void closePremierePanel()
    {
        if(premierePanel!=null)
        premierePanel.SetActive(false);
    }
    //
    public void closeEndPanel()
    {
        if(endPanel!=null)
        endPanel.SetActive(false);
    }
    public void openEndPanel()
    {
        if (endPanel != null)
            endPanel.SetActive(true);
    }
    //
    public void EndGamePanel()
    {
        openEndPanel();
        int ps = scoremanager.playerPoints;
        int pc_s= scoremanager.pcPoints;
        playerScoreTxt.text = ps.ToString();
        pcScoreTxt.text = pc_s.ToString();
        if (ps>pc_s)
        {
            winner.text = "Winner is Player";
        }
        else if(pc_s >ps)
        {
            winner.text = "Winner is Pc";
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(LevelToLoad);
    }

    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
