using System;
using UnityEngine;

public class KitchenManager
{
    private int _orderKey;
    public int OrderKey { get{return _orderKey;} set{_orderKey = value;} }
    
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
