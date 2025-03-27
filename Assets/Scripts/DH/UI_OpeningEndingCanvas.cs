using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OpeningEndingCanvas : MonoBehaviour
{
    public List<Transform> _buttons;

    private void Awake()
    {
        foreach(Transform button in _buttons)
        {
            button.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(button.GetComponent<TypingText>().SkipText()));
        }
    }
}
