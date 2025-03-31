using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_StatusResultCanvas : MonoBehaviour
{
    public Image _customerImage;
    public Image _iconImage;
    public Image _fadeOut;
    public TextMeshProUGUI _clickUIText;
    public Sprite[] _iconSprites;
    
    //결과창
    public TextMeshProUGUI _topText;
    public TextMeshProUGUI _menuPriceText;
    public TextMeshProUGUI _resultRemainingPercentageText;
    public Image _resultBar;
    public TextMeshProUGUI _finalResultText;
    public TextMeshProUGUI _finalResultPlusText;

    private bool _isDone = false;
    private bool _isGameOver = false;

    //현재 고객 선택지
    private CustomerOrderData _customerOrderData;
    private UI_StatusReactionCanvas _uICustomerReactionCanvas;
    private TextMeshProUGUI _customerReactionText; //손님 반응 대사
    private Canvas _uICustromerReactionCanvas;
    private GraphicRaycaster _uICustomerReactionCanvasGraphicRaycaster;
    private bool _skipAnimation; // 입력 시 즉시 완료
    private void Start()
    {
        _uICustomerReactionCanvas = FindAnyObjectByType<UI_StatusReactionCanvas>();
        _customerReactionText = _uICustomerReactionCanvas.GetComponentInChildren<TextMeshProUGUI>();
        _uICustromerReactionCanvas = _uICustomerReactionCanvas.GetComponent<Canvas>();
        _uICustomerReactionCanvasGraphicRaycaster = _uICustomerReactionCanvas.GetComponent<GraphicRaycaster>();
        _skipAnimation = false;
        
        SetCustomerResult(Manager.Restaurant.CurrentCustomNum);
    }

    private void Update()
    {
        if (!_isDone && (Input.GetMouseButtonUp(0) || Keyboard.current.spaceKey.wasPressedThisFrame))
        {
            _skipAnimation = true; // 애니메이션 스킵
        }
        else if (_isDone && (Input.GetMouseButtonUp(0) || Keyboard.current.spaceKey.wasPressedThisFrame))
        {
            if (!_isGameOver)
            {
                Manager.Game.GameStart();
                //_fadeOut.enabled = false;
                //_iconImage.enabled = false;
                
                ////결과창
                //_fadeOut.enabled = false;
                //_topText.enabled = false;
                //_menuPriceText.enabled = false;
                //_resultRemainingPercentageText.enabled = false;
                //_resultBar.enabled = false;
                //_finalResultPlusText.enabled = false;
                //_finalResultText.enabled = false;
                
                //_clickUIText.enabled = false;
            }
            else
            {
                // 곰팡이 엔딩
                Manager.Game.GoEnding(4);
            }
        }
        // if (!_isDone) return;
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (!_isGameOver)
        //     {
        //         Manager.Game.GameStart();
        //         //_fadeOut.enabled = false;
        //         //_iconImage.enabled = false;
        //         
        //         ////결과창
        //         //_fadeOut.enabled = false;
        //         //_topText.enabled = false;
        //         //_menuPriceText.enabled = false;
        //         //_resultRemainingPercentageText.enabled = false;
        //         //_resultBar.enabled = false;
        //         //_finalResultPlusText.enabled = false;
        //         //_finalResultText.enabled = false;
        //         
        //         //_clickUIText.enabled = false;
        //     }
        //     else
        //     {
        //         Manager.Game.GoEnding(3);
        //     }
        // }
    }
    public void SetCustomerResult(int customerKey)
    {
        CustomerData _customerData = CustomerDatabase.ObjectData[customerKey];
        _customerImage.sprite = _customerData.customerGoodSprite;

        
        // 지금 선택한 선택지 인덱스 찾기
        int key = Manager.Kitchen.MenuKey;
        int index = -1;
        _customerOrderData = Manager.Restaurant.CurrentCustomerOrderData;
        Debug.Log(_customerOrderData);
        Debug.Log(key);
        for (int i = 0; i < 3; i++)
        {
            if (_customerOrderData.customerAnswerMenuList[i] == key)
            {
                index = i;
                break;
            }
        }
        
        // 싫어하는 선택지
        if (index != -1)
        {
            if (_customerOrderData.preferenceList[index] == -1)
            {
                _customerImage.sprite = _customerData.customerBadSprite;
                _customerReactionText.text = _customerOrderData.answerReaction[index];
            }
        }
        // soso 선택지
        if (index != -1)
        {
            if (_customerOrderData.preferenceList[index] == 0)
            {
                _customerImage.sprite = _customerData.customerGoodSprite;
                _customerReactionText.text = _customerOrderData.answerReaction[index];
            }
        }
        
        // 좋아하는 음식 선택지
        if (index != -1)
        {
            if (_customerOrderData.preferenceList[index] == 1)
            {
                _customerImage.sprite = _customerData.customerGoodSprite;
                _customerReactionText.text = _customerOrderData.answerReaction[index];
            }
        }
        
        // 남은 재료 양에 따라 평가
        float score = Manager.Kitchen.ResultRemainingPercentage;
        print("score" + score);
        ECustomerIcon scoreIcon;
        if (Manager.Kitchen.MoldPercentage > 0.01)
        {
            scoreIcon = ECustomerIcon.Vomit;
            _isGameOver = true;
        }
        else if (score < 60)
        {
            scoreIcon = ECustomerIcon.Bad;
        }
        else if (score < 80)
        {
            scoreIcon = ECustomerIcon.Good;
        }
        else 
        {
            scoreIcon = ECustomerIcon.Heart;
        }
        // 결과창 현재 메뉴 가격
        _menuPriceText.text = MenuDatabase.ObjectData[key].menuPrice + "원";
        // 결과창 남은 재료 양
        _resultRemainingPercentageText.text = Manager.Kitchen.ResultRemainingPercentage.ToString("F1") + "%";
        // 결과창 최종 가격
        _finalResultText.text = ((Manager.Kitchen.ResultRemainingPercentage * MenuDatabase.ObjectData[key].menuPrice) / 100).ToString("F0") + "원";

        StartCoroutine(CoShowCustomerResult(scoreIcon));
    }

    IEnumerator CoShowCustomerResult(ECustomerIcon icon)
    {
        // 손님 반응 대사 활성화
        yield return WaitOrSkip(0.5f);
        _uICustromerReactionCanvas.enabled = true;
        _uICustomerReactionCanvasGraphicRaycaster.enabled = true;

        _skipAnimation = false; // 스킵 초기화
        
        yield return WaitOrSkip(1.8f);
        
        _skipAnimation = false; // 스킵 초기화
        // 손님 반응 대사 비활성화
        _uICustromerReactionCanvas.enabled = false;
        _uICustomerReactionCanvasGraphicRaycaster.enabled = false;

        // 결과창 시작
        _fadeOut.enabled = true;
        yield return WaitOrSkip(0.2f);
        _topText.enabled = true;
        yield return WaitOrSkip(0.4f);
        _menuPriceText.enabled = true;
        yield return WaitOrSkip(0.2f);
        _resultRemainingPercentageText.enabled = true;
        yield return WaitOrSkip(0.2f);
        _resultBar.enabled = true;
        yield return WaitOrSkip(0.2f);
        _finalResultPlusText.enabled = true;
        _finalResultText.enabled = true;
        yield return WaitOrSkip(1.5f);
        _clickUIText.enabled = true;

        _isDone = true;
        _skipAnimation = false; // 초기화
    }

// 기다리거나 스킵하는 공통 메서드
    private IEnumerator WaitOrSkip(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (_skipAnimation)
            {
                yield break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

