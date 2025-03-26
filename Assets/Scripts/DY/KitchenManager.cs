using System;
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public Action<int> order;
    
    public void Init()
    {
        
    }
    
    // 주문 받기
    public void Order(int orderNum)
    {
        order?.Invoke(orderNum);
    }
    
     
}
