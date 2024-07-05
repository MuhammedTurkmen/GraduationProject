using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "", order = 1)]
public class BuildingTypeSO : ScriptableObject
{
    public Transform Prefab;
    public Transform Visual;
    public int Width;
    public int Height;
    public string BuildingName;
    public bool TurnAble;
}