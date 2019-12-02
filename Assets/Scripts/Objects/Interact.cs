using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interact : MonoBehaviour
{
    public string MessageToDisplay = "";
    public Text messagePanel;
    bool m_IsAvailable = false;
    protected GameObject player = null;

    public bool IsAvailable()
    {
        return m_IsAvailable;
    }

    protected void Start()
    {
        gameObject.tag = "Interactable";
        m_IsAvailable = false;
    }
    
    public void DisplayMessage(bool forceRefresh=false) { if(messagePanel.text == "" || forceRefresh) messagePanel.text = MessageToDisplay; }
    public void HideMessage() { if (messagePanel.text == MessageToDisplay) messagePanel.text = ""; }
    
    protected void OnTriggerStay(Collider collider)
    {

        if (collider.CompareTag("Player"))
        {
            m_IsAvailable = true;
            DisplayMessage();
            player = collider.gameObject;
        }
    }

    protected void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            m_IsAvailable = false;
            HideMessage();
            player = null;
        }
    }
}