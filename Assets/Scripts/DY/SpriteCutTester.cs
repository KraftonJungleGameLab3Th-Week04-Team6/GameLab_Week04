using UnityEngine;

public class SpriteCutTester : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer foodSpriteRenderer;
    public PolygonCollider2D polygonCollider;

    private Texture2D maskTexture;

    [ContextMenu("Calculate Remaining Percentage")]
    private void CalculateRemainingPercentage()
    {
        // 원본 스프라이트 텍스처 가져오기
        Texture2D foodTexture = foodSpriteRenderer.sprite.texture;
        int texWidth = foodTexture.width;
        int texHeight = foodTexture.height;

        // RenderTexture 준비
        RenderTexture rt = new RenderTexture(texWidth, texHeight, 0, RenderTextureFormat.ARGB32);
        rt.Create();

        // 마스크 렌더링
        RenderMask(rt, texWidth, texHeight);

        // RenderTexture에서 Texture2D 읽기
        maskTexture = new Texture2D(texWidth, texHeight, TextureFormat.ARGB32, false);
        RenderTexture.active = rt;
        maskTexture.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        maskTexture.Apply();
        RenderTexture.active = null;
        rt.Release();

        // 픽셀 비교하여 남은 영역 계산
        float remainingPercent = GetRemainingPercentage(foodTexture, maskTexture);
        Debug.Log($"남은 음식 영역: {remainingPercent:F2}%");
    }

    private void RenderMask(RenderTexture rt, int texWidth, int texHeight)
    {
        // 임시 카메라를 만들어 마스크 렌더링
        GameObject camObj = new GameObject("TempCamera");
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = foodSpriteRenderer.bounds.size.y / 2f;
        cam.cullingMask = LayerMask.GetMask("MaskLayer");
        cam.backgroundColor = Color.black;
        cam.clearFlags = CameraClearFlags.Color;
        cam.targetTexture = rt;

        // 임시 GameObject로 폴리곤 메쉬 렌더링
        GameObject polyObj = new GameObject("TempPolygon");
        polyObj.layer = LayerMask.NameToLayer("MaskLayer");
        MeshFilter mf = polyObj.AddComponent<MeshFilter>();
        MeshRenderer mr = polyObj.AddComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Unlit/Color"));
        mr.material.color = Color.white;

        // PolygonCollider의 좌표로 메쉬 생성
        Mesh mesh = PolygonToMesh(polygonCollider);
        mf.mesh = mesh;

        polyObj.transform.position = foodSpriteRenderer.transform.position;

        cam.Render();
    }

    private Mesh PolygonToMesh(PolygonCollider2D collider)
    {
        Vector2[] points = collider.points;
        Triangulator triangulator = new Triangulator(points);
        int[] indices = triangulator.Triangulate();

        Vector3[] vertices = new Vector3[points.Length];
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = points[i];

        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = indices
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private float GetRemainingPercentage(Texture2D original, Texture2D mask)
    {
        Color32[] originalPixels = original.GetPixels32();
        Color32[] maskPixels = mask.GetPixels32();

        int totalFoodPixels = 0;
        int remainingPixels = 0;

        for (int i = 0; i < originalPixels.Length; i++)
        {
            if (originalPixels[i].a > 0)
            {
                totalFoodPixels++;
                if (maskPixels[i].r == 0)
                    remainingPixels++;
            }
        }

        if (totalFoodPixels == 0) return 0f;

        return (remainingPixels / (float)totalFoodPixels) * 100f;
    }
}
