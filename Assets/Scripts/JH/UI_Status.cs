using TMPro;
using UnityEngine;

public class UI_Status : MonoBehaviour
{
    public GameObject Money;
    public GameObject Popularity;
    private TextMeshProUGUI _moneyText;
    private TextMeshProUGUI _popularityText;


    private void Awake()
    {
        _moneyText = Money.GetComponentInChildren<TextMeshProUGUI>();
        _popularityText = Popularity.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        SetUI();
    }

    public void SetUI()
    {
        if (_moneyText == null)
        {
            _moneyText = Money.GetComponentInChildren<TextMeshProUGUI>();
        }


        if (_popularityText == null)
        {
            _popularityText = Popularity.GetComponentInChildren<TextMeshProUGUI>();
        }
        _popularityText.text = Manager.Game.Popularity.ToString();
    }
}
