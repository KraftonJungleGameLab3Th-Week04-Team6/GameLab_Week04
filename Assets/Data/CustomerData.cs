using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Scriptable Objects/CustomerData")]
public class CustomerData : ScriptableObject
{
    [Tooltip("손님 ID")]
    public int customerID;
    [Tooltip("손님 이름")]
    public string customerName;
    [Tooltip("손님 긍정 이미지")]
    public Sprite customerGoodSprite;
    [Tooltip("주문 부정 이미지")]
    public Sprite customerBadSprite;
    [Tooltip("좋아하는 음식 이미지")]
    public Sprite favoriteFood;
    [Tooltip("대답 리스트")]
    public CustomerOrderData[] customerOrderDataList;
}
