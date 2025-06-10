using UnityEngine;

public enum ItemType
{
    metal,
    wood,
    plastic
}

[CreateAssetMenu(fileName = "New Item", menuName = "Items", order = 51)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;          // визуальный префаб
    public bool isResource;            // сырьё или готовый товар
    public Sprite icon;
    public ItemType resourseType;
}