using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawLine : MonoBehaviour
{
    private bool _isDrawing = false;

    [Header("line option")]
    [SerializeField] private float minDistance;
    [SerializeField] private float lineWidth;
    [SerializeField] private Color lineColor;
    [SerializeField] private float CorrectionDistance;
    [SerializeField] private Color insideColor;

    private void Awake()
    {
        
    }

    private void Update()
    {
        Draw();

        if (Input.GetKeyDown(KeyCode.R)) // 디버깅용
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Draw()
    {
        if (Input.GetMouseButton(0) && !_isDrawing) //그리는 중이 아니라면 마우스를 눌렀을 때 시작
        {
            _isDrawing = true;
            StartCoroutine(Drawing(new GameObject("line")));
        }
    }

    private IEnumerator Drawing(GameObject line)
    {
        line.layer = LayerMask.NameToLayer("DrawingLine");

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>(); // 마우스를 따라 선을 그리기 위한 LineRenderer
        lineRenderer.startWidth = lineWidth;
        lineRenderer.material.color = lineColor;
        lineRenderer.positionCount = 1;
        lineRenderer.sortingOrder = 10;

        EdgeCollider2D edgeCollider2D = line.AddComponent<EdgeCollider2D>(); // 폐곡선 충돌 확인을 위한 edgeCollider2D

        List<Vector2> pointsList = new(); // line의 포지션 정보를 저장하기 위한 Vector2리스트
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

            pointsList.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 3));
            edgeCollider2D.SetPoints(pointsList); // 마지막에서 3번째 점 까지만 콜라이더 추가 (자신과의 충돌 방지)

            RaycastHit2D hit = Physics2D.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 2)
                , lineRenderer.GetPosition(lineRenderer.positionCount - 1) - lineRenderer.GetPosition(lineRenderer.positionCount - 2)
                , Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), lineRenderer.GetPosition(lineRenderer.positionCount - 2))
                , 1 << LayerMask.NameToLayer("DrawingLine")); // 마지막으로 생성한 선에서 충돌 판정, 그리는 선하고만 충돌하도록 마스크 설정

            if (hit) // 기존 선과 충돌했다면 (폐곡선이 만들어졌다면)
            {
                isShape = true;

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

                pointsList = new List<Vector2>(new Vector2[lineRenderer.positionCount - index - 1]); // 시작점과 끝점을 같게 만들면 메시폴리곤 생성이 이상해짐

                for (int i = index; i < lineRenderer.positionCount - 1; i++)
                {
                    pointsList[i - index] = lineRenderer.GetPosition(i);
                }

                lineRenderer.positionCount = lineRenderer.positionCount - index - 1;
                lineRenderer.SetPositions(pointsList.ConvertAll(elem => (Vector3)elem).ToArray());

                PolygonCollider2D polygonCollider2D = line.AddComponent<PolygonCollider2D>(); 
                polygonCollider2D.SetPath(0, pointsList);
                break;
            }

            yield return null;
        }

        if (!isShape) // 선을 완전히 잇지 않아도 완성되도록 보정
        {
            edgeCollider2D.SetPoints(pointsList.GetRange(1, pointsList.Count - 1));

            if (Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1)) < CorrectionDistance
                && !Physics2D.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1)
                , lineRenderer.GetPosition(0) - lineRenderer.GetPosition(lineRenderer.positionCount - 1)
                , Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1))
                , 1 << LayerMask.NameToLayer("DrawingLine"))) // 시작점과 끝점 사이에 다른 선이 없으면서 가깝다면 폐곡선으로 보정
            {
                pointsList.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 1));

                PolygonCollider2D polygonCollider2D = line.AddComponent<PolygonCollider2D>();
                polygonCollider2D.SetPath(0, pointsList);

                isShape = true;
            }

            edgeCollider2D.SetPoints(pointsList);
        }

        line.layer = LayerMask.NameToLayer("SlicedArea");
        Destroy(edgeCollider2D);
        Destroy(lineRenderer);

        if (isShape) // 폐곡선이 완성되었다면 채우기
        {
            Mesh filledMesh = new()
            {
                vertices = pointsList.ConvertAll(elem => (Vector3)elem).ToArray(),
                triangles = new Triangulator(pointsList.ToArray()).Triangulate()
            };

            MeshRenderer meshRenderer = line.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = line.AddComponent<MeshFilter>();

            meshRenderer.material = new Material(Shader.Find("UI/Default"));
            meshRenderer.material.color = insideColor;
            meshFilter.mesh = filledMesh;
        }
        else // 폐곡선이 완성되지 않았다면 선 삭제
        {
            Destroy(line);
        }

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
