using System.Collections.Generic;
using UnityEngine;

public class OrderDatabase : MonoBehaviour
{
    public static Dictionary<int , OrderData> ObjectData => _orderData;
    // int : key -> OrderData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , OrderData> _orderData = new Dictionary<int, OrderData>();

    private void Awake()
    {
        AddCustomerData(201, "Order/Order1"); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddCustomerData(202, "Order/Order2"); 
        AddCustomerData(203, "Order/Order3"); 
        AddCustomerData(204, "Order/Order4"); 
        AddCustomerData(205, "Order/Order5"); 
    }

    public static void AddCustomerData(int id, string path)
    {
        if (!_orderData.ContainsKey(id))
        {
            _orderData.Add(id, (OrderData)Resources.Load(path));
        }
    }

}
