using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private GameObject[,] passObjectArr;
    public GameObject highlightedSlot;

    private int checkInt;
    public IntVector2 otherItemPos = IntVector2.zero;

    private void Start()
    {
        passObjectArr = PanelScript.slotGrid;
        
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            checkInt = SlotScript.checkInt;
            if (highlightedSlot != null && ItemScript.selectedItem != null) //check if drop/swap
            {
                switch (checkInt)
                {
                    case 0: //store on empty slots
                        StoreItem(ItemScript.selectedItem);
                        ItemScript.isDragging = false;
                        ItemScript.selectedItem = null;
                        SlotScript.itemSize = IntVector2.zero;
                        break;
                    case 1: //swap items
                        ItemScript.selectedItem = SwapItem(ItemScript.selectedItem);
                        ItemScript.selectedItem.GetComponent<CanvasGroup>().alpha = 0.5f;
                        ItemScript.isDragging = true;
                        break;
                }
            }
            else if (highlightedSlot != null && ItemScript.selectedItem == null && highlightedSlot.GetComponent<SlotScript>().storedItem != null) //check if retrieving item
            {
                ItemScript.selectedItem = GetItem(highlightedSlot);
                ItemScript.selectedItem.GetComponent<CanvasGroup>().alpha = 0.5f;
                ItemScript.isDragging = true;
            }
        }
    }

    private void StoreItem(GameObject item)
    {
        SlotScript instanceScript;
        IntVector2 itemSize = ItemScript.selectedItem.GetComponent<ItemScript>().itemSize;
        IntVector2 gridPos = highlightedSlot.GetComponent<SlotScript>().gridPos;

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                //set each slot parameters
                instanceScript = passObjectArr[x + gridPos.x, y + gridPos.y].GetComponent<SlotScript>();
                instanceScript.storedItem = item;
                instanceScript.storedItemSize = itemSize;
                instanceScript.itemPos = gridPos;
                instanceScript.isOccupied = true;
                passObjectArr[x + gridPos.x, y + gridPos.y].GetComponent<Image>().color = Color.white;

                ItemScript.selectedItem.transform.SetParent(GameObject.Find("ItemAnchor").transform);
                ItemScript.selectedItem.transform.position = highlightedSlot.transform.position;
                ItemScript.selectedItem.GetComponent<CanvasGroup>().alpha = 0.75f;
            }
        }

    }

    private GameObject GetItem(GameObject slotObject)
    {
        SlotScript highlightedItemScript = slotObject.GetComponent<SlotScript>();
        GameObject retItem = highlightedItemScript.storedItem;

        IntVector2 itemSize = highlightedItemScript.storedItemSize;
        IntVector2 itemPos = highlightedItemScript.itemPos;
        SlotScript.itemSize = itemSize;

        SlotScript instanceScript;

        for (int y = 0; y < itemSize.y; y++)
        {
            for (int x = 0; x < itemSize.x; x++)
            {
                //reset each slot parameters
                instanceScript = passObjectArr[x + itemPos.x, y + itemPos.y].GetComponent<SlotScript>();
                instanceScript.storedItem = null;
                instanceScript.storedItemSize = IntVector2.zero;
                instanceScript.itemPos = IntVector2.zero;
                instanceScript.isOccupied = false;
            }
        }
        if (checkInt == 0)
        {
            slotObject.GetComponent<SlotScript>().GridColorChange(new Color32(180, 255, 180, 255), true);
        }
        
        return retItem;
    }

    private GameObject SwapItem(GameObject item)
    {
        GameObject tempItem;
        tempItem = GetItem(passObjectArr[otherItemPos.x ,otherItemPos.y]); //retrieve and store item on temp
        StoreItem(item); //store selectedItem
        return tempItem;
    }


}
