using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ElementPrefab;

    public void SetupInventory(List<Level.ContraptionInventoryCount> inventory)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.transform.parent = null; // Become Batman!
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (var item in inventory)
        {
            if(item.Count == 0)
            {
                continue;
            }
            var elementGO = Instantiate(ElementPrefab, gameObject.transform);
            InventoryElement ie = elementGO.GetComponent<InventoryElement>();

            ie.Setup(item.ContraptionName, item.ContraptionSprite, item.Count);
        }
    }
}
