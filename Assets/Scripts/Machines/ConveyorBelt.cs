using System.Collections;
using UnityEngine;

public class ConveyorBelt : PlacedObject
{
    GameObject obj, nextObj, centerObj;

    float _ratio;
    readonly int _tranportFrameCount = 60;

    public bool moveForward;

    Vector2 targetPos;

    /// <summary>
    /// 'IEnumarator' belli sürelerde beklemeyi saðlayan method tipidir.
    /// Eþyalarý taþýmak için kullanýlýr
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveItemOnNextPosition()
    {
        if (obj == null)
        {
            print("Boþ Item");
            yield break;
        }

        float _transportDeltaTime = .1f / _tranportFrameCount;
        Vector3 _interpoletedPos;

        for (int i = 0; i < _tranportFrameCount; i++)
        {
            _ratio = (float)i / _tranportFrameCount;

            _interpoletedPos =
                Vector3.Lerp(
                    obj.transform.position,
                    targetPos,
                    _ratio
                );

            obj.transform.position = _interpoletedPos;

            // verilen saniye kadar beklemeye yarayan yapý
            yield return new WaitForSeconds(_transportDeltaTime);
        }

        GridSystem.Instance.ChangeItemsPlace(checkPos, targetPos);
        obj = null;

        StopCoroutine(enumarator);
        enumarator = null;
    }


    private IEnumerator enumarator;

    private Vector2 checkPos;

    internal void TakeAction(bool originObj)
    {
        if (originObj)
        {
            nextObj = GridSystem.Instance.TakeItem(Origin + (Vector2)transform.right);

            if (nextObj != null)
            {
                return;
            }
       
            targetPos = GetNextPosition();
            checkPos = Origin;
        }
        else
        {
            centerObj = GridSystem.Instance.TakeItem(Origin);

            if (centerObj != null)
            {
                return;
            }

            checkPos = Origin - (Vector2)transform.right;
            Vector2 pos =
                    new(
                        Mathf.FloorToInt(checkPos.x) + 0.5f,
                        Mathf.FloorToInt(checkPos.y) + 0.5f
                    );
            checkPos = pos;

            pos =
                new Vector2(
                    Mathf.FloorToInt(Origin.x) + 0.5f,
                    Mathf.FloorToInt(Origin.y) + 0.5f
                );

            Origin = pos;

            targetPos = Origin;
        }

        obj = GridSystem.Instance.TakeItem(checkPos);

        if (obj == null)
        {
            return;
        }

        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
        }
       

        if (!gameObject.activeInHierarchy)
            return;

        if (enumarator == null)
        {
            enumarator = MoveItemOnNextPosition();
            StartCoroutine(enumarator);
        }
    }
}