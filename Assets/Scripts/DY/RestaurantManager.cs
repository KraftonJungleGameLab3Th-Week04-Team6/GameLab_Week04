using System;
using UnityEngine;

public class RestaurantManager
{
    public int CurrentCustomNum { get { return _currentCustomNum; } set { _currentCustomNum = value; } }

    public Action<int> sendCustomer;

    private int _currentCustomNum;

    public CustomerOrderData CurrentCustomerOrderData { get {return _currentCustomerOrderData; } set { _currentCustomerOrderData = value;}}

    private CustomerOrderData _currentCustomerOrderData;
    public void SendCustomer(int customerID)
    {
        Debug.Log(customerID);
        sendCustomer?.Invoke(customerID);
    }
}
