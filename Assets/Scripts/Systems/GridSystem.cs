using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private static GridSystem instance;

    public static GridSystem Instance
    {
        get
        {
            return instance;
        }
    }

    public Dictionary<Vector2, GameObject> itemList = new();

    public bool CheckPosition(Vector2 pos)
    {
        return itemList.ContainsKey(pos);
    }

    public bool RemoveItem(Vector2 pos)
    {
        if (CheckPosition(pos))
        {
            itemList.Remove(pos);
            //print("item removed");
            return true;
        }

        //print("item didnt removed");

        //print("-------------------------------------------");

        /*
        
        foreach (var item in itemList)
        {
            print(item);
        }
        
        */

        return false;
    }

    public bool AddItem(GameObject obj, Vector2 pos)
    {
        if (!CheckPosition(pos))
        {
            itemList.Add(pos, obj);
            return true;
        }

        return false;
    }

    public void ChangeItemsPlace(Vector2 oldPos, Vector2 newPos)
    {
        if (!CheckPosition(oldPos))
        {
            print("Anahtar koltuðun altýnda kalýk");
            return;
        }

        if (CheckPosition(newPos))
        {
            print("Doluuuu");
            return;
        }

        GameObject obj = itemList[oldPos];
        itemList.Remove(oldPos);
        itemList[newPos] = obj;

        //print("-------------------------------------------");
        
        //foreach (var item in itemList)
        //{
        //    print(item);
        //}
    }

    public GameObject TakeItem(Vector2 pos)
    {
        if (CheckPosition(pos))
            return itemList[pos];

        return null;
    }

    public GridSystem()
    {
        instance = this;
    }
}