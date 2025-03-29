using NUnit.Framework.Internal.Filters;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutIngridient : MonoBehaviour
{
    private Bounds _bounds;
    private Vector2 _origin;
    private Color[] _nowColors;
    private Sprite _sprite;
    private MoldSpawner _moldSpawner;
    private ContactFilter2D _filter2D = new();
    List<Collider2D> _results = new();

    private void Awake()
    {
        _bounds = transform.GetComponent<PolygonCollider2D>().bounds;
        _origin = new Vector2(_bounds.min.x, _bounds.min.y);
        _sprite = transform.GetComponent<SpriteRenderer>().sprite;
        _nowColors = _sprite.texture.GetPixels((int)_sprite.textureRect.x, (int)_sprite.textureRect.y, (int)_sprite.textureRect.width, (int)_sprite.textureRect.height);

        _moldSpawner = FindAnyObjectByType<MoldSpawner>();
        _filter2D.SetLayerMask(1 << LayerMask.NameToLayer("Mold"));
    }

    public void Cut(ref PolygonCollider2D polygonCollider2D)
    {
        Color[] cutColors = (Color[])_nowColors.Clone(); // 현재 영역 스프라이트를 깊은 복사 한 잘릴 영역 스프라이트

        for (int y = 0; y < (int)_sprite.textureRect.height; y++)
        {
            for (int x = 0; x < (int)_sprite.textureRect.width; x++)
            {
                Vector2 checkPixel = _origin + new Vector2(_bounds.size.x * x / (int)_sprite.textureRect.width, _bounds.size.y * y / (int)_sprite.textureRect.height);

                if (polygonCollider2D.OverlapPoint(checkPixel))
                {
                    _nowColors[y * (int)_sprite.textureRect.width + x] = new Color(1, 1, 1, 0); // 잘릴 영역 내라면 현재 스프라이트 픽셀 제거
                }
                else
                {
                    cutColors[y * (int)_sprite.textureRect.width + x] = new Color(1, 1, 1, 0); // 잘릴 영역 외라면 잘린 스프라이트 픽셀 제거c
                }
            }
        }

        Texture2D newText = new((int)_sprite.rect.width, (int)_sprite.rect.height);
        Texture2D cutText = new((int)_sprite.rect.width, (int)_sprite.rect.height);
        newText.SetPixels(_nowColors);
        cutText.SetPixels(cutColors);
        newText.Apply();
        cutText.Apply();

        transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(newText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f)); // 잘린 영역을 반영한 스프라이트 재설정

        GameObject piece = new GameObject("piece"); // 잘려진 영역 게임오브젝트 생성
        piece.transform.position = transform.position;
        SpriteRenderer spriteRenderer = piece.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -2;
        spriteRenderer.sprite = Sprite.Create(cutText, new Rect(0, 0, newText.width, newText.height), new Vector2(0.5f, 0.5f));

        polygonCollider2D.Overlap(_filter2D, _results);

        foreach(Collider2D collider2D in _results)
        {
            Transform mold = collider2D.transform;

            mold.SetParent(piece.transform);
            Destroy(mold.GetComponent<CircleCollider2D>());
            _moldSpawner.MoldSet.Remove(mold);
        }

        /*foreach (Transform mold in _moldSpawner.MoldSet.ToList()) // 영역 내 곰팡이는 같이 제거하고 이동
        {
            if(Physics2D.OverlapCollider(mold.GetComponent<CircleCollider2D>(), _filter2D, _results) > 0)
            {
                mold.SetParent(piece.transform);
                Destroy(mold.GetComponent<CircleCollider2D>());
                _moldSpawner.MoldSet.Remove(mold);
            }
        }*/

        piece.AddComponent<CuttedIngridient>();

        _moldSpawner.CheckMoldSetCount();
    }
}
