using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public BuildingTypeSO BuildingType;

    internal Quaternion Rotation { get; private set; }
    internal Directions Direction;
    internal Vector2 Origin;

    public Vector2 GetGridPosition()
    {
        return Origin;
    }


    public void SetOrigin()
    {
        Vector2 pos = new(Mathf.FloorToInt(transform.position.x) + 0.5f, Mathf.FloorToInt(transform.position.y) + 0.5f);
        Origin = pos;
    }

    public void SetGridPosition(Vector2 pos)
    {
        Origin = pos;
    }

    public void SetRotation(Quaternion quaternion)
    {
        Rotation = quaternion;
        Direction = quaternion.Quaternion2Direction();
    }
    public void SetRotation(Directions dir)
    {
        Direction = dir;
        Rotation = dir.Direction2Quaternion();
    }

    public Vector2 GetNextPosition()
    {
        return transform.position + transform.right;
    }

    public Vector2 GetPreviousPosition()
    {
        return transform.position - transform.right;
    }


    [Header("BUILDING")]
    public bool LeftEditable;
    public bool RightEditable, TopEditable, BottomEditable;

    public void Edit()
    {

    }
}