using UnityEngine;

public class KitchenOrder : MonoBehaviour
{
    private OrderData _orderData;
    private IngredientsData _ingredientsData;
    [SerializeField] private GameObject _ingredientsSpawnPoint;
    private void Start()
    {
//        Manager.Kitchen.order += Order;
        Order(201);
    }

    // 주문 받기
    private void Order(int key)
    {
        // 해당 주문에 대한 재료 목록 가져오기
        _orderData = OrderDatabase.ObjectData[key];
        // 해당 주문에 대한 재료 데이터 가져오기
        for (int i = 0; i < _orderData.orderIngredients.Count; i++)
        {
            _ingredientsData = IngredientsDatabase.ObjectData[_orderData.orderIngredients[i]];
            IngredientsSetting();
        }
    }
    
    // 주문에 대한 재료 생성
    private void IngredientsSetting()
    {
        GameObject ingredients = _ingredientsData.IngredientsPrefab;
        GameObject ingredientsObj = Instantiate(ingredients, _ingredientsSpawnPoint.transform.position, Quaternion.identity);
    }
}
