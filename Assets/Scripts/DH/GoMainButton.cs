using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoMainButton : MonoBehaviour
{
    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(() => Manager.Game.GoMain());
    }
}
