using UnityEngine;

public class CutFood : MonoBehaviour
{
    private Bounds bounds;
    private Vector2 origin;
    private Color[] newColors;
    private Sprite sprite;
    public Transform broccoli;

    private void Awake()
    {
        bounds = broccoli.GetComponent<PolygonCollider2D>().bounds;
        origin = new Vector2(bounds.min.x, bounds.min.y);
        sprite = broccoli.GetComponent<SpriteRenderer>().sprite;
        newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
    }

    private void Start()
    {
        //Cut(null);
    }

    public void Cut(ref PolygonCollider2D polygonCollider2D)
    {
        Debug.Log(sprite.textureRect);
        Color[] cutColors = (Color[])newColors.Clone();

        for (int y = 0; y < 284; y++)
        {
            for (int x = 0; x < 376; x++)
            {
                Vector2 checkPoint = origin + new Vector2(bounds.size.x * x / 376, bounds.size.y * y / 284);

                if (polygonCollider2D.OverlapPoint(checkPoint))
                {
                    newColors[y * 376 + x] = new Color(1, 1, 1, 0);
                }
                else
                {
                    cutColors[y * 376 + x] = new Color(1, 1, 1, 0);
                }
            }
        }

        Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        Texture2D cutText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        newText.SetPixels(newColors);
        cutText.SetPixels(cutColors);
        newText.Apply();
        cutText.Apply();

        broccoli.GetComponent<SpriteRenderer>().sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        GameObject piece = new GameObject("piece");
        SpriteRenderer spriteRenderer = piece.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(cutText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));
    }
}
