using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldSpawner : MonoBehaviour
{
    public GameObject moldPrefab;
    public int maxMoldCount = 100;
    public float spawnIntervalTime = 0.1f;
    public float spreadRadius = 0.3f; // 퍼지는 범위
    public float minDistanceBetweenMolds = 0.2f;

    private PolygonCollider2D _vegCollider;
    private List<Transform> _moldCenters = new List<Transform>(); // 기존 곰팡이 위치들
    private int _currentCount = 0;

    void Start()
    {
        _vegCollider = GetComponent<PolygonCollider2D>();

        if (_vegCollider == null)
        {
            Debug.LogError("PolygonCollider2D가 필요해요!");
            return;
        }

        StartCoroutine(SpreadMold());
    }

    IEnumerator SpreadMold()
    {
        // 1. 시작 위치 하나 생성
        Vector2 startPos = GetRandomPointInside();
        GameObject firstMold = Instantiate(moldPrefab, startPos, Quaternion.identity, transform);
        _moldCenters.Add(firstMold.transform);
        _currentCount++;

        yield return new WaitForSeconds(spawnIntervalTime);

        // 2. 계속 주변으로 퍼지기
        while (_currentCount < maxMoldCount)
        {
            List<Transform> newMolds = new List<Transform>();

            foreach (var center in _moldCenters)
            {
                Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
                Vector2 spawnPos = (Vector2)center.position + randomOffset;

                if (_vegCollider.OverlapPoint(spawnPos) && !IsTooCloseToExistingMold(spawnPos) && !IsOverlappingOtherCollider(spawnPos))
                {
                    GameObject mold = Instantiate(moldPrefab, spawnPos, Quaternion.identity, transform);
                    newMolds.Add(mold.transform);
                    _currentCount++;

                    if (_currentCount >= maxMoldCount)
                        break;

                }
            }
            print(_currentCount);
            // 새로 생긴 곰팡이들을 중심 리스트에 추가
            _moldCenters.AddRange(newMolds);

            yield return new WaitForSeconds(spawnIntervalTime);
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
            if (Vector2.Distance(pos, mold.position) < minDistanceBetweenMolds)
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
}