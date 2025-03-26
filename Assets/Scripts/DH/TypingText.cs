using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    private Image openingDisplay;
    private TextMeshProUGUI openingTextMesh;
    private string originalText;
    private WaitForSeconds delay = new WaitForSeconds(0.05f);

    private void Awake()
    {
        openingDisplay = GetComponent<Image>();
        openingTextMesh = GetComponentInChildren<TextMeshProUGUI>();
        originalText = openingTextMesh.text;
    }

    private void OnEnable()
    {
        StartCoroutine(StartOpening());
    }

    private IEnumerator StartOpening()
    {
        float a = 0;
        string text = null;

        char[] buffer = new char[originalText.Length];
        System.Array.Fill(buffer, '\0');

        openingTextMesh.text = text;

        while (a < 1)
        {
            openingDisplay.color = new(1, 1, 1, a);
            a += 0.005f;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        buffer = new char[originalText.Length];
        System.Array.Fill(buffer, '\0');

        for(int i = 0; i < originalText.Length; i++)
        {
            buffer[i] = originalText[i];
            openingTextMesh.SetCharArray(buffer);

            yield return delay;
        }

        yield break;
    }
}
