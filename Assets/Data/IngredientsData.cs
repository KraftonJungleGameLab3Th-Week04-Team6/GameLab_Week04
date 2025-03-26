using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Scriptable Objects/FoodData")]
public class IngredientsData : ScriptableObject // 재료 데이터
{
    [Tooltip("재료 ID")]
    public int IngredientsID;
    [Tooltip("재료 이름")]
    public string IngredientsName;
    [Tooltip("재료 프리펩")]
    public GameObject IngredientsPrefab;
}
