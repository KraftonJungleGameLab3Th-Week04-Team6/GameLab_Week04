using System.Collections.Generic;
using UnityEngine;

public class CustomerDatabase : MonoBehaviour
{
    public static Dictionary<int , CustomerData> ObjectData => _customerData;
    // int : key -> OrderData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , CustomerData> _customerData = new Dictionary<int, CustomerData>();

    private void Awake()
    {
        AddCustomerData(1, "Customer/Customer1"); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddCustomerData(2, "Customer/Customer2"); 
        AddCustomerData(3, "Customer/Customer3"); 
        AddCustomerData(4, "Customer/Customer4"); 
        AddCustomerData(5, "Customer/Customer5"); 
        AddCustomerData(6, "Customer/Customer6"); 
        AddCustomerData(7, "Customer/Customer7"); 
        AddCustomerData(8, "Customer/Customer8"); 
        AddCustomerData(9, "Customer/Customer9");
        AddCustomerData(10, "Customer/Customer10");  
    }

    public static void AddCustomerData(int id, string path)
    {
        if (!_customerData.ContainsKey(id))
        {
            _customerData.Add(id, (CustomerData)Resources.Load(path));
        }
    }

}
