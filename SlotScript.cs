using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject[,] passObjectArr;
    private Color32 green, red, yellow;


    public static int checkInt;

    public static IntVector2 itemSize = IntVector2.zero;
    private InventoryManager InvenManager;

    public IntVector2 gridPos;
    public GameObject storedItem;
    public IntVector2 storedItemSize;
    public IntVector2 itemPos; //start position of stored item
    public bool isOccupied = false;
    

    private void Start()
    {
        green = new Color32(180, 255, 180, 255);
        red = new Color32(255, 180, 180, 255);
        yellow = new Color32(255, 255, 180, 255);
        passObjectArr = PanelScript.slotGrid;
        InvenManager = this.transform.parent.GetComponent<InventoryManager>();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        checkInt = SlotCheck();
        switch (checkInt)
        {
            case 0:
                GridColorChange(green, true); break;
            case 1:
                GridColorChange(yellow, true); break;// improve later. make the other item glow yellow instead of selectedItem
            case 2:
                GridColorChange(red, true); break;
        }
        InvenManager.highlightedSlot = eventData.pointerEnter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GridColorChange(Color.white, false);
        InvenManager.highlightedSlot = null;
        if (InvenManager.otherItemPos != IntVector2.oneNeg) //potential bug
        {
            InvenManager.otherItemPos = IntVector2.oneNeg;
        }
        

    }


    public void GridColorChange(Color32 color, bool enter)
    {
        IntVector2 checkArea;

        checkArea.x = Mathf.Clamp(itemSize.x, 0, passObjectArr.GetLength(0) - gridPos.x);
        checkArea.y = Mathf.Clamp(itemSize.y, 0, passObjectArr.GetLength(1) - gridPos.y);

        //if checkArea is outside slotGrid
        if ((itemSize.x + gridPos.x > passObjectArr.GetLength(0)) || (itemSize.y + gridPos.y > passObjectArr.GetLength(1)))
        {
            if (enter)
            {
                color = red;
                checkInt = 2;
            }  
        }
        for (int y = 0; y < checkArea.y; y++)
        {
            for (int x = 0; x < checkArea.x; x++)
            {
                passObjectArr[x + gridPos.x, y + gridPos.y].GetComponent<Image>().color = color;
            }
        }
    }


    private int SlotCheck()
    {
        GameObject obj = null;
        SlotScript instanceScript;
        IntVector2 checkArea;

        checkArea.x = Mathf.Clamp(itemSize.x, 0, passObjectArr.GetLength(0) - gridPos.x);
        checkArea.y = Mathf.Clamp(itemSize.y, 0, passObjectArr.GetLength(1) - gridPos.y);


        for (int y = 0; y <checkArea.y; y++)
        {
            for (int x = 0; x < checkArea.x; x++)
            {
                instanceScript = passObjectArr[x + gridPos.x, y + gridPos.y].GetComponent<SlotScript>();
                if (instanceScript.isOccupied)
                {
                    if (obj == null)
                    {
                        obj = instanceScript.storedItem;
                        InvenManager.otherItemPos = instanceScript.itemPos;
                    }
                    else if (obj != instanceScript.storedItem)
                    {
                        return 2; // if checkArea has 1+ items occupied
                    }
                }
            }
        }
        
        if (obj == null) //if checkArea is not occupied
        {
            return 0;
        }
        else //if cheakArea only has 1 item ocupied
        {
            return 1;
        }
    }
    
}