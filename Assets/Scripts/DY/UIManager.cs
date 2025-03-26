using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    UI_Customer _customer;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _customer = FindAnyObjectByType<UI_Customer>();
    }


    public void SetCustomerUI(int customerId)
    {
        Debug.Log("SetCustomerUI");
        CustomerData customerData = CustomerDatabase.ObjectData[customerId];
        _customer.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = customerData.customerName + "/n" + customerData.customerOrder;
        for (int i = 0; i < 3; i++)
        {
            _customer.transform.GetChild(i+1).GetComponentInChildren<TextMeshProUGUI>().text = customerData.customerAnswerList[i];
            _customer.transform.GetChild(i+1).GetComponent<UI_Button>().OrderId = customerData.customerAnswerMenuList[i];
        }

        _customer.GetComponent<Canvas>().enabled = true;
    }
}