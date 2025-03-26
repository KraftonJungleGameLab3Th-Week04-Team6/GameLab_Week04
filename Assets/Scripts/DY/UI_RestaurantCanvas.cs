using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RestaurantCanvas : MonoBehaviour
{
    public GameObject _customerText;
    public List<UI_Button> _buttons;
    public Image _customerImage;
    
    private CustomerData _customerData;

    private void Start()
    {
        Manager.Restaurant.sendCustomer += OnCustomerOrder;
        transform.GetChild(3).GetComponent<TMP_Text>().text = Manager.Game.CurrentDay + " DAY";
    }

    private void OnCustomerOrder(int key)
    {
        _customerData = CustomerDatabase.ObjectData[key];
        _customerText.GetComponentInChildren<TextMeshProUGUI>().text = _customerData.customerName + '\n' +_customerData.customerOrder;
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = _customerData.customerAnswerList[i];
            int menuIndex = _customerData.customerAnswerMenuList[i];
            _buttons[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick(menuIndex));
        }
        _customerImage.sprite = _customerData.customerSprite;
        
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
