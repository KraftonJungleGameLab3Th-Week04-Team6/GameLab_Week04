using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TodayEndController : MonoBehaviour
{
    public GameObject TodayEndCanvas;
    private TMP_Text _todayGetMoney;
    private TMP_Text _todayPayMoney;
    private TMP_Text _totalMoney;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Manager.Game.TodayCustomerCount == Manager.Game.TodayCustomerMaxCount)
        {
            //TodayEndCanvas;
            //TodayEndCanvas = FindAnyObjectByType<TodayEndCanvas>().gameObject;
             _todayGetMoney = TodayEndCanvas.transform.GetChild(1).GetComponent<TMP_Text>();
             _todayPayMoney = TodayEndCanvas.transform.GetChild(2).GetComponent<TMP_Text>();
             _totalMoney = TodayEndCanvas.transform.GetChild(3).GetComponent<TMP_Text>();

            TodayEndCanvas.SetActive(true);
            _todayGetMoney.text = "하루 수익 : " + Manager.Game.TodayGetMoney.ToString();
            _todayPayMoney.text = "하루 지출 : " + "-5000";
            Manager.Game.TotalMoney -= 5000;
            Manager.Game.TotalMoney += Manager.Game.TodayGetMoney;
            Manager.Game.TodayGetMoney = 0;
            _totalMoney.text = "총 금액 : " + Manager.Game.TotalMoney.ToString();

           
        }
        print("하루 방문자" + Manager.Game.TodayCustomerCount);
        print("가격" + Manager.Game.TodayGetMoney);
        print("하루날짜" + Manager.Game.CurrentDay);
    }

    public void NextDay()
    {
        print("NextDay");
        if (Manager.Game.TodayCustomerCount >= 5 && Manager.Game.TotalMoney >= 0)
        {
            SceneManager.LoadScene("SW_TestScene");
        }
        else if (Manager.Game.TotalMoney <= 0)
        {
            print("GameOver");
            SceneManager.LoadScene("DYTestScene");
        }
        Manager.Game.CurrentDay += 1;
        Manager.Game.TodayCustomerCount = 0;
        SceneManager.LoadScene("DY_RestaurantScene");
    }

}
