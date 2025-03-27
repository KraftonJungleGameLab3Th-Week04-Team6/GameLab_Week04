using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager
{
    public int LeftCustomers { get { return _leftCustomers; } set { _leftCustomers = value; } }
    private int _leftCustomers;
    public int[] IndexToOrderId { get {return _indexToOrderId;} set { _indexToOrderId = value; } }
    private int[] _indexToOrderId = new int[3];

    private void StartDay()
    {
        LeftCustomers = 5;
        CreateCustomer();
    }

    private void EndDay()
    {
        Debug.Log("손님 그만 받고 일일 정산.");
        // 손님 그만 받고 일일 정산.
    }

    // 인풋: 첫 날인지 여부
    // 해야햐는 것: 손님의 수 만큼 랜덤한 손님 데이터 뽑아서 미니게임 시작
    private void CreateCustomer()
    {
        if (LeftCustomers == 0)
        {
            EndDay();
            return;
        }
        LeftCustomers--;
        int randInt = Random.Range(0, CustomerDatabase.ObjectData.Count);
        CustomerData customerData;
        CustomerDatabase.ObjectData.TryGetValue(randInt, out customerData);

        for (int i = 0; i < 3; i++)
        {
            // _indexToOrderId[i] = customerData.customerAnswerList[i].Item1;
        }

        // Manager.UI.SetCustomerUI(customerData.customerID);
    }

}
