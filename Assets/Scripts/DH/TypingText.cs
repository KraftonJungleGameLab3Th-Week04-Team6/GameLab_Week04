using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    public bool IsDone { get { return _isDone; } }

    private IEnumerator _coroutine;
    private Image _textDisplay;
    private TextMeshProUGUI _textMesh;
    private string _originalText;
    private WaitForSeconds _delay;
    private bool _isDone = false;

    private void Awake()
    {
        _textDisplay = GetComponent<Image>();
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _originalText = _textMesh.text;
        _delay = new WaitForSeconds(2 * Time.deltaTime);
    }

    private void OnEnable()
    {
        _coroutine = StartText();
        StartCoroutine(_coroutine);
    }

    private IEnumerator StartText()
    {
        float a = 0;
        string text = null;

        char[] buffer = new char[_originalText.Length];
        System.Array.Fill(buffer, '\0');

        _textMesh.text = text;

        while (a < 1)
        {
            _textDisplay.color = new(1, 1, 1, a);
            a += 0.005f;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        buffer = new char[_originalText.Length];
        System.Array.Fill(buffer, '\0');

        for(int i = 0; i < _originalText.Length; i++)
        {
            buffer[i] = _originalText[i];
            _textMesh.SetCharArray(buffer);

            yield return _delay;
        }

        yield return new WaitForSeconds(1f);

        _isDone = true;

        yield break;
    }

    public IEnumerator SkipText()
    {
        StopCoroutine(_coroutine);

        _textMesh.text = _originalText;

        yield return new WaitForSeconds(1f);

        _isDone = true;

        yield break;
    }
}
