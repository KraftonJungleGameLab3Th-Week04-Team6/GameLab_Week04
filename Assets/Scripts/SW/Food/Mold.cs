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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("SlicedArea"))
        {
            Destroy(gameObject);
        }
    }
}
