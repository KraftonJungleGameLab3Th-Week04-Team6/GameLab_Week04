using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RestaurantCanvas : MonoBehaviour
{
    public GameObject _customerText;
    public List<UI_Button> _buttons;
    public Image _customerImage;
    
    //손님 데이터
    private CustomerData _customerData;
    //대사 데이터
    private CustomerOrderData[] _customerOrderData;
    private CustomerOrderData _currentOrderData;

    private void Awake()
    {
        Manager.Restaurant.sendCustomer += OnCustomerOrder;
        transform.GetChild(3).GetComponent<TMP_Text>().text = Manager.Game.CurrentDay + " DAY";
    }

    private void OnCustomerOrder(int key)
    {
        _customerData = CustomerDatabase.ObjectData[key];
        //손님 주문 대사 목록 받아오기
        _customerOrderData = _customerData.customerOrderDataList;
        
        //손님 주문 대사 랜덤으로 선택
        int randomIntOrder = Random.Range(0, _customerOrderData.Length);
        _currentOrderData = _customerOrderData[randomIntOrder];
        
        _customerText.GetComponentInChildren<TextMeshProUGUI>().text = _customerData.customerName + '\n' +_currentOrderData.customerOrder;
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentOrderData.customerAnswerList[i];
            int menuIndex = _currentOrderData.customerAnswerMenuList[i];
            _buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick(menuIndex));
        }
        _customerImage.sprite = _customerData.customerGoodSprite;
        
    }

    void ButtonClick(int orderKey)
    {
        Debug.Log("ButtonClick" + orderKey);
        Manager.Kitchen.OrderKey = orderKey;
        Manager.Game.GoKitchen();
    }

    private void OnDisable()
    {
        Manager.Restaurant.sendCustomer -= OnCustomerOrder;
    }
}
