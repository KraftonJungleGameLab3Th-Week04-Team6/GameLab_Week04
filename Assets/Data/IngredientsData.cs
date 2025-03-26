using UnityEngine;

[CreateAssetMenu(fileName = "IngredientsData", menuName = "Scriptable Objects/IngredientsData")]
public class IngredientsData : ScriptableObject // 재료 데이터
{
    [Tooltip("재료 ID")]
    public int IngredientsID;
    [Tooltip("재료 이름")]
    public string IngredientsName;
    [Tooltip("재료 프리펩")]
    public GameObject IngredientsPrefab;
}
