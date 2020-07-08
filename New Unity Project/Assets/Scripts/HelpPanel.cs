using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    [SerializeField]
    GameObject panel=null;

    public void ActivePanel()
    {
        panel.SetActive(true);
    }
    public void DeactivePanel()
    {
        panel.SetActive(false);
    }
}
