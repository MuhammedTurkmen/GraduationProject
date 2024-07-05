using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : PlacedObject
{
    private Vector2 _grabPosition, _releasePosition;

    public Directions storageSide;
    AutomaticMachine machine;

    public List<GameObject> storedObj;
    public List<ItemSO> storedItems;

    public float maxItemFlushWaitTime, itemFlushWaitTime;

    public int maxItemCount = 5;

    public GameObject ItemUserPrefab;
    public Transform ItemParent;

    private void Start()
    {
        storedObj = new();
        storedItems = new();

        machine = transform.GetComponentInParent<AutomaticMachine>();

        _grabPosition = GetPreviousPosition();
        _releasePosition = 
            transform.localPosition.y > 1 ? 
                transform.position + transform.up - transform.right :
                transform.position - transform.up - transform.right
            ;

        // TimeTickSystemi kullarak 'TryTakeItem' methodunu düzenli aralýklarla çaðýrýr
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            TryTakeItem();
        };
}

    public bool storageIsFull;

    /// <summary>
    /// Oyunun her karesinde çaðýrýlan 'Update' methodu ile gereksiz eþyalarý kontrol eder ve belli bir süre geçtikten sonra siler
    /// </summary>
    private void Update()
    {
        if (storedItems.Count > 0)
        {
            itemFlushWaitTime += Time.deltaTime;

            if (itemFlushWaitTime > maxItemFlushWaitTime)
            {
                int pos = 1;

                for (int i = 0; i < storedItems.Count; i++)
                {
                    GameObject itemUser = Instantiate(ItemUserPrefab, ItemParent);
                    itemUser.transform.position = _releasePosition;
                    itemUser.GetComponent<ItemUser>()._scriptableItem = storedItems[i];
                    itemUser.SetActive(true);
                    pos++;
                    storedItems.RemoveAt(i);
                    storageIsFull = false;
                }
                DestroyItems();
            }
        }
    }

    /// <summary>
    /// Listeye eklenen eþyalarý yok eder
    /// </summary>
    public void DestroyItems()
    {
        if (storedObj.Count <= 0)
            return;

        for (int i = 0;i < storedObj.Count;i++) 
        {
            Destroy(storedObj[i]);
        }

        storedObj.Clear();
        storedItems.Clear();
        storageIsFull = false;
    }

    /// <summary>
    /// Taþýma bandýndan gelen eþyalarý bazý koþullara göre depolar
    /// </summary>
    public void TryTakeItem()
    {
        if (machine._creatableItem != null || machine._machineProcessTypes[0] == ProcessType.None)
            return;

        if (storageIsFull || GridSystem.Instance.TakeItem(Origin) != null)
            return;

        GameObject obj = GridSystem.Instance.TakeItem( _grabPosition );

        if (obj != null)
        {
            Vector2 pos =
                    new(
                        Mathf.FloorToInt(obj.transform.position.x) + 0.5f,
                        Mathf.FloorToInt(obj.transform.position.y) + 0.5f
                    );

            GridSystem.Instance.RemoveItem(pos);
            obj.SetActive( false );
            storedObj.Add( obj );
            itemFlushWaitTime = 0;
            storedItems.Add( obj.GetComponent<ItemUser>()._scriptableItem );
        }
        
        if (storedItems.Count >= maxItemCount)
        {
            storageIsFull = true;
        }
    }
}