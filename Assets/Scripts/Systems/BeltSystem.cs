using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class BeltSystem : MonoBehaviour
{

    [SerializeField]
    private LayerMask _layerMask;

    public List<BeltPath> BeltPaths;

    public static event EventHandler OnBeltAdded;
    public static event EventHandler OnBeltRemoved;

    private void Start()
    {
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            UseAllBelts();
        };

        BeltPaths = new();
    }

    public static BeltSystem Instance
    {
        get
        {
            return _instance;
        }
    }

    private static BeltSystem _instance;

    public BeltSystem()
    {
        _instance = this;
    }

    /// <summary>
    /// Koyulan Conveyor Belt bir Path'in devamýna eklenirse Path'e ekler yoksa yeni bir Path oluþturur
    /// </summary>
    /// <param name="belt"></param>
    public void AddNewBelt2Path(ConveyorBelt belt)
    {
        Vector2 pos = belt.Origin;

        bool itemAdded = false;

        foreach (BeltPath path in BeltPaths)
        {
            Vector2 topPos = pos + new Vector2(0, 1);
            Vector2 downPos = pos + new Vector2(0, -1);
            Vector2 leftPos = pos + new Vector2(-1, 0);
            Vector2 rightPos = pos + new Vector2(1, 0);

            int topIndex = path.IsGridPositionPartOfBeltPath(topPos);
            int downIndex = path.IsGridPositionPartOfBeltPath(downPos);
            int leftIndex = path.IsGridPositionPartOfBeltPath(leftPos);
            int rightIndex = path.IsGridPositionPartOfBeltPath(rightPos);

            if (topIndex != -1)
            {
                //print("top");
                path.AddBelt2End(belt);
                itemAdded = true;
            }
            else if (downIndex != -1)
            {
                //print("down");
                path.AddBelt2End(belt);
                itemAdded = true;
            }
            else if (leftIndex != -1)
            {
                //print("left");
                path.AddBelt2End(belt);
                itemAdded = true;
            }
            else if (rightIndex != -1)
            {
                //print("right");
                path.AddBelt2End(belt);

                itemAdded = true;
            }
        }

        if (!itemAdded)
        {
            BeltPath path = new();
            path.AddBelt2End(belt);
            BeltPaths.Add(path);
        }
    }

    /// <summary>
    /// Taþýma bandýnýn dahil olduðu Path'in baþýna eþya ekler
    /// </summary>
    /// <param name="belt"></param>
    /// <param name="item"></param>
    public void AddNewItem2Path(ConveyorBelt belt, GameObject item)
    {
        Vector2 pos = belt.Origin;

        foreach (BeltPath path in BeltPaths)
        {
            if (path.IsGridPositionPartOfBeltPath(pos) != -1)
            {
                path.AddItemHead2List(item);
            }
        }
    }

    /// <summary>
    /// Conveyor Belt'in dahil olduðu Path'in herhangi bir noktasýna eþya ekler
    /// </summary>
    /// <param name="belt"></param>
    /// <param name="item"></param>
    public void AddNewItem2PathCenter(ConveyorBelt belt, GameObject item)
    {
        Vector2 pos = belt.Origin;

        foreach (BeltPath path in BeltPaths)
        {
            int beltIndex = path.IsGridPositionPartOfBeltPath(pos);

            if (beltIndex != -1)
            {
                path.AddItemCenter2List(beltIndex, item);
            }
        }
    }

    /// <summary>
    /// Verilen pozisyondaki taþýma bandýný path'den siler
    /// </summary>
    /// <param name="pos"></param>
    public void RemoveBeltFromPath(Vector2 pos)
    {
        foreach (BeltPath path in BeltPaths)
        {
            int beltIndex = path.IsGridPositionPartOfBeltPath(pos);

            if (beltIndex != -1)
            {
                path.beltList.RemoveAt(beltIndex);
            }
        }

        for (int i = 0; i < BeltPaths.Count; i++)
        {
            if (BeltPaths[i].beltList.Count <= 0)
            {
                BeltPaths.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Path'leri gezerek taþýma bantlarýný aktif eder
    /// </summary>
    public void UseAllBelts()
    {
        for (int i = 0; i < BeltPaths.Count; i++)
        {
            if (BeltPaths[i] == null)
            {
                print("boþtur");
                return;
            }

            BeltPaths[i].MoveItemsOnTheBelt();
        }
    }

    public class BeltPath
    {
        public List<ConveyorBelt> beltList;
        public List<IEnumerator> enumeratorList;

        public BeltPath()
        {
            beltList = new();
            enumeratorList = new();
        }

        /// <summary>
        /// Taþýma bantý ekler
        /// </summary>
        /// <param name="belt"></param>
        public void AddBelt2End(ConveyorBelt belt)
        {
            beltList.Add(belt);
            //beltItems.Add(null);
        }

        /// <summary>
        /// Path'deki ilk bandý bularak olduðu poziyona GridSystem'i kullanarak eþya ekler
        /// </summary>
        /// <param name="item"></param>
        public void AddItemHead2List(GameObject item)
        {
            if (GridSystem.Instance.TakeItem(beltList[0].Origin) == null)
            {
                item.GetComponent<ItemUser>().enabled = false;
                Rigidbody2D rigi = item.GetComponent<Rigidbody2D>();
                rigi.velocity = Vector2.zero;
                rigi.simulated = false;
                item.transform.rotation = Quaternion.Euler(0, 0, 0);
                item.transform.position = beltList[0].Origin;
                GridSystem.Instance.AddItem(item, beltList[0].Origin);
            }
        }

        /// <summary>
        /// Verilen taþýma bandý indisine eþya koyar
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void AddItemCenter2List(int index, GameObject item)
        {
            if (GridSystem.Instance.TakeItem(beltList[index].Origin) == null)
            {
                Rigidbody2D rigi = item.GetComponent<Rigidbody2D>();
                rigi.velocity = Vector2.zero;
                rigi.simulated = false;
                item.transform.rotation = Quaternion.Euler(0, 0, 0);
                item.transform.position = beltList[0].Origin;
                GridSystem.Instance.AddItem(item, beltList[index].Origin);
            }
        }

        /// <summary>
        /// Verilen pozisyonda taþýma bandýnýn olup olmadýðýný kontrol eder
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public int IsGridPositionPartOfBeltPath(Vector2 pos)
        {
            for (int i = 0; i < beltList.Count; i++)
            {
                if (beltList[i].GetGridPosition() == pos)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Taþýma bandýnýn üzerindeki eþyalarý taþýr
        /// </summary>
        public void MoveItemsOnTheBelt()
        {
            for (int i = beltList.Count - 1; i >= 0; i--)
            {
                if (beltList[i].moveForward)
                {
                    if (IsGridPositionPartOfBeltPath(beltList[i].GetNextPosition()) != -1)
                    {
                        beltList[i].TakeAction(true);
                    }
                }
                else
                {
                    beltList[i].TakeAction(true);
                }
            }
        }
    }
}