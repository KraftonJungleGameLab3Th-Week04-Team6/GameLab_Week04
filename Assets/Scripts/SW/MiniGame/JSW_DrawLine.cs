using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JSW_DrawLine : MonoBehaviour
{
    public GameObject sliceEffect;

    private bool _isDrawing = false;

    [Header("line option")]
    [SerializeField] private float minDistance;
    [SerializeField] private float lineWidth;
    [SerializeField] private Color lineColor;
    [SerializeField] private float correctionDistance;
    [SerializeField] private Color insideColor;

    private MiniGameController _minigameController;
    private JSW_CheckArea _checkArea;
    private KitchenCamera _kitchenCamera;

    private void Start()
    {
        _minigameController = FindAnyObjectByType<MiniGameController>();
        _checkArea = FindAnyObjectByType<JSW_CheckArea>();
        _kitchenCamera = FindAnyObjectByType<KitchenCamera>();
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Input.GetMouseButton(0) && !_isDrawing && _minigameController.isStart) //그리는 중이 아니라면 마우스를 눌렀을 때 시작
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
        lineRenderer.material = MakeMaterial(lineColor);
        lineRenderer.sortingOrder = 1;

        EdgeCollider2D edgeCollider2D = line.AddComponent<EdgeCollider2D>(); // 폐곡선 충돌 확인을 위한 edgeCollider2D

        bool isShape = false; // 폐곡선 생성 여부

        Vector2 previousPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 과도한 LineRenderer 점 생성 방지를 위한 이전 위치와 현재 위치
        Vector2 currentPosition;

        List<Vector2> pointsList = new(); // line의 포지션 정보를 저장하기 위한 Vector2리스트

        int correctionIndex = -1; // 미완성 곡선의 보정 체크를 할 lineRenderer 포지션 인덱스

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, previousPosition); // 마우스 위치를 선의 처음 위치로 설정
        lineRenderer.SetPosition(1, previousPosition);
        pointsList.Add(previousPosition);
        pointsList.Add(previousPosition);
        edgeCollider2D.SetPoints(pointsList); // 판정은 3번째 점부터 시작해야 하므로 lineRenderer과 edgeCollider2D에 2개를 미리 추가

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

            pointsList.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 3));
            edgeCollider2D.SetPoints(pointsList); // 마지막에서 3번째 점 까지만 콜라이더 추가 (자신과의 충돌 방지)

            if (lineRenderer.positionCount > 5 && Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount - 1)) < correctionDistance) // 첫 점과 마지막 점의 거리
            {
                correctionIndex = lineRenderer.positionCount - 1;
            }

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
                    if(CCW(lineRenderer.GetPosition(i), hit.point, lineRenderer.GetPosition(i + 1))==0){
                    {
                        index = i;
                        break;
                    }
                }*/

                float minDist = Mathf.Infinity; // 가장 가까운 점 찾기
                int index = -1;

                for (int i = 1; i < lineRenderer.positionCount - 2; i++)
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

                break;
            }

            yield return null;
        }

        if (!isShape && lineRenderer.positionCount >= 4 && correctionIndex >= 3) // 선이 이어지지 않아도 완성되도록 보정 (positionCount 4 이상부터 선 2개)
        {
            pointsList.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 2));
            edgeCollider2D.SetPoints(pointsList.GetRange(4, correctionIndex - 2)); // pointsList 0, 1, 2, 3는 시작점임(lineRenderer도 시작점이 겹치니까), correctionIndex는 개수-1임

            if (!Physics2D.Raycast(lineRenderer.GetPosition(correctionIndex)
                , lineRenderer.GetPosition(0) - lineRenderer.GetPosition(correctionIndex)
                , Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(correctionIndex))
                , 1 << LayerMask.NameToLayer("DrawingLine")))
            {
                isShape = true;

                pointsList = new List<Vector2>(new Vector2[correctionIndex]);

                for (int i = 0; i < correctionIndex; i++)
                {
                    pointsList[i] = lineRenderer.GetPosition(i + 1);
                }

                int[] ccwCount = new int[2];

                int convexCheck = 0; // 볼록 다각형인지 확인하는 변수, 0은 아님 1은 반시계 -1은 시계

                for (int i = 1; i < pointsList.Count - 2; i++)
                {
                    if (CCW(pointsList[i], pointsList[i + 1], pointsList[i + 2]) > 0) ccwCount[0]++; // 반시계
                    else if ((CCW(pointsList[i], pointsList[i + 1], pointsList[i + 2]) < 0)) ccwCount[1]++; // 시계
                }

                if (ccwCount[0] > 5 * ccwCount[1]) convexCheck = 1;
                else if (ccwCount[1] > 5 * ccwCount[0]) convexCheck = -1;

                if (convexCheck != 0) // 볼록 다각형이라면 컨벡스 보정
                {

                    if (convexCheck * CCW(pointsList[0], pointsList[1], pointsList[pointsList.Count - 1]) > 0) // 1이면 처음에서 보정
                    {
                        correctionIndex = pointsList.Count - 1;

                        while (correctionIndex > 1)
                        {
                            if (CCW(pointsList[0], pointsList[correctionIndex], pointsList[correctionIndex - 1]) != convexCheck) break;

                            correctionIndex--;
                        }

                        pointsList = new(pointsList.GetRange(0, correctionIndex + 1));
                    }
                    else // -1이면 마지막에서 보정
                    {
                        correctionIndex = 0;

                        while (correctionIndex < pointsList.Count - 2)
                        {
                            if (CCW(pointsList[pointsList.Count - 1], pointsList[correctionIndex], pointsList[correctionIndex + 1]) == convexCheck) break;

                            correctionIndex++;
                        }

                        pointsList = new(pointsList.GetRange(correctionIndex, pointsList.Count - correctionIndex));
                    }
                }
                else // 아니라면 가장 가까운 점으로 보정
                {
                    while (correctionIndex > 2)
                    {
                        if (Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(correctionIndex))
                            < Vector2.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(correctionIndex - 1))) break;

                        correctionIndex--;
                    }

                    pointsList = new(pointsList.GetRange(0, correctionIndex));
                }
            }
        }

        line.layer = LayerMask.NameToLayer("SlicedArea");
        line.name = "area";
        Destroy(edgeCollider2D);
        Destroy(lineRenderer);

        if (isShape) // 폐곡선이 완성되었다면 폴리곤과 매시 생성
        {
            PolygonCollider2D polygonCollider2D = line.AddComponent<PolygonCollider2D>();
            polygonCollider2D.SetPath(0, pointsList);

            Mesh filledMesh = new()
            {
                vertices = pointsList.ConvertAll(elem => (Vector3)elem).ToArray(),
                triangles = new Triangulator(pointsList.ToArray()).Triangulate()
            };

            MeshRenderer meshRenderer = line.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = line.AddComponent<MeshFilter>();

            meshRenderer.material = MakeMaterial(insideColor);
            meshFilter.mesh = filledMesh;

            _checkArea.cutColliders.Add(polygonCollider2D);
            _kitchenCamera.SliceMoving();
            if (polygonCollider2D != null)
            {
                Instantiate(sliceEffect, polygonCollider2D.bounds.center, Quaternion.identity);
            }
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

    private Material MakeMaterial(Color color)
    {
        Material newMaterial = new Material(Shader.Find("UI/Default"));
        newMaterial.color = color;

        return newMaterial;
    }

    private int CCW(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float cross = (p2.x - p1.x) * (p3.y - p1.y) - (p3.x - p1.x) * (p2.y - p1.y);

        if (cross > 0) return 1;
        else if (cross < 0) return -1;
        else return 0;
    }
}