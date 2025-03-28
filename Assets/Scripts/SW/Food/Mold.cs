using UnityEngine;

public class Mold : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("SlicedArea"))
        {
            Destroy(gameObject);
        }
    }
}
