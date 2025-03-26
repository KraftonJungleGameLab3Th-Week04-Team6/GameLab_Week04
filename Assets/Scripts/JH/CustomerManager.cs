using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerManager
{
    public int LeftCustomers { get { return _leftCustomers; } set { _leftCustomers = value; } }
    private int _leftCustomers;
    

    public void Init()
    {

    }

    private void StartDay()
    {
        LeftCustomers = 5;
    }

    private void EndDay()
    {
        // 손님 그만 받고 일일 정산.
    }

    private void CreateCustomer()
    {
        //if (LeftCustomers > 0) {

        //OrderDatabase.ObjectData.TryGetValue()
    }
    // 인풋: 첫 날인지 여부
    // 해야햐는 것: 손님의 수 만큼 랜덤한 손님 데이터 뽑아서 미니게임 시작

}
