using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayText : MonoBehaviour
{
    public bool IsDone { get { return _isDone; } }

    private IEnumerator _coroutine;
    private Button _button;
    private Image _textDisplay;
    private TextMeshProUGUI _textMesh;
    private string _originalText;
    private bool _isDone = false;
    private int _spacePressCount = 0;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _textDisplay = GetComponent<Image>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _originalText = _textMesh.text;

        _button.onClick.AddListener(SkipText);
    }

    private void OnEnable()
    {
        _coroutine = StartText();
        StartCoroutine(_coroutine);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spacePressCount++;

            if (_spacePressCount == 1 && !_isDone)
            {
                SkipText();
            }
            else if (_spacePressCount >= 2)
            {
                CompleteText();
            }
        }
    }

    private IEnumerator StartText()
    {
        WaitForSecondsRealtime _delay;

        float a = 0;
        string text = null;

        _textMesh.text = text;

        _delay = new WaitForSecondsRealtime(0.001f);

        _textDisplay.color = new(1, 1, 1, 0);
        while (true)
        {
            _textDisplay.color = new(1, 1, 1, a);

            if (a > 1f) break;
            else a = Mathf.Lerp(a, 1.8f, Time.deltaTime * 2f);

            yield return _delay;
        }
        _textDisplay.color = new(1, 1, 1, 1);

        char[] buffer = new char[_originalText.Length];
        System.Array.Fill(buffer, '\0');
        _delay = new WaitForSecondsRealtime(0.02f);

        for (int i = 0; i < _originalText.Length; i++)
        {
            buffer[i] = _originalText[i];
            _textMesh.SetCharArray(buffer);

            yield return _delay;
        }

        Invoke(nameof(EndText), 0.5f);

        yield break;
    }

    public void SkipText()
    {
        StopCoroutine(_coroutine);

        _textDisplay.color = new Color(1, 1, 1, 1);
        _textMesh.text = _originalText;

        Invoke(nameof(EndText), 0.5f);
    }

    public void EndText()
    {
        _button.onClick.AddListener(() => { _isDone = true; });
    }
    private void CompleteText()
    {
        StopCoroutine(_coroutine);
        CancelInvoke(nameof(EndText));

        _textDisplay.color = new Color(1, 1, 1, 1);
        _textMesh.text = _originalText;

        _isDone = true;  // 바로 완료 상태로 전환
    }
}
