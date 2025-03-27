using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OpeningCanvas : MonoBehaviour
{
    public List<Transform> _displays;

    private void Awake()
    {
        foreach(Transform display in _displays)
        {
            display.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(display.GetComponent<TypingText>().SkipText()));
        }
    }
}
