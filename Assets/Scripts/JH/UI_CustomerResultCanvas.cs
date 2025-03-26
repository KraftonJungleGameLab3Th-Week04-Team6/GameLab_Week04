using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_CustomerResultCanvas : MonoBehaviour
{
    public Image _customerImage;
    public Image _iconImage;
    public Image _fadeOut;
    public TextMeshProUGUI _clickUIText;
    public Sprite[] _iconSprites;

    private bool _isDone = false;
    private bool _isGameOver = false;

    private void Start()
    {
        SetCustomerResult(Manager.Restaurant.CurrentCustomNum);
    }

    private void Update()
    {
        if (!_isDone) return;

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isGameOver)
            {
                _fadeOut.enabled = false;
                _iconImage.enabled = false;
                _clickUIText.enabled = false;
                Manager.Game.GameStart();
            }
            else
            {
                SceneManager.LoadScene("DYTestScene");
            }
        }
    }
    public void SetCustomerResult(int customerKey)
    {
        CustomerData _customerData = CustomerDatabase.ObjectData[customerKey];
        _customerImage.sprite = _customerData.customerSprite;

        //float score = Manager.Kitchen.ResultRemainingPercentage - Manager.Kitchen.MoldPercentage;
        float score = 70;
        if (Manager.Kitchen.MoldPercentage > 0.01 || score < 20)
        {
            StartCoroutine(CoShowCustomerResult(ECustomerIcon.Vomit));
            _isGameOver = true;
        }
        else if (score < 60)
        {
            StartCoroutine(CoShowCustomerResult(ECustomerIcon.Bad));
        }
        else if (score < 80)
        {
            StartCoroutine(CoShowCustomerResult(ECustomerIcon.Good));
        }
        else 
        {
            StartCoroutine(CoShowCustomerResult(ECustomerIcon.Heart));
        }
    }

    IEnumerator CoShowCustomerResult(ECustomerIcon icon)
    {
        yield return new WaitForSeconds(1f);
        _fadeOut.enabled = true;
        yield return new WaitForSeconds(1f);
        _iconImage.enabled = true;
        _iconImage.sprite = _iconSprites[(int)icon];
        yield return new WaitForSeconds(1.5f);
        _clickUIText.enabled = true;
        
        _isDone = true;

    }
}

