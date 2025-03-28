using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCanvasController : MonoBehaviour
{

    public  GameObject[] childrensUI; 
    

    void Start()
    {
        StartCoroutine("StartDay",0.5f);
    }

    IEnumerator StartDay()
    {
        if (Manager.Game.TodayCustomerCount == 0)
        {
            int yesterDay = Manager.Game.CurrentDay;
            if (yesterDay == 1)
            {
                childrensUI[0].SetActive(true);
                childrensUI[2].SetActive(true);
            }
            else
            {
                childrensUI[0].SetActive(true);
                childrensUI[1].SetActive(true);
                childrensUI[1].GetComponent<TMP_Text>().text = "DAY" + (yesterDay - 1);
                childrensUI[2].SetActive(true);
                childrensUI[2].GetComponent<TMP_Text>().text = "DAY" + yesterDay;
                GetComponent<Animator>().Play("NextDay");
            }

            yield return new WaitForSeconds(2.0f);

            childrensUI[1].SetActive(false);
            childrensUI[2].SetActive(false);

            while (true)
            {
                Image img = childrensUI[0].GetComponent<Image>();
                Color c = img.color;
                c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * 2f);
                img.color = c;
                if (c.a < 0.05)
                {
                    break;
                }
                yield return null;
            }

            childrensUI[0].SetActive(false);
        }
    }
}
