using System.Linq;
using UnityEngine;

public class AutomaticMachine : Crafter
{
    public ProcessType[] _machineProcessTypes;

    private ItemStorage _leftStorage, _rightStorage;

    private void Start()
    {
        // Giri� 1
        _leftStorage = transform.GetChild(6).GetChild(0).GetComponent<ItemStorage>();
        // Giri� 2
        _rightStorage = transform.GetChild(6).GetChild(1).GetComponent<ItemStorage>();

        _leftStorage.SetOrigin();
        _rightStorage.SetOrigin();

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            CallProcessRevealing(_leftStorage);
            CallProcessRevealing(_rightStorage);
        };
    }

    public void CallProcessRevealing(ItemStorage storage)
    {
        if (_machineProcessTypes[0] == ProcessType.None)
            return;

        _placedItems = storage.storedItems.ToList();

        if (GridSystem.Instance.CheckPosition(storage.Origin))
        {
            return;
        }

        for (int i = 0; i < _machineProcessTypes.Count(); i++)
        {
            ProcessRevealing(_machineProcessTypes[i]);
        }

        if (_creatableItem == null)
            return;

        _leftStorage.DestroyItems();
        _leftStorage.storageIsFull = false;

        GameObject item = Instantiate(ItemUserPrefab, ItemsParent);
        item.transform.position = storage.Origin;
        ItemUser itemUser = item.GetComponent<ItemUser>();
        item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        itemUser._scriptableItem = _creatableItem;
        GridSystem.Instance.AddItem(item, storage.Origin);
        item.SetActive(true);

        ResetItems();
    }

    /// <summary>
    /// 'TimeTickSystem' taraf�ndan belirli aral�klarla �a��r�larak e�ya tariflerini kontrol eden d�ng�y� �a��r�r
    /// </summary>
    /// <param name="createPos"></param>
    public void TryCreateItem()
    {
        //CallProcessRevealing(_leftStorage.Origin);
    }
}