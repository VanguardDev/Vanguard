using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    GameObject currentPanel;

    private void Start()
    {
        startPanel?.SetActive(true);
        currentPanel = startPanel;
    }

    public void ChangePanel(GameObject newPanel)
    {
        currentPanel?.SetActive(false);
        newPanel?.SetActive(true);
        currentPanel = newPanel;
    }
}
