using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerOrderData", menuName = "Scriptable Objects/CustomerOrderData")]
public class CustomerOrderData : ScriptableObject
{
    [Tooltip("대사 ID")]
    public int customerOrderID;
    [Tooltip("손님 기분")]
    public int Mood;
    [Tooltip("주문 대화")]
    public String customerOrder;
    [Tooltip("메뉴 ID 리스트")]
    public int[] customerAnswerMenuList;
    [Tooltip("대답 리스트")]
    public String[] customerAnswerList;
    [Tooltip("선호도")]
    public int[] preferenceList;
}
