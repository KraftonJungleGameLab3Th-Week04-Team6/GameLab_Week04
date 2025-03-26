using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderData", menuName = "Scriptable Objects/OrderData")]

public class OrderData : ScriptableObject // 주문 정보 데이터
{
    [Tooltip("주문 ID")]
    public int orderID;
    [Tooltip("음식 이름")]
    public string orderName;
    [Tooltip("필요한 재료 목록")]
    public List<int> orderIngredients;
    [Tooltip("가격")]
    public int orderPrice;
}
