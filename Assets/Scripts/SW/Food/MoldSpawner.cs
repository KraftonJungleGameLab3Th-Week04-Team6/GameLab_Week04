using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MoldSpawner : MonoBehaviour
{

    [Header("Spawner option")]
    public GameObject moldPrefab;

    private int _maxMoldCount = 230;
    private float _spawnIntervalTime = 0.1f;
    private float _spreadRadius = 0.3f; // 퍼지는 범위
    private float _minDistanceBetweenMolds = 0.2f;
    private int _startPoint;

    private PolygonCollider2D _vegCollider;
    private List<Transform> _moldCenters = new List<Transform>(); // 기존 곰팡이 위치들
    private int _currentMoldCount = 0;
    private MiniGameController _miniGameController;



    void Start()
    {
        _vegCollider = GetComponent<PolygonCollider2D>();

        print(gameObject);

        if (_vegCollider == null)
        {
            return;
        }

        //StartMold();

        _miniGameController = FindAnyObjectByType<MiniGameController>();
        _miniGameController.OnSliceEndEvent += StopSpawn;
    }

    public void SettingMoldCount(int maxMoldCount, float spawnIntervalTime, float spreadRadius, float minDistanceBetweenMolds, int startPoint, float scale)
    {
        _maxMoldCount = maxMoldCount;
        _spawnIntervalTime = spawnIntervalTime;
        _spreadRadius = spreadRadius;
        _minDistanceBetweenMolds = minDistanceBetweenMolds;
        _startPoint = startPoint;
        StartCoroutine(ScaleUp(scale));
    }

    IEnumerator ScaleUp(float scale)
    {
        Vector3 targetScale = Vector3.one * scale;
        while (true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 30);
            if (Vector3.Distance(transform.localScale, targetScale)  < 0.01f)
            {
                transform.localScale = targetScale;
                break;
            }
            yield return null;
        }
    }

    public void StartMold()
    {
        for (int i = 0; i < _startPoint; i++)
        {
            StartCoroutine(SpreadMold());
        }
    }

    public void StopSpawn()
    {
        StopAllCoroutines();
    }

    IEnumerator SpreadMold()
    {
        // 1. 시작 위치 하나 생성
        Vector2 startPos = GetRandomPointInside();
        GameObject firstMold = Instantiate(moldPrefab, startPos, Quaternion.identity, transform);
        _moldCenters.Add(firstMold.transform);
        _currentMoldCount++;

        StartCoroutine(CheckingMoldCount());

        yield return new WaitForSeconds(_spawnIntervalTime);

        // 2. 계속 주변으로 퍼지기
        while (_currentMoldCount < _maxMoldCount)
        {
            List<Transform> newMolds = new List<Transform>();

            foreach (var center in _moldCenters)
            {
                if (center == null) continue;
                Vector2 randomOffset = Random.insideUnitCircle * _spreadRadius;
                Vector2 spawnPos = (Vector2)center.position + randomOffset;

                if (_vegCollider.OverlapPoint(spawnPos) && !IsTooCloseToExistingMold(spawnPos) && !IsOverlappingOtherCollider(spawnPos))
                {
                    GameObject mold = Instantiate(moldPrefab, spawnPos, Quaternion.identity, transform);
                    newMolds.Add(mold.transform);
                    _currentMoldCount++;

                    if (_currentMoldCount >= _maxMoldCount)
                        break;
                }
            }
            // 새로 생긴 곰팡이들을 중심 리스트에 추가
            _moldCenters.AddRange(newMolds);

            yield return new WaitForSeconds(_spawnIntervalTime);
        }
    }

    IEnumerator CheckingMoldCount()
    {
        while (true)
        {
            bool isEndding = true;
            foreach (var center in _moldCenters)
            {
                if (center != null)
                {
                    isEndding = false;
                }
            }
            if (isEndding)
            {
                _miniGameController.OnEndButton();
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    Vector2 GetRandomPointInside()
    {
        Bounds bounds = _vegCollider.bounds;
        for (int i = 0; i < 100; i++) // 무한루프 방지
        {
            Vector2 point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (_vegCollider.OverlapPoint(point))
                return point;
        }

        return _vegCollider.bounds.center; // fallback
    }

    bool IsTooCloseToExistingMold(Vector2 pos)
    {
        foreach (Transform mold in _moldCenters)
        {
            if (mold == null) continue;
            if (Vector2.Distance(pos, mold.position) < _minDistanceBetweenMolds)
                return true;
        }
        return false;
    }

    bool IsOverlappingOtherCollider(Vector2 pos)
    {
        // 현재 채소 콜라이더는 제외하고 검사
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);

        foreach (var hit in hits)
        {
            if (hit != null && hit != _vegCollider) // 자기 자신은 무시
            {
                return true; // 뭔가 겹침
            }
        }
        return false; // 겹치는 거 없음
    }

    private void OnDestroy()
    {
        _miniGameController.OnSliceEndEvent -= StopSpawn;
    }
}