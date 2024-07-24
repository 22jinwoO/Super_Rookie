using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedTarget : AbsSearchTarget, ISearchTarget
{
    public bool IsSearch { get; set; }
    public Transform TargetUnit { get; set; }
    public Transform _thistargetUnit;

    public IUnitActState ActState { get; set; }
    public CapsuleCollider2D TargetColider { get; set; }

    private void Awake()
    {
        unitDataCs = GetComponent<BaseUnitData>();
    }

    private void Update()
    {
        _thistargetUnit = TargetUnit;
    }
    public void SearchTarget()
    {

        {
            // 오버랩 스피어 생성
            Collider2D[] _cols = Physics2D.OverlapCircleAll((Vector2)transform.position, unitDataCs.UnitSightRange, layerMask);


            // 탐지된 적이 없다면 함수 탈출
            if (_cols.Length <= 0)
            {
                //unitInfoCs._nav.isStopped = true;
                ActState.Exit();
                return;
            }

            // 가장 가까운 적을 의미하는 변수
            Transform _shortestTarget = null;

            // 거리 무한 값 할당
            float _shortestDistance = _shortestDistance = Vector3.Distance(transform.position, _cols[0].transform.position);

            // 선택 정렬로 가장 가까운 거리의 적 추출
            for (int i = 0; i < _cols.Length; i++)
            {

                int location = i;

                for (int j = i + 1; j < _cols.Length; j++)
                {
                    if (_shortestDistance >= Vector3.Distance(transform.position, _cols[j].transform.position))
                    {
                        _shortestDistance = Vector3.Distance(transform.position, _cols[j].transform.position);
                        _shortestTarget = _cols[j].transform;
                        location = j;
                    }
                }

                Collider2D temp = null;

                temp = _cols[location];
                _cols[location] = _cols[i];
                _cols[i] = temp;

            }

            _shortestTarget = _cols[0].transform;

            //시야범위 밖의 타겟인데 자꾸 타겟을 인식해서 예외처리 구문 추가
            if (_shortestDistance > unitDataCs.UnitSightRange)
            {
                IsSearch = false;
                _thistargetUnit = TargetUnit;
                ActState.Exit();
                return;
            }

            // 거리가 가장 가까운 적 타겟을 TargetUnit 변수에 할당
            TargetUnit = _shortestTarget; 

            TargetColider = TargetUnit.GetComponent<CapsuleCollider2D>();

            // 타겟 할당
            _thistargetUnit = TargetUnit;

            // 현재 상태 탈출
            ActState.Exit();
        }
    }
}

