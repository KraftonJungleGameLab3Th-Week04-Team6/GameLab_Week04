using System;
using UnityEngine;

public class RestaurantManager
{
    public Action<int> sendCustomer;

    public void SendCustomer(int customerID)
    {
        Debug.Log(customerID);
        sendCustomer?.Invoke(customerID);
    }
}
