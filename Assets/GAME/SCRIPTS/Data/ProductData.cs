using UnityEngine;

[CreateAssetMenu(menuName = "Game/Resource", fileName = "NewResourceData")]
public class ResourceData : ScriptableObject
{
    public string resourceName;    // Название ресурса
    public Sprite icon;            // Иконка для отображения в UI
    public int baseValue;          // Базовая ценность/цена ресурса
    // Можно добавить другие свойства, например тип, редкость и т.д.
}

[CreateAssetMenu(menuName = "Game/Product", fileName = "NewProductData")]
public class ProductData : ScriptableObject
{
    public string productName;    // Название готового изделия
    public Sprite icon;           // Иконка для UI
    public int sellPrice;         // Цена продажи изделия
    public ResourceData producedFrom; // Сырье, из которого производится (для справки)
    // Можно добавить свойства: время производства, качество, и т.п.
}
