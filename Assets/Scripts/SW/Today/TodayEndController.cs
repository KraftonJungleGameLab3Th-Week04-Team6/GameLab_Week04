using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TodayEndController : MonoBehaviour
{
    // 여기 넣으면 안되는데... 종헌님이 작업하고 계시니까 옮겨야할 듯
    public TMP_Text _todayMoney;
    //
    public GameObject TodayEndCanvas;
    private TMP_Text _todayGetMoney;
    private TMP_Text _todayPayMoney;
    private TMP_Text _totalMoney;
    private TMP_Text _yesterdayTotalMoney;

    private int _todayGetMoneyNum;
    private int _totalGetMoneyNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _todayGetMoneyNum = Manager.Game.TodayGetMoney;
        _totalGetMoneyNum = Manager.Game.TotalMoney;
        _todayMoney.text = (_todayGetMoneyNum + _totalGetMoneyNum).ToString(); 

        if (Manager.Game.TodayCustomerCount == Manager.Game.TodayCustomerMaxCount)
        {
            //TodayEndCanvas;
            //TodayEndCanvas = FindAnyObjectByType<TodayEndCanvas>().gameObject;
             _todayGetMoney = TodayEndCanvas.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
             _todayPayMoney = TodayEndCanvas.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
             _totalMoney = TodayEndCanvas.transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>();
            _yesterdayTotalMoney = TodayEndCanvas.transform.GetChild(1).GetChild(4).GetComponent<TMP_Text>();

            TodayEndCanvas.SetActive(true);

            _yesterdayTotalMoney.text = "어제 합산 : " + (_totalGetMoneyNum).ToString();
            _todayGetMoney.text = "일 수입 : +" + _todayGetMoneyNum.ToString();
            _todayPayMoney.text = "일 고정비 : " + "-8000";
            Manager.Game.TotalMoney -= 8000;
            Manager.Game.TotalMoney += _todayGetMoneyNum;
            Manager.Game.TodayGetMoney = 0;
            _totalMoney.text = "합산 : " + Manager.Game.TotalMoney.ToString();
        }
        print("하루 방문자" + Manager.Game.TodayCustomerCount);
        print("가격" + Manager.Game.TodayGetMoney);
        print("하루날짜" + Manager.Game.CurrentDay);
    }

    public void NextDay()
    {
        print("NextDay");
        if (Manager.Game.CurrentDay >= 5 && Manager.Game.TotalMoney >= 0)
        {
            // 최고 엔딩
            if (Manager.Game.TotalMoney >= 15000 && Manager.Game.Popularity >= 20)
            {
                Manager.Game.GoEnding(0);
            }
            // 최악 엔딩
            else if (Manager.Game.Popularity < 0)
            {
                Manager.Game.GoEnding(2);
            }
            else 
            {
                Manager.Game.GoEnding(1);
            }
        }
        else if (Manager.Game.TotalMoney <= 0)
        {
            print("GameOver");
            Manager.Game.GoEnding(4);
        }
        else
        {
            Manager.Game.CurrentDay += 1;
            Manager.Game.TodayCustomerCount = 0;
            SceneManager.LoadScene("DY_RestaurantScene");
        }
    }

}
