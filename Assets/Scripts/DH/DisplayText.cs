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

    private IEnumerator StartText()
    {
        WaitForSecondsRealtime _delay;

        float a = 0;
        string text = null;

        _textMesh.text = text;

        _delay = new WaitForSecondsRealtime(0.001f);

        while (a < 1)
        {
            _textDisplay.color = new(1, 1, 1, a);
            a += 0.01f;
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

        _textDisplay.color = new(1, 1, 1, 1);
        _textMesh.text = _originalText;

        Invoke(nameof(EndText), 0.5f);
    }

    public void EndText()
    {
        _button.onClick.AddListener(() => { _isDone = true; });
    }
}
