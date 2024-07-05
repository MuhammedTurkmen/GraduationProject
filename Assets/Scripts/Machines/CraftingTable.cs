using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : Crafter
{
    [SerializeField]
    private ProcessType _machineProcessType;

    private Image _itemImage;

    private void Awake()
    {
        _itemImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Tarif aramak i�in masa �zerine yeni e�ya ekler
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="item"></param>
    public void AddItem(GameObject obj, ItemSO item)
    {
        if (_creatableItem != null)
            return;

        Rigidbody2D rigi = obj.GetComponent<Rigidbody2D>();
        rigi.simulated = false;

        obj.transform.position = transform.position + new Vector3(0.5f, 0.5f, 0);
        _placedObj.Add(obj);
        _placedItems.Add(item);


        ProcessRevealing(_machineProcessType);
        SetItemImage();
    }

    /// <summary>
    /// Olu�turulacak nesneye �zelliklerini verir
    /// </summary>
    public void SetItemImage()
    {
        if (_creatableItem != null)
        {
            _itemImage.enabled = true;
            _itemImage.sprite = _creatableItem.ItemSprite;
        }
        else
        {
            _itemImage.enabled = false;
            _itemImage.sprite = null;
        }
    }

    /// <summary>
    /// Bu fonksiyon yap�labilek olan e�yay� oyuncuya verir
    /// </summary>
    public void TakeItem()
    {
        if (_creatableItem == null)
            return;

        DestroyPlacedObjects();

        int pos = 1;

        for (int i = 0; i < _creatableItem.CreatedItemCount; i++)
        {
            GameObject itemUser = Instantiate(ItemUserPrefab, ItemsParent);
            itemUser.transform.position = transform.position + 2 * pos * Vector3.right;
            itemUser.GetComponent<ItemUser>()._scriptableItem = _creatableItem;
            itemUser.SetActive(true);
            pos++;
        }

        ThrowUnnecessaryItems();

        ResetItems();
        SetItemImage();
    }

    /// <summary>
    /// Masadaki e�yalar� ��karmya yarar
    /// </summary>
    public void ThrowItems()
    {
        if (_placedItems.Count == 0)
            return;

        for (int i = 0; i < _placedObj.Count; i++)
        {
            Rigidbody2D rigi = _placedObj[i].GetComponent<Rigidbody2D>();

            _placedObj[i].transform.position = transform.position - Vector3.up * 2 + i * 2 * Vector3.right;
            rigi.velocity = Vector2.zero;
            rigi.simulated = true;
        }

        ResetItems();
        SetItemImage();
    }

    public void ThrowUnnecessaryItems()
    {
        if (_placedItems.Count == 0)
            return;

        print($"{_placedItems.Count}");

        for (int i = 0; i < _placedObj.Count; i++)
        {
            print($"toplam e�ya say�s�: {_requiredItems.Count}");

            print($"i before : {i}");

            if (!_requiredItems.Contains(_placedItems[i]))
            {
                Rigidbody2D rigi = _placedObj[i].GetComponent<Rigidbody2D>();

                _placedObj[i].transform.position = transform.position - Vector3.up * 2 + i * 2 * Vector3.right;
                rigi.velocity = Vector2.zero;
                rigi.simulated = true;

                _placedItems.RemoveAt(i);
                i -= 1;
                print($"i after : {i}");
            }
        }

        //ResetItems();
        SetItemImage();
    }
}