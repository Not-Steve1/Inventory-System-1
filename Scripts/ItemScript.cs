using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemScript : MonoBehaviour ,IPointerClickHandler, IPointerExitHandler,IPointerEnterHandler{

    public int itemL, itemW;   
    public IntVector2 itemSize;

    
    private GameObject highlightedItem = null;
    public static GameObject selectedItem = null;
    private CanvasGroup itemCanvas;
    public static bool isDragging = false;
    private float slotSize;
    private Vector3 dragOffset;

    private void Start()
    {
        itemSize = new IntVector2(itemL, itemW);
        itemCanvas = GetComponent<CanvasGroup>();
        slotSize = GameObject.Find("InvenPanel").GetComponent<PanelScript>().slotSize;
        dragOffset = new Vector3(-slotSize / 2, slotSize / 2, 0);
    }

    //drag selected item
    private void Update()
    {
        if (isDragging)
        {
            selectedItem.transform.position = Input.mousePosition + dragOffset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightedItem = this.gameObject;      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedItem = null;
    }

    //mouse pick up item
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((highlightedItem != null) &&(selectedItem == null))
        {
            selectedItem = highlightedItem;
            isDragging = true;
            SlotScript.itemSize = itemSize;
            itemCanvas.alpha = 0.5f;
            itemCanvas.blocksRaycasts = false;
            
        }
        
    }

    
}
