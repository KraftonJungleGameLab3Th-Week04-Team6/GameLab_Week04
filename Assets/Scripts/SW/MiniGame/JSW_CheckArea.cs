using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class JSW_CheckArea : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Vector2 areaSize = new Vector2(5f, 5f); // 5 유닛 x 5 유닛 (500px)
    public int resolution = 100; // 한 축당 레이캐스트 개수 (높을수록 정밀도 증가)

    [Header("Colliders")]
    public Collider2D foodCollider; // 음식 스프라이트 콜라이더
    public List<PolygonCollider2D> cutColliders; // 여러 잘라낸 콜라이더

    [Header("Mold")]
    public LayerMask moldLayer; // 곰팡이가 있는 Layer

    private float stepSizeX;
    private float stepSizeY;

    [ContextMenu("Calculate Area Percentage")]
    public void CalculateAreaPercentage()
    {
        if (foodCollider == null || cutColliders == null || cutColliders.Count == 0)
        {
            //Debug.LogError("콜라이더를 반드시 할당해주세요!");
            return;
        }

        stepSizeX = areaSize.x / resolution;
        stepSizeY = areaSize.y / resolution;

        int totalFoodHits = 0;
        int cutHits = 0;
        int moldHits = 0;
        int moldCount = 0;

        Vector2 origin = (Vector2)transform.position - areaSize * 0.5f;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                Vector2 checkPoint = origin + new Vector2(x * stepSizeX, y * stepSizeY);

                bool hitFood = foodCollider.OverlapPoint(checkPoint);
                if (!hitFood) continue;

                totalFoodHits++;

                // 곰팡이 먼저 체크
                Collider2D[] moldHitsArray = Physics2D.OverlapPointAll(checkPoint, moldLayer);
                if (moldHitsArray.Length > 0)
                {
                    moldCount = moldHitsArray.Length;
                    moldHits++;
                    continue; // 곰팡이에 닿은 곳은 잘라낸 영역 계산 안 함
                }

                // 잘라낸 영역 체크
                if (IsInCutArea(checkPoint))
                {
                    cutHits++;
                }
            }
        }

        int remainingHits = totalFoodHits - cutHits - moldHits;
        float remainingPercentage = totalFoodHits == 0 ? 0f : ((float)remainingHits / totalFoodHits) * 100f;
        //곰팡이가 덮인 비율
        float moldPercentage = totalFoodHits == 0 ? 0f : ((float)moldHits / remainingHits) * 100f;

        Debug.Log($"음식 전체 픽셀 수: {totalFoodHits}");
        Debug.Log($"잘라낸 픽셀 수: {cutHits}");
        Debug.Log($"곰팡이 덮인 픽셀 수: {moldHits}");
        Debug.Log($"곰팡이 덮인 갯수: {moldCount}");
        Debug.Log($"남은 음식 영역: {remainingPercentage:F2}%");
        Debug.Log($"곰팡이가 덮인 비율: {moldPercentage:F2}%");

        Manager.Kitchen.ResultRemainingPercentage += remainingPercentage;
        Manager.Kitchen.MoldPercentage += moldPercentage;
    }
    // 영역 가시화를 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, areaSize);
    }

    private bool IsInCutArea(Vector2 point)
    {
        foreach (var collider in cutColliders)
        {
            if (collider != null && collider.OverlapPoint(point))
                return true;
        }
        return false;
    }

    public void ResetCheckArea()
    {
        foodCollider = null;

        for (int i = 0;i < cutColliders.Count;i++)
        {
            Destroy(cutColliders[i].transform.gameObject);
        }
        cutColliders.Clear();
    }
    public void SetFoodCollider(Collider2D Collider)
    {
        foodCollider = Collider;
    }
}