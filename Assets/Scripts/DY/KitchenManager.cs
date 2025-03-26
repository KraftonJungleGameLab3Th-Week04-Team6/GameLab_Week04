using System;
using UnityEngine;

public class KitchenManager
{
    private int _orderKey;
    public int OrderKey { get{return _orderKey;} set{_orderKey = value;} }

    // 게임 결과
    //남은 음식 영역
    private float _resultRemainingPercentage;
    //곰팡이 덮인 비율
    private float moldPercentage;
    
    public float ResultRemainingPercentage { get{return _resultRemainingPercentage;} set{_resultRemainingPercentage = value; }}
    public float MoldPercentage { get{return moldPercentage;} set{moldPercentage = value; }}
    
    public void Init()
    {
        
    }
     
}
