using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuData", menuName = "Scriptable Objects/MenuData")]

public class MenuData : ScriptableObject // 주문 정보 데이터
{
    [Tooltip("주문 ID")]
    public int menuID;
    [Tooltip("음식 이름")]
    public string menuName;
    [Tooltip("필요한 재료 목록")]
    public List<int> menuIngredients;
    [Tooltip("가격")]
    public int menuPrice;
}
