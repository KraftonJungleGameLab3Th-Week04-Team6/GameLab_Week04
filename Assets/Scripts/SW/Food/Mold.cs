using Unity.VisualScripting;
using UnityEngine;

public class Mold : MonoBehaviour
{
    public Sprite DefaultMold;
    public Sprite SafeModeMold;

    private void Start()
    {
       if (Manager.Game.SafeMoldMode)
       {
            GetComponent<SpriteRenderer>().sprite = SafeModeMold;
       }
       else
       {
            GetComponent<SpriteRenderer>().sprite = DefaultMold;
       }
    }
}
