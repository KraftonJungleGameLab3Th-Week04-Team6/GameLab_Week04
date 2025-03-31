using UnityEngine;

public class FoodAreaRaycaster : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Vector2 areaSize = new Vector2(5f, 5f); // 5 유닛 x 5 유닛 (500px)
    public int resolution = 100; // 한 축당 레이캐스트 개수 (높을수록 정밀도 증가)

    [Header("Colliders")]
    public Collider2D foodCollider; // 음식 스프라이트 콜라이더
    public PolygonCollider2D cutCollider; // 드로우로 만든 잘라내는 콜라이더

    private float stepSizeX;
    private float stepSizeY;

    [ContextMenu("Calculate Area Percentage")]
    public void CalculateAreaPercentage()
    {
        if (foodCollider == null || cutCollider == null)
        {
            Debug.LogError("콜라이더를 반드시 할당해주세요!");
            return;
        }

        stepSizeX = areaSize.x / resolution;
        stepSizeY = areaSize.y / resolution;

        int totalFoodHits = 0;
        int cutHits = 0;

        Vector2 origin = (Vector2)transform.position - areaSize * 0.5f;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                Vector2 checkPoint = origin + new Vector2(x * stepSizeX, y * stepSizeY);

                // 음식 이미지 콜라이더에 닿는지 체크
                bool hitFood = foodCollider.OverlapPoint(checkPoint);
                if (hitFood) totalFoodHits++;

                // 잘라낸 콜라이더에 닿는지 체크
                if (hitFood && cutCollider.OverlapPoint(checkPoint))
                    cutHits++;
            }
        }

        int remainingHits = totalFoodHits - cutHits;

        float remainingPercentage = totalFoodHits == 0 ? 0f : ((float)remainingHits / totalFoodHits) * 100f;

        Debug.Log($"음식 전체 픽셀 수: {totalFoodHits}");
        Debug.Log($"잘라낸 픽셀 수: {cutHits}");
        Debug.Log($"남은 음식 영역: {remainingPercentage:F2}%");
    }

    // 영역 가시화를 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }
}