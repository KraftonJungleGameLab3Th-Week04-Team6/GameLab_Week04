using System.Collections.Generic;
using UnityEngine;

public class CustomerDatabase : MonoBehaviour
{
    public static Dictionary<int , CustomerData> ObjectData => _customerData;
    // int : key -> OrderData의 ID 값으로 맞추는게 좋음 
    static Dictionary<int , CustomerData> _customerData = new Dictionary<int, CustomerData>();

    private void Awake()
    {
        AddCustomerData(1, ""); // 1번 -> 찾을때 해당 아이템의 번호 찾아야 됨
        AddCustomerData(2, ""); 
        AddCustomerData(3, ""); 
        AddCustomerData(4, ""); 
        AddCustomerData(5, ""); 
        AddCustomerData(6, ""); 
        AddCustomerData(7, ""); 
        AddCustomerData(8, ""); 
        AddCustomerData(9, "");
        AddCustomerData(10, "");  
    }

    public static void AddCustomerData(int id, string path)
    {
        if (!_customerData.ContainsKey(id))
        {
            _customerData.Add(id, (CustomerData)Resources.Load(path));
        }
    }

}
