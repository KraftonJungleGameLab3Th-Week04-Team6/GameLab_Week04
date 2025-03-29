using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class MoldSpawner : MonoBehaviour
{
    [Header("Spawner option")]
    public GameObject moldPrefab;

    public HashSet<Transform> MoldSet { get { return _moldSet; } }

    private int _maxMoldCount = 230;
    private float _spawnIntervalTime = 0.1f;
    private float _spreadRadius = 0.3f; // 퍼지는 범위
    private float _minDistanceBetweenMolds = 0.2f;
    private int _startPoint;

    private PolygonCollider2D _vegCollider;
    private HashSet<Transform> _moldSet = new(); // 남아있는 곰팡이 관리 해시셋
    private int _currentMoldCount = 0; // 누적 소환된 곰팡이 수
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
        _moldSet.Add(firstMold.transform);
        _currentMoldCount++;

        yield return new WaitForSeconds(_spawnIntervalTime);

        // 2. 계속 주변으로 퍼지기
        while (_currentMoldCount < _maxMoldCount)
        {
            List<Transform> newMolds = new List<Transform>();

            foreach (Transform mold in _moldSet.ToList())
            {
                if (mold == null) continue;
                Vector2 randomOffset = Random.insideUnitCircle * _spreadRadius;
                Vector2 spawnPos = (Vector2)mold.position + randomOffset;

                if (_vegCollider.OverlapPoint(spawnPos) && !IsTooCloseToExistingMold(spawnPos) && !IsOverlappingOtherCollider(spawnPos))
                {
                    GameObject newMold = Instantiate(moldPrefab, spawnPos, Quaternion.identity, transform);
                    _moldSet.Add(newMold.transform);
                    _currentMoldCount++;

                    if (_currentMoldCount >= _maxMoldCount)
                        break;
                }
            }

            yield return new WaitForSeconds(_spawnIntervalTime);
        }
    }

    public void CheckMoldSetCount()
    {
        if (_moldSet.Count == 0)
        {
            _miniGameController.OnEndButton();
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
        foreach (Transform mold in _moldSet.ToList())
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