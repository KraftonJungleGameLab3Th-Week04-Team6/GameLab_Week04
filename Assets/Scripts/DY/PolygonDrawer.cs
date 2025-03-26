using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class PolygonDrawer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private PolygonCollider2D polygonCollider;

    [Header("Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float minDistanceBetweenPoints = 0.1f;

    private readonly List<Vector2> drawPoints = new List<Vector2>();
    private bool isDrawing = false;

    private void Reset()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleDrawingInput();
    }

    private void HandleDrawingInput()
    {
        if (Input.GetMouseButtonDown(0))
            StartDrawing();

        if (Input.GetMouseButton(0) && isDrawing)
            UpdateDrawing();

        if (Input.GetMouseButtonUp(0) && isDrawing)
            EndDrawing();
    }

    private void StartDrawing()
    {
        isDrawing = true;
        drawPoints.Clear();
        lineRenderer.positionCount = 0;
    }

    private void UpdateDrawing()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (drawPoints.Count == 0 || Vector2.Distance(drawPoints[^1], mouseWorldPos) >= minDistanceBetweenPoints)
        {
            drawPoints.Add(mouseWorldPos);
            lineRenderer.positionCount = drawPoints.Count;
            lineRenderer.SetPosition(drawPoints.Count - 1, mouseWorldPos);
        }
    }

    private void EndDrawing()
    {
        isDrawing = false;

        // 그린 경로가 폐곡선이 아니면 자동으로 닫아줌
        if (drawPoints.Count > 2 && Vector2.Distance(drawPoints[0], drawPoints[^1]) > minDistanceBetweenPoints)
        {
            drawPoints.Add(drawPoints[0]);
            lineRenderer.positionCount = drawPoints.Count;
            lineRenderer.SetPosition(drawPoints.Count - 1, drawPoints[0]);
        }

        UpdatePolygonCollider();
    }

    private void UpdatePolygonCollider()
    {
        polygonCollider.pathCount = 1;

        Vector2[] simplifiedPoints = SimplifyPath(drawPoints);
        polygonCollider.SetPath(0, simplifiedPoints);
    }

    // 선택적으로 경로를 단순화 (포인트 수를 줄여 성능 최적화)
    private Vector2[] SimplifyPath(List<Vector2> points, float tolerance = 0.05f)
    {
        // 간단한 경로를 그대로 반환하거나 Douglas-Peucker 알고리즘을 구현 가능
        return points.ToArray();
    }

    // 외부에서 특정 좌표가 폴리곤 내에 있는지 확인하는 메서드 제공
    public bool IsPointInsidePolygon(Vector2 point)
    {
        return polygonCollider.OverlapPoint(point);
    }
}
