using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawLine : MonoBehaviour
{
    private bool _isDrawing = false;

    [Header("line option")]
    [SerializeField] private float minDistance;
    [SerializeField] private float lineWidth;
    [SerializeField] private Color lineColor;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Draw();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Draw()
    {
        if (Input.GetMouseButton(0) && !_isDrawing) //그리는 중이 아니라면 마우스를 눌렀을 때 시작
        {
            _isDrawing = true;
            StartCoroutine(StartDraw(new GameObject("line")));
        }
    }

    private IEnumerator StartDraw(GameObject line)
    {
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>(); // 마우스를 따라 선을 그리기 위한 LineRenderer
        lineRenderer.startWidth = lineWidth;
        lineRenderer.material.color = lineColor;
        lineRenderer.positionCount = 1;

        EdgeCollider2D edgeCollider2D = line.AddComponent<EdgeCollider2D>(); // 폐곡선 충돌 확인을 위한 edgeCollider2D

        List<Vector2> v = new(); // line의 포지션 정보를 저장하기 위한 Vector2리스트
        bool isShape = false; // 폐곡선 생성 여부

        Vector2 previousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 과도한 LineRenderer 점 생성 방지를 위한 이전 위치와 현재 위치
        Vector2 currentPosition;

        lineRenderer.SetPosition(0, previousPosition); // 마우스 위치를 처음 위치로 설정

        while (Input.GetMouseButton(0)) // 버튼을 누르는 동안
        {
            currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(currentPosition, previousPosition) < minDistance) //
            {
                yield return null;
                continue;
            }
            
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition); // 새로운 위치로 선 그리기
            previousPosition = currentPosition;
            
            if(lineRenderer.positionCount < 3) //
            {
                yield return null;
                continue;
            }

            v.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 3));
            edgeCollider2D.SetPoints(v); // 마지막에서 3번째 점 까지만 콜라이더 추가 (자신과의 충돌 방지)

            RaycastHit2D hit = Physics2D.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 2),
                lineRenderer.GetPosition(lineRenderer.positionCount - 1) - lineRenderer.GetPosition(lineRenderer.positionCount - 2),
                Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineRenderer.GetPosition(lineRenderer.positionCount - 2)),
                ~(1 << LayerMask.NameToLayer("SlicedArea"))); // 마지막으로 생성한 선에서 충돌 판정, 기존 폐곡선 영역과는 충돌하지 않도록 마스크 설정

            if (hit) // 기존 선과 충돌했다면 (폐곡선이 만들어졌다면)
            {
                isShape = true;
                Destroy(edgeCollider2D);

                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                /*int index = 0; // 일직선 위의 점 찾기

                for(int i = 0; i < lineRenderer.positionCount - 2; i++)
                {
                    float ccw = CCW(lineRenderer.GetPosition(i), hit.point, lineRenderer.GetPosition(i + 1));

                    if (ccw > -0.01f && ccw < 0.01f)
                    {
                        index = i;
                        break;
                    }
                }*/

                float minDist = Mathf.Infinity; // 가장 가까운 점 찾기
                int index = 0;

                for(int i = 0; i < lineRenderer.positionCount - 2; i++)
                {
                    if (Vector2.Distance(lineRenderer.GetPosition(i), hit.point) < minDist)
                    {
                        minDist = Vector2.Distance(lineRenderer.GetPosition(i), hit.point);
                        index = i;
                    }
                }

                lineRenderer.SetPosition(index, hit.point);

                v = new List<Vector2>(new Vector2[lineRenderer.positionCount - index]);

                for (int i = index; i < lineRenderer.positionCount; i++)
                {
                    v[i - index] = lineRenderer.GetPosition(i);
                    lineRenderer.SetPosition(i - index, v[i - index]);
                }

                lineRenderer.positionCount = lineRenderer.positionCount - index;

                PolygonCollider2D polygonCollider2D = line.AddComponent<PolygonCollider2D>();
                polygonCollider2D.SetPath(0, v);
                break;
            }

            yield return null;
        }

        if (!isShape) Destroy(line); // 폐곡선이 아니라면 line 삭제

        line.layer = LayerMask.NameToLayer("SlicedArea");

        while (Input.GetMouseButton(0)) // 마우스를 한 번 떼야 다시 그릴 수 있도록
        {
            yield return null;
        }

        _isDrawing = false;

        yield break;
    }

    private float CCW(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p2.x - p1.y) * (p3.y - p1.y) - (p3.x - p1.x) * (p2.y - p1.y);
    }
}
