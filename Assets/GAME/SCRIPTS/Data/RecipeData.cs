using UnityEngine;

[System.Serializable]
public class ResourceRequirement
{
    public ResourceData resource;
    public int amount = 1;
}

[CreateAssetMenu(menuName = "Game/Recipe", fileName = "NewRecipeData")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public ResourceRequirement[] requirements;
    public ProductData outputProduct;
}
