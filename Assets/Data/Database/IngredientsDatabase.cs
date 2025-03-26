using System.Collections.Generic;
using UnityEngine;

public class IngredientsDatabase : MonoBehaviour
{
    public static Dictionary<int , IngredientsData> ObjectData => _ingredientsData;
    // int : key -> OrderData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , IngredientsData> _ingredientsData = new Dictionary<int, IngredientsData>();

    private void Awake()
    {
        AddCustomerData(1, "Ingredients/Broccoli"); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddCustomerData(2, "Ingredients/Cabbage"); 
        AddCustomerData(3, "Ingredients/Conor"); 
        AddCustomerData(4, "Ingredients/GreenOnion"); 
        AddCustomerData(5, "Ingredients/Pepper"); 
        AddCustomerData(6, "Ingredients/Pimento"); 
        AddCustomerData(7, "Ingredients/Radish"); 
    }

    public static void AddCustomerData(int id, string path)
    {
        if (!_ingredientsData.ContainsKey(id))
        {
            _ingredientsData.Add(id, (IngredientsData)Resources.Load(path));
        }
    }

}
