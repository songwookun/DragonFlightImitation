using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject mailPanel;
    public GameObject inventoryPanel;

    public void ToggleMail()
    {
        mailPanel.SetActive(!mailPanel.activeSelf);
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
    public void CloseMail()
    {
        mailPanel.SetActive(false);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

}