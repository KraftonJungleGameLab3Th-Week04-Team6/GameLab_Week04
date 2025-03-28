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
        // 손님 데이터 가져오기
        _customerData = CustomerDatabase.ObjectData[key];

        //손님 주문 대사 목록 받아오기
        _customerOrderData = _customerData.customerOrderDataList;

        int intOrder = Manager.Game.CurrentDay <= 3 ? 0 : 1;
        _currentOrderData = _customerOrderData[intOrder];
        //매니저에 동기화
        Manager.Restaurant.CurrentCustomerOrderData = _currentOrderData;
        
        _customerText.GetComponentInChildren<TextMeshProUGUI>().text = _customerData.customerName + '\n' +_currentOrderData.customerOrder;
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentOrderData.customerAnswerList[i];
            int menuIndex = _currentOrderData.customerAnswerMenuList[i];
            _buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick(menuIndex));
        }
        
        //손님 이미지 설정
        // 기분 좋을때
        if (_currentOrderData.mood > 0)
        {
            _customerImage.sprite = _customerData.customerGoodSprite;
        }
        // 기분 나쁠때
        if (_currentOrderData.mood < 0)
        {
            _customerImage.sprite = _customerData.customerBadSprite;
        }
        // null 방지
        if (_customerImage.sprite == null)
        {
            _customerImage.sprite = _customerData.customerGoodSprite;
        }

        
    }

    void ButtonClick(int menuKey)
    {
        Debug.Log("ButtonClick" + menuKey);
        Manager.Kitchen.MenuKey = menuKey;
        Manager.Game.GoKitchen();
    }

    private void OnDisable()
    {
        Manager.Restaurant.sendCustomer -= OnCustomerOrder;
    }
}
