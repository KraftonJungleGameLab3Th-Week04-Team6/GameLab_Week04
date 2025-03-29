using UnityEngine;

public class PopularityManager
{
    UI_Status _ui_Status;
    

    private void Awake()
    {
        _ui_Status = GameObject.FindAnyObjectByType<UI_Status>();
    }
    public void Init()
    {
        Manager.Game.Popularity = 0;
    }

    public void PlusPopularity(int value)
    {
        Manager.Game.Popularity += value;
        Debug.Log($"인기도 {value} 증가 \n 총 인기도: {Manager.Game.Popularity}");
        if (_ui_Status == null)
        {
            _ui_Status = GameObject.FindAnyObjectByType<UI_Status>();
        }
        _ui_Status.SetUI();
    }




}
