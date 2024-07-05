using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crafter : PlacedObject
{
    [SerializeField]
    internal ItemSO[] _creatableItems;

    internal ItemSO _creatableItem;
    internal List<ItemSO> _requiredItems;

    internal int _requiredItemsCount, _requiredItemsTimer, _requiresProcessTypesTimer;

    [SerializeField]
    internal GameObject ItemUserPrefab;

    [SerializeField]
    internal Transform ItemsParent;

    internal List<ItemSO> _placedItems = new();
    internal List<GameObject> _placedObj = new();

    public void ProcessRevealing(ProcessType selectedProcessType)
    {
        if (_creatableItems.Length <= 0)
        {
            print("Yapýlabilecek eþya yok");
        }

        for (int a = 0; a < _creatableItems.Length; a++)
        {
            _requiredItems = _creatableItems[a].RequiredItems.ToList();
            _requiredItemsCount = _requiredItems.Count;
            _requiredItemsTimer = 0;
            _requiresProcessTypesTimer = 0;

            if (_requiredItemsCount == 0)
            {
                print("Tarif Yok");
            }

            for (int b = 0; b < _placedItems.Count; b++)
            {
                if (_creatableItems[a].RequiredItems.Contains(_placedItems[b]))
                {
                    //_requiredItems.Remove(_placedItems[b]);
                    _requiredItemsTimer++;
                }
            }

            if (_requiredItemsTimer < _requiredItemsCount)
            {
                continue;
            }

            for (int b = 0; b < _placedItems.Count; b++)
            {
                if (_placedItems[b].UsingPlacessProcessTypes.Contains(selectedProcessType))
                {
                    _requiresProcessTypesTimer++;
                }
            }

            if (_requiresProcessTypesTimer >= _requiredItemsCount)
            {
                _creatableItem = _creatableItems[a];
                print($"{_creatableItem} Oluþturulabilir");
                return;
            }
        }
    }

    public bool ItemsIsPlaced => _placedItems.Count != 0;

    internal void ResetItems()
    {
        _requiredItems = null;
        _creatableItem = null;
        _requiredItemsCount = 0;
        _requiredItemsTimer = 0;
        _requiresProcessTypesTimer = 0;
        _placedObj.Clear();
        _placedItems.Clear();
    }

    internal void DestroyPlacedObjects()
    {
        if (_placedItems.Count > 0)
        {
            for (int i = 0; i < _placedObj.Count; i++)
            {
                Destroy(_placedObj[i]);
            }
        }
    }
}