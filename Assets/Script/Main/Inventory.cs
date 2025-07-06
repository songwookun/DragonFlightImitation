using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;

    public void AddItem(string itemName)
    {
        GameObject obj = Instantiate(slotPrefab, slotParent);
        TMP_Text text = obj.GetComponentInChildren<TMP_Text>();
        text.text = itemName;
    }
}
