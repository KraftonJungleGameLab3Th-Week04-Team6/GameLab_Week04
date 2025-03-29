using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_RestaurantCanvas : MonoBehaviour
{
    public GameObject _customerText;
    public List<UI_Button> _buttons;
    public Image _customerImage;
    public GameObject PopularityFX;
    public TMP_Text totalMoney;
    
    //손님 데이터
    private CustomerData _customerData;
    //대사 데이터
    private CustomerOrderData[] _customerOrderData;
    private CustomerOrderData _currentOrderData;
    private GameObject _popularityFX;

    private void Awake()
    {
        Manager.Restaurant.sendCustomer += OnCustomerOrder;
        transform.GetChild(3).GetComponent<TMP_Text>().text = "DAY " + Manager.Game.CurrentDay;
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

    async Task ButtonClick(int menuKey)
    {
        Debug.Log("ButtonClick" + menuKey);
        Manager.Kitchen.MenuKey = menuKey;

        // 선택한 선택지 인덱스 찾기
        int index = -1;
        //_currentOrderData
        for (int i = 0; i < 3; i++)
        {
            if (_currentOrderData.customerAnswerMenuList[i] == menuKey)
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            Vector3 screenPoint = Input.mousePosition;


            // 좋아하는 선택지
            if (_currentOrderData.preferenceList[index] == 1)
            {
                _popularityFX = Instantiate(PopularityFX, screenPoint + Vector3.up * 50, Quaternion.identity);
                _popularityFX.transform.SetParent(transform);
                _popularityFX.GetComponent<TextEffect>().value = 2;
                _popularityFX.GetComponent<TextEffect>().Init();
                Manager.Popularity.PlusPopularity(2);
                await Task.Delay(1000);
            }

            // 싫어하는 선택지
            if (_currentOrderData.preferenceList[index] == -1)
            {
                _popularityFX = Instantiate(PopularityFX, screenPoint + Vector3.up * 50, Quaternion.identity);
                _popularityFX.transform.SetParent(transform);
                _popularityFX.GetComponent<TextEffect>().value = -1; 
                _popularityFX.GetComponent<TextEffect>().Init();
                Manager.Popularity.PlusPopularity(-1);
                await Task.Delay(1000);
            }
        }

        Manager.Game.GoKitchen();
    }

    private void OnDisable()
    {
        Manager.Restaurant.sendCustomer -= OnCustomerOrder;
    }
}
