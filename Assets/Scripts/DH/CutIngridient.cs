using UnityEngine;

public class CutIngridient : MonoBehaviour
{
    private Bounds _bounds;
    private Vector2 _origin;
    private Color[] _nowColors;
    private Sprite _sprite;

    public void Init()
    {
        _bounds = transform.GetComponent<PolygonCollider2D>().bounds;
        _origin = new Vector2(_bounds.min.x, _bounds.min.y);
        _sprite = transform.GetComponent<SpriteRenderer>().sprite;
        _nowColors = _sprite.texture.GetPixels((int)_sprite.textureRect.x, (int)_sprite.textureRect.y, (int)_sprite.textureRect.width, (int)_sprite.textureRect.height);
    }

    public void Cut(ref PolygonCollider2D polygonCollider2D)
    {
        Color[] cutColors = (Color[])_nowColors.Clone();

        for (int y = 0; y < (int)_sprite.textureRect.height; y++)
        {
            for (int x = 0; x < (int)_sprite.textureRect.width; x++)
            {
                Vector2 checkPixel = _origin + new Vector2(_bounds.size.x * x / (int)_sprite.textureRect.width, _bounds.size.y * y / (int)_sprite.textureRect.height);

                if (polygonCollider2D.OverlapPoint(checkPixel))
                {
                    _nowColors[y * (int)_sprite.textureRect.width + x] = new Color(1, 1, 1, 0);
                }
                else
                {
                    cutColors[y * (int)_sprite.textureRect.width + x] = new Color(1, 1, 1, 0);
                }
            }
        }

        Texture2D newText = new((int)_sprite.rect.width, (int)_sprite.rect.height);
        Texture2D cutText = new((int)_sprite.rect.width, (int)_sprite.rect.height);
        newText.SetPixels(_nowColors);
        cutText.SetPixels(cutColors);
        newText.Apply();
        cutText.Apply();

        transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        GameObject piece = new GameObject("piece");
        piece.transform.position = transform.position;
        SpriteRenderer spriteRenderer = piece.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(cutText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));
        piece.AddComponent<CuttedIngridient>();
    }
}
