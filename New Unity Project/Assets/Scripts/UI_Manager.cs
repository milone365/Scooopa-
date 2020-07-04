using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject helpPanel=null;
    [SerializeField]
    GameObject premierePanel = null;

    private void Start()
    {
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

    public void closeHelpPanel()
    {
        helpPanel.SetActive(false);
    }

    public void closePremierePanel()
    {
        premierePanel.SetActive(false);
    }

}
