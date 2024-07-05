using UnityEngine;

public class ItemGenerator : PlacedObject, IItemStorage
{
    [SerializeField]
    private GameObject _itemUserPrefab;

    [SerializeField]
    private Transform _itemsParent;

    [SerializeField]
    private ItemSO _itemType;

    private int itemCount = 5; 

    private int tick;

    [SerializeField]
    private int maxItemCount, maxItemCreateTick;

    private void Start()
    {
        SetOrigin();
        SetCheckPosition();

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            GenerateItem();
        };
    }

    private Vector2 checkPos;
    private GameObject itemUser;

    public void SetCheckPosition()
    {
        checkPos = new Vector2(Origin.x + 1, Origin.y);
    }

    private void GenerateItem()
    {
        if (itemCount < maxItemCount)
        { 
            tick++;

            if (tick >= maxItemCreateTick)
            {
                tick = 0;
                itemCount++;
            }
        }

        if (!GridSystem.Instance.CheckPosition(checkPos))
        {
            if (itemCount > 0)
            {
                itemCount--;

                itemUser = Instantiate(_itemUserPrefab, _itemsParent);
                itemUser.GetComponent<ItemUser>()._scriptableItem = _itemType;
                itemUser.gameObject.transform.position = checkPos;
                itemUser.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

                GridSystem.Instance.AddItem(itemUser, checkPos);
            }
        }
    }

    public ItemSO TakeItem()
    {
        throw new System.NotImplementedException();
    }

    public bool CheckItem()
    {
        return itemCount > 0;
    }
}