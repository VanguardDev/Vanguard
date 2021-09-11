using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    /// <summary>
    /// A list of all panels to be managed by this controller.  The first panel in the list will be
    /// enabled on start, and the rest will be disabled
    /// </summary>
    [SerializeField] List<GameObject> panels;
    GameObject currentPanel;

    private void Start()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        panels[0]?.SetActive(true);
        currentPanel = panels[0];
    }

    public void ChangePanel(GameObject newPanel)
    {
        currentPanel?.SetActive(false);
        newPanel?.SetActive(true);
        currentPanel = newPanel;
    }
}
