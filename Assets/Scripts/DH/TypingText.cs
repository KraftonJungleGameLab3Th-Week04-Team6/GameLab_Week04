using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    public bool IsDone { get { return _isDone; } }

    private Image _openingDisplay;
    private TextMeshProUGUI _openingTextMesh;
    private string _originalText;
    private WaitForSeconds _delay = new WaitForSeconds(0.05f);
    private bool _isDone = false;

    private void Awake()
    {
        _openingDisplay = GetComponent<Image>();
        _openingTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        _originalText = _openingTextMesh.text;
    }

    private void OnEnable()
    {
        StartCoroutine(StartOpening());
    }

    private IEnumerator StartOpening()
    {
        float a = 0;
        string text = null;

        char[] buffer = new char[_originalText.Length];
        System.Array.Fill(buffer, '\0');

        _openingTextMesh.text = text;

        while (a < 1)
        {
            _openingDisplay.color = new(1, 1, 1, a);
            a += 0.005f;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        buffer = new char[_originalText.Length];
        System.Array.Fill(buffer, '\0');

        for(int i = 0; i < _originalText.Length; i++)
        {
            buffer[i] = _originalText[i];
            _openingTextMesh.SetCharArray(buffer);

            yield return _delay;
        }

        _isDone = true;

        yield break;
    }
}
