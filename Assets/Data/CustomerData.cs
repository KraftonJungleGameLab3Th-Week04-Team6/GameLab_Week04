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
    [Tooltip("손님 이미지")]
    public Sprite customerSprite;
    [Tooltip("주문 대화")]
    public String customerOrder;
    [Tooltip("메뉴 ID 리스트")]
    public int[] customerAnswerMenuList;
    [Tooltip("대답 리스트")]
    public String[] customerAnswerList;
}
