using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestaurantManager
{
    public int CurrentCustomNum { get { return _currentCustomNum; } set { _currentCustomNum = value; } }

    public Action<int> sendCustomer;

    private int _currentCustomNum;

    public CustomerOrderData CurrentCustomerOrderData { get {return _currentCustomerOrderData; } set { _currentCustomerOrderData = value;}}

    private CustomerOrderData _currentCustomerOrderData;


    public int[] CustomerList { get { return _customerList; } }
    private int[] _customerList = new int[15];



    public void Init()
    {
        Debug.Log("초기 손님 순서 설정");
        // 1~3일차 손님 9명생성
        int[] randomNumbers = MakeRandomNumbers(1, 11);
        for (int i = 0; i < 9; i++)
        {
            CustomerList[i] = randomNumbers[i];
        }

        // 1~3일차 손님 중에서 4~5일차 손님 6명 생성
        randomNumbers = MakeRandomNumbers(0, 9, UnityEngine.Random.Range(0, 999999999));
        for (int i = 0; i < 6; i++)
        {
            CustomerList[9+i] = CustomerList[randomNumbers[i]] + 10;
        }

        // 손님 순서 출력
        //for (int i = 0; i < 15; i++)
        //{
        //    Debug.Log(CustomerList[i]);
        //}
    }

    public static int[] MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
    {
        if (randomSeed == 0)
            randomSeed = (int)DateTime.Now.Ticks;

        List<int> values = new List<int>();
        for (int v = minValue; v < maxValue; v++)
        {
            values.Add(v);
        }

        int[] result = new int[maxValue - minValue];
        System.Random random = new System.Random(Seed: randomSeed);
        int i = 0;
        while (values.Count > 0)
        {
            int randomValue = values[random.Next(0, values.Count)];
            result[i++] = randomValue;

            if (!values.Remove(randomValue))
            {
                // Exception
                break;
            }
        }

        return result;
    }

    public void SendCustomer(int customerID)
    {
        Debug.Log(customerID);
        sendCustomer?.Invoke(customerID);
    }
}
