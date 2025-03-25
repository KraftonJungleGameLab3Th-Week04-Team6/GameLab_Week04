using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawTemp : MonoBehaviour
{
    public Material redLineMaterial;
    public Material blueLineMaterial;
    public float lineWidth = 0.1f;

    private LineRenderer currentLine;
    private EdgeCollider2D currentCollider;
    private List<Vector3> points = new List<Vector3>();
    private List<GameObject> drawnLines = new List<GameObject>();
    private Camera cam;

    public bool redDrawing_possible = true;
    public bool blueDrawing_possible = true;
    public GameObject redSlider;
    public GameObject blueSlider;
    public float redDrawingGage;
    public float blueDrawingGage;

    private string ColorCheck;
    Rigidbody2D rb;
    GameObject lineObj;

    // 라인 그리기 시작 여부
    private bool isLeftDrawing = false;
    private bool isRightDrawing = false;
    
    // ▼ 추가: 2D 물리 머티리얼(빙판 효과용)
    public PhysicsMaterial2D icePhysicsMaterial;
    
    private bool firstLineDrawn = false;
    
    public GameObject bulletTimeOverlay;  // 전체 화면을 덮는 검은색 이미지(초기 알파 = 0)
    public float normalCameraSize = 5f;  // 일반 상태에서 카메라의 orthographicSize
    public float zoomedCameraSize = 3f;  // 불릿타임 시 목표 줌인 값
    private Coroutine overlayCoroutine;
    void Start()
    {
        // 슬라이더 값을 가져옴
        redDrawingGage = redSlider.GetComponent<Slider>().value;
        blueDrawingGage = blueSlider.GetComponent<Slider>().value;
        cam = Camera.main;
    }

    void Update()
    {
        if (!KDY_GameManager.instance.isGameStart)
        {
            return;
        }
        
        // 슬라이더 값을 실시간으로 업데이트
        redSlider.GetComponent<Slider>().value = redDrawingGage;
        blueSlider.GetComponent<Slider>().value = blueDrawingGage;

        // 빨간색 슬라이더의 게이지가 0 이하일 때 라인 종료
        if (redDrawingGage <= 0)
        {
            FinishLine();
            redDrawing_possible = false;
        }
        else
        {
            redDrawing_possible = true;
        }

        // 파란색 슬라이더의 게이지가 0 이하일 때 라인 종료
        if (blueDrawingGage <= 0)
        {
            FinishLine();
            blueDrawing_possible = false;
        }
        else
        {
            blueDrawing_possible = true;
        }

        // 빨간색과 파란색 슬라이더 각각 차게 처리
        if (redDrawingGage < 100)
        {
            redDrawingGage += 20 * Time.deltaTime;
        }
        if (blueDrawingGage < 100)
        {
            blueDrawingGage += 20 * Time.deltaTime;
        }

        // 왼쪽 마우스 버튼 클릭 시 빨간색 선 그리기
        if (Input.GetMouseButtonDown(0) && redDrawing_possible)
        {
            if (!KDY_GameManager.instance.isGameStart) return;
            ColorCheck = "RED_Line";
            StartLine(true,ColorCheck);

            // ▼ 변경/추가: 좌클릭 드로잉 시작
            isLeftDrawing = true;
            CheckBulletTime();

        }
        // 오른쪽 마우스 버튼 클릭 시 파란색 선 그리기
        else if (Input.GetMouseButtonDown(1) && blueDrawing_possible)
        {
            if (!KDY_GameManager.instance.isGameStart) return;
            ColorCheck = "BLUE_Line";
            StartLine(false, ColorCheck);

            // ▼ 변경/추가: 우클릭 드로잉 시작
            isRightDrawing = true;
            CheckBulletTime();
        }

        // 마우스 버튼을 누르고 있을 때 선 업데이트
        else if (Input.GetMouseButton(0) && redDrawing_possible)
        {
            UpdateLine(true);
            redDrawingGage -= 200 * Time.deltaTime; // 초당 감소
        }
        else if (Input.GetMouseButton(1) && blueDrawing_possible)
        {
            UpdateLine(false);
            blueDrawingGage -= 200 * Time.deltaTime; // 초당 감소
        }

        // 3) 마우스 버튼 Up
        else if (Input.GetMouseButtonUp(0))
        {
            // 좌클릭 드로잉 종료
            isLeftDrawing = false;
            FinishLine();
            CheckBulletTime(); // ▼ 마우스 뗀 후 다시 체크
        }
        else if (Input.GetMouseButtonUp(1))
        {
            // 우클릭 드로잉 종료
            isRightDrawing = false;
            FinishLine();
            CheckBulletTime(); // ▼ 마우스 뗀 후 다시 체크
        }

        // 마우스 휠 버튼 처리
        if (Input.GetMouseButtonDown(2))  // 마우스 휠 버튼 클릭
        {
            Debug.Log("마우스 휠 버튼 클릭됨!");
        }
    }

    public void DrawItem() // drawIten 호출 함수
    {
        redDrawingGage = 100;
        blueDrawingGage = 100;
    }

    // ▼ 변경/추가: 드로잉 여부를 확인해서 Time.timeScale 변경
    private void CheckBulletTime()
    {
        if (isLeftDrawing || isRightDrawing)
        {
            Time.timeScale = 0.05f;  // 느려지는 배율
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            // 배경 오버레이 효과: 목표 알파 0.8 (80% 불투명)로 전환
            if (bulletTimeOverlay != null)
            {
                if (overlayCoroutine != null)
                    StopCoroutine(overlayCoroutine);
                overlayCoroutine = StartCoroutine(AnimateOverlayAlpha(0.8f, 0.2f));
            }
            // 카메라 줌인 효과: 목표 orthographicSize = 4.75 (줌인 효과)
            if (cam != null)
            {
                StartCoroutine(AnimateCameraZoom(4.70f, 0.2f));
            }
        }
        else
        {
            Time.timeScale = 1f;  // 원래 속도 복귀
            Time.fixedDeltaTime = 0.02f; // 기본값

            // 오버레이 복귀 효과: 목표 알파 0으로 전환
            if (bulletTimeOverlay != null)
            {
                if (overlayCoroutine != null)
                    StopCoroutine(overlayCoroutine);
                overlayCoroutine = StartCoroutine(AnimateOverlayAlpha(0f, 0.2f));
            }
            // 카메라 줌아웃 효과: 목표 orthographicSize = 5 (기본값)
            if (cam != null)
            {
                StartCoroutine(AnimateCameraZoom(5f, 0.2f));
            }
        }
    }
    private IEnumerator AnimateCameraZoom(float targetSize, float duration)
    {
        float startSize = cam.orthographicSize;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            yield return null;
        }
        cam.orthographicSize = targetSize;
    }
    private IEnumerator AnimateOverlayAlpha(float targetAlpha, float duration)
    {
        // bulletTimeOverlay는 씬 상에 배치된, 전체 화면을 덮는 오브젝트여야 합니다.
        // 이 오브젝트는 SpriteRenderer를 가지고 있어야 하며, 초기 알파는 0이어야 합니다.
        SpriteRenderer sr = bulletTimeOverlay.GetComponent<SpriteRenderer>();
        if (sr == null)
            yield break;

        float startAlpha = sr.color.a;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            Color col = sr.color;
            col.a = newAlpha;
            sr.color = col;
            yield return null;
        }
        // 최종 알파값을 보장합니다.
        Color finalCol = sr.color;
        finalCol.a = targetAlpha;
        sr.color = finalCol;
    }
    void StartLine(bool isRedLine, string colorcheck)
    {
        lineObj = new GameObject("DrawnLine");

        currentLine = lineObj.AddComponent<LineRenderer>();
        currentCollider = lineObj.AddComponent<EdgeCollider2D>();
        rb = lineObj.AddComponent<Rigidbody2D>();

        currentCollider.enabled = false;
        rb.simulated = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        currentCollider.isTrigger = false;
        currentCollider.edgeRadius = lineWidth / 2;
        //currentCollider에 physics material 추가
        currentCollider.sharedMaterial = icePhysicsMaterial;

        lineObj.tag = colorcheck;
        lineObj.layer = LayerMask.NameToLayer(colorcheck);

        

        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        currentLine.positionCount = 0;
        currentLine.useWorldSpace = true;

        currentLine.numCornerVertices = 5;
        currentLine.numCapVertices = 5;
        currentLine.textureMode = LineTextureMode.Tile;


        points.Clear();
        drawnLines.Add(lineObj);

        // 빨간색 혹은 파란색 라인 색상 지정
        if (isRedLine)
        {
            
            currentLine.material = redLineMaterial;
        }
        else
        {
            currentLine.material = blueLineMaterial;
        }

        StartCoroutine(DestroyLineAfterDelay(lineObj, 3f));
    }

    IEnumerator DestroyLineAfterDelay(GameObject lineObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(lineObj); // 3초 뒤에 라인 제거
    }

    void UpdateLine(bool isRedLine)
    {
        if (currentLine == null || currentCollider == null)
            return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // 카메라와의 거리 설정
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
    
        // 플레이어 오브젝트와의 최소 거리를 설정 (예: 1.0f)
        float minDistanceFromPlayer = 0.3f;
        if (Vector3.Distance(worldPos, KDY_PlayerController.instance.transform.position) < minDistanceFromPlayer)
        {
            // 플레이어와 너무 가까우면 점을 추가하지 않음
            return;
        }
    
        points.Add(worldPos);

        // Catmull-Rom 보간법으로 부드럽게 처리
        List<Vector3> smoothPoints = GetCatmullRomPositions(points, 10); // 각 구간마다 10개의 보간점 생성

        // 보간된 결과를 라인렌더러에 적용
        currentLine.positionCount = smoothPoints.Count;
        for (int i = 0; i < smoothPoints.Count; i++)
        {
            currentLine.SetPosition(i, smoothPoints[i]);
        }

        // 보간된 결과를 사용해 콜라이더 업데이트
        UpdateColliderWithPoints(smoothPoints);
    }


    private List<Vector3> GetCatmullRomPositions(List<Vector3> pts, int subdivisions)
    {
        List<Vector3> output = new List<Vector3>();
        int count = pts.Count;
        if (count < 2)
            return new List<Vector3>(pts);

        // 첫번째와 마지막 점을 보간에서 사용할 수 있도록 양 끝을 복제
        List<Vector3> pointsExtended = new List<Vector3>();
        pointsExtended.Add(pts[0]); // 맨 앞 복제
        pointsExtended.AddRange(pts);
        pointsExtended.Add(pts[count - 1]); // 맨 뒤 복제

        // Catmull-Rom 보간 적용 (i = 1부터 pointsExtended.Count - 2 까지)
        for (int i = 1; i < pointsExtended.Count - 2; i++)
        {
            Vector3 p0 = pointsExtended[i - 1];
            Vector3 p1 = pointsExtended[i];
            Vector3 p2 = pointsExtended[i + 1];
            Vector3 p3 = pointsExtended[i + 2];

            // 각 구간마다 subdivisions개의 점 생성
            for (int j = 0; j < subdivisions; j++)
            {
                float t = j / (float)subdivisions;
                Vector3 newPoint = CatmullRom(p0, p1, p2, p3, t);
                output.Add(newPoint);
            }
        }
        // 마지막 원래 점 추가
        output.Add(pointsExtended[pointsExtended.Count - 2]);
        return output;
    }
    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * ((2f * p1) +
                       (-p0 + p2) * t +
                       (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
                       (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }private void UpdateColliderWithPoints(List<Vector3> smoothPoints)
    {
        if (currentCollider == null || smoothPoints.Count < 2)
            return;

        Vector2[] colliderPoints = new Vector2[smoothPoints.Count];
        for (int i = 0; i < smoothPoints.Count; i++)
        {
            colliderPoints[i] = new Vector2(smoothPoints[i].x, smoothPoints[i].y);
        }
        currentCollider.SetPoints(new List<Vector2>(colliderPoints));
    }

    void FinishLine()
    {
        if (lineObj != null)
        {
            if (currentCollider != null)
            {
                currentCollider.enabled = true;
            }

            if (rb != null)
            {
                rb.simulated = true;
            }

            // 기존 currentLine, currentCollider 초기화
            if (currentLine != null)
            {
                currentLine = null;
            }
            if (currentCollider != null)
            {
                currentCollider = null;
            }
        
            Color color = lineObj.GetComponent<LineRenderer>().material.color;
            color.a = 1.0f;
            lineObj.GetComponent<LineRenderer>().material.color = color;
        
            // 겹침 해결: 플레이어와 선(Collider) 간의 최소 분리 벡터 계산
            Collider2D playerCol = KDY_PlayerController.instance.GetComponent<Collider2D>();
            Collider2D lineCol = lineObj.GetComponent<EdgeCollider2D>();
            if (playerCol != null && lineCol != null)
            {
                ColliderDistance2D distanceInfo = playerCol.Distance(lineCol);
                if (distanceInfo.isOverlapped)
                {
                    // separation vector: 반대 방향으로 (-distance) 만큼 이동
                    Vector2 separation = distanceInfo.normal * -distanceInfo.distance;
                    lineObj.transform.position += (Vector3)separation;
                }
            }

            if (!firstLineDrawn)
            {
                firstLineDrawn = true;
                KDY_GameManager.instance.GameStart();
            }
        }
    }


    void UpdateCollider()
    {
        if (currentCollider == null || points.Count < 2) return;

        Vector2[] colliderPoints = new Vector2[points.Count];
        for (int i = 0; i < points.Count; i++)
        {
            colliderPoints[i] = new Vector2(points[i].x, points[i].y);
        }

        currentCollider.SetPoints(new List<Vector2>(colliderPoints));
    }

    void EraseAllLines()
    {
        foreach (GameObject line in drawnLines)
        {
            Destroy(line);
        }
        drawnLines.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Line collided with: {collision.gameObject.name}");
    }
}
