using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndingCanvas : MonoBehaviour
{
    public List<Transform> _displays;
    public Transform _mainButtons;

    private void Awake()
    {
        foreach (Transform display in _displays)
        {
            display.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(display.GetComponent<TypingText>().SkipText()));
        }
        _mainButtons.GetComponent<Button>().onClick.AddListener(() => Manager.Game.GoMain());
    }
}
