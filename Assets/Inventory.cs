using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform slotGrid;
    public GameObject slotPrefab;

    public void AddItem(string itemName)
    {
        GameObject slot = Instantiate(slotPrefab, slotGrid);
        slot.GetComponentInChildren<TextMeshProUGUI>().text = itemName;
    }
}