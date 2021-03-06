using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorSelect : MonoBehaviour
{
    private GameObject[] floorList;
    private Hashtable floorHashtable = new Hashtable();
    [Tooltip("楼层弹出x的偏移量")] 
    public float xBais=100;
    [Tooltip("楼层弹出y的偏移量")]
    public float zBais=100;

    public float moveSpeed=10;

    private Vector3 destination;
    private Vector3 originalPos;
    private Transform lastFloor;


    // Start is called before the first frame update
    void Start()
    {

        UpdateFloorList();
        if (floorList.Length == 0) {
            Debug.Log("没有找到floor，请设置tag为floor");
        }
        lastFloor = floorList[0].transform;
        originalPos = floorList[0].transform.position;
        
    }
    //更新floorlist
    void UpdateFloorList()
    {
        floorList = GameObject.FindGameObjectsWithTag("floor");
        int i = 0;
        foreach(GameObject floor in floorList)
        {
           // Debug.Log(floor);
            floorHashtable.Add(i, floor);
            i++;
        }
    }
    void updateBias()
    {
        destination.x = originalPos.x + xBais;
        destination.z = originalPos.z + zBais;
    }
    // Update is called once per frame
    void Update()
    {
        updateBias();
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            resetFloor();
            
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
         
            floorPop(((GameObject)floorHashtable[0]).transform);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
           
            floorPop(((GameObject)floorHashtable[1]).transform);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            
            floorPop(((GameObject)floorHashtable[2]).transform);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {

            floorPop(((GameObject)floorHashtable[3]).transform);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {

            floorPop(((GameObject)floorHashtable[4]).transform);
        }

    }
    public void selectFloor(int i)
    {
        floorPop(((GameObject)floorHashtable[i-1]).transform);
    }
    public void resetFloor()
    {
        Vector3 v3 = originalPos;
        v3.y=lastFloor.position.y;
        lastFloor.position = v3;
    }
    void floorPop(Transform floor)
    {
        resetFloor();
        destination.y=floor.position.y;
        floor.position = destination;
        lastFloor = floor;
        //floor.position = Vector3.MoveTowards(floor.position, destination, moveSpeed * Time.deltaTime);
    }
}
