using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelScript : MonoBehaviour
{
    /*to do list
     * add panels (done)
     * dynamic inventory functions (done)
     * make test items (done)
     * move items   (done)  
     * drop mechanics   (done)
     * improve move items mechanics (done)
     * make scroll list UI for items
     * drop items back list
     * create item generator
     * add  delete item panel
     * add item stat
     * add item stat overlay
     * make random item generator *later*
     *
     * have item stat affect player stats *later*
     * create random item stat *maybe* *hard*
     *
     * 
     */

    /*optionals
     * add graphics
     * item rotate
     * save/load function *hard*
     * improve IntVector2 methods and parameters
     */



    //public static List<GameObject> //may of use later
    public static GameObject[,] slotGrid;
    public GameObject slotPrefab;
    public GameObject anchorPrefab;
    
    public int row, column;
    public float slotSize;
    public float edgePadding;


    //test vars
    

    public void Awake()
    {
        slotGrid = new GameObject[column, row];
        ResizePanel(this.gameObject);
        CreateSlots();
        ResizePanel(anchorPrefab);
    }

   



    private void CreateSlots()
    {
        GetComponent<GridLayoutGroup>().cellSize = Vector2.one * slotSize;
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++)
            {

                slotGrid[x, y] = (GameObject)Instantiate(slotPrefab);          
                slotGrid[x, y].transform.name = "slot[" + x + "," + y + "]";
                slotGrid[x, y].transform.SetParent(this.transform);
                slotGrid[x, y].GetComponent<RectTransform>().localScale = Vector3.one;
                slotGrid[x, y].GetComponent<SlotScript>().gridPos = new IntVector2(x, y);
            }
        }
        
    }

    private void ResizePanel(GameObject obj)
    {
        float width, height;
        width = (column * slotSize) + (obj == this.gameObject ? (edgePadding * 2):0);
        height = (row * slotSize) + (obj == this.gameObject ? (edgePadding * 2) : 0);
        
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        rect.localScale = Vector3.one;
    }
}
