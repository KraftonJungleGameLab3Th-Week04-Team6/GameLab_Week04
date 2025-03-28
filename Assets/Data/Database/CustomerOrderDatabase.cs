using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderDatabase : MonoBehaviour
{
    public static Dictionary<int , CustomerData> ObjectData => _customerOrderData;
    // int : key -> OrderData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , CustomerData> _customerOrderData = new Dictionary<int, CustomerData>();

    private void Awake()
    {
        AddCustomerOrderData(1, "CustomerOrder/Customer1_Order1"); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddCustomerOrderData(2, "CustomerOrder/Customer2_Order1"); 
        AddCustomerOrderData(3, "CustomerOrder/Customer3_Order1"); 
        AddCustomerOrderData(4, "CustomerOrder/Customer4_Order1"); 
        AddCustomerOrderData(5, "CustomerOrder/Customer5_Order1"); 
        AddCustomerOrderData(6, "CustomerOrder/Customer6_Order1"); 
        AddCustomerOrderData(7, "CustomerOrder/Customer7_Order1"); 
        AddCustomerOrderData(8, "CustomerOrder/Customer8_Order1"); 
        AddCustomerOrderData(9, "CustomerOrder/Customer9_Order1");
        AddCustomerOrderData(10, "CustomerOrder/Customer10_Order1");
    }

    public static void AddCustomerOrderData(int id, string path)
    {
        if (!_customerOrderData.ContainsKey(id))
        {
            _customerOrderData.Add(id, (CustomerData)Resources.Load(path));
        }
    }

}
