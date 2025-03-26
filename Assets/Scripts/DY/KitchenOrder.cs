using UnityEngine;

public class KitchenOrder : MonoBehaviour
{
    
    private void Start()
    {
        Manager.Kitchen.order += Order;
    }

    // 주문 받기
    private void Order(int key)
    {
        // 해당 주문에 대한 재료 목록 가져오기
    }

    private void FoodSetting()
    {
        
    }
}
