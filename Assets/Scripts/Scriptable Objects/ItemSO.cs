using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Eþya bilgilerini tutmak için bilgi tutmaya yarayan 'Scriptable Object' oluþturmayý saðlar
/// </summary>
[CreateAssetMenu(menuName = "Objects/Item", fileName = "Item")]

public class ItemSO : ScriptableObject
{
    [SerializeField]
    private Sprite _itemSprite;

    [SerializeField]
    private ItemType _item;
    [SerializeField]
    List<ProcessType> _usingPlacessProcessTypes;

    [Header("Receipts")]

    [SerializeField]
    List<ProcessType> _creatingProcessTypes;

    public List<ItemSO> RequiredItems;

    [SerializeField]
    private int _createdItemCount = 1;

    public Sprite ItemSprite => _itemSprite;

    public ItemType Item => _item;

    public int CreatedItemCount => _createdItemCount;

    public List<ProcessType> CreatingProcessTypes => _creatingProcessTypes;
    public List<ProcessType> UsingPlacessProcessTypes => _usingPlacessProcessTypes;
}
