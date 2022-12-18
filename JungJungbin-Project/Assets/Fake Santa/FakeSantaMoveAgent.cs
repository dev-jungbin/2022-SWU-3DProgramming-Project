using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class FakeSantaMoveAgent : MonoBehaviour
{
    // 순찰 지점들을 저장하기 위한 List 타입의 변수
    public List<Transform> wayPoints;

    // 다음 순찰 지점의 배열 Index
    public int nextIdx;

    // 걷기 속도
    float walkingSpeed = 1.5f;

    // 추적 속도
    float traceSpeed = 3.0f;

    // NavMeshAgent 컴포넌트를 저장할 변수
    NavMeshAgent agent;

    // 순찰 여부를 판단하는 변수
    bool patrolling;

    // 추적 대상 위치 저장하는 변수
    Vector3 traceTarget;

    void Start()
    {
        // NavMeshAgent 컴포넌트를 추출 후 변수에 저장
        agent = GetComponent<NavMeshAgent>();

        // 하이러키 뷰의 WayPointGroup 게임오브젝트 추출
        GameObject group = GameObject.Find("WayPointGroup");

        // 추출한 WayPointGroup이 null이 아닐 경우 수행
        if (group != null) {

            // WayPointGroup 하위에 있는 모든 Transform 컴포넌트 추출한 후 List 타입의 wayPoints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);

            // wayPoints 리스트의 순서를 랜덤하게 섞기
            wayPoints = GetShuffleList(wayPoints);
            
            // 배열의 첫 번째 항목 삭제
            wayPoints.RemoveAt(0);
        }
        // patrolling 세팅하는 함수 실행
        SetPatrolling(true);
    }

    // Update is called once per frame
    void Update()
    {
        // 순찰 모드가 아닐 경우 이후 로직 수행 안함
        if (!patrolling) return;
        
        // NavMeshAgent가 이동하고 있고 목적지에 도착했는지 여부 계산
        if (agent.velocity.magnitude >= 0.2f && agent.remainingDistance <= 0.5f) {
            // 다음 목적지의 배열 첨자 게산
            nextIdx = ++nextIdx % wayPoints.Count;

            // 다음 목적지로 이동 명령 수행
            MoveWayPoint();
        }
    }

    // patrolling 변수를 세팅하는 함수
    public void SetPatrolling(bool patrol) {

        // 파라미터로 들어온 patrol을 patrolling 변수로 지정
        patrolling = patrol;

        // 속도 지정
        agent.speed = walkingSpeed;

        // 다음 목적지로 이동 명령 수행
        MoveWayPoint();
    }

    // traceTarget을 세팅하는 함수
    public void SetTraceTargert(Vector3 pos) {

        // 파라미터로 들어온 pos를 traceTarget에 저장
        traceTarget = pos;
        
        // 속도 지정
        agent.speed = traceSpeed;

        // TraceTarget 함수 실행
        TraceTarget(traceTarget);
    }

    // 주인공을 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos) {

        // 최단거리 경로 게산이 끝나지 않으면 함수 종료
        if (agent.isPathStale) return;

        // 목적지 지정
        agent.destination = pos;
        
        // 이동 시작
        agent.isStopped = false;
    }

    // 다음 목적지까지 이동 명령 내리는 하수
    void MoveWayPoint() {
        // 최단거리 경로 게산이 끝나지 않으면 함수 종료
        if (agent.isPathStale) return;

        // wayPoints 배열에서 추출한 위치로 다음 목적지 지정
        agent.destination = wayPoints[nextIdx].position;

        // 네비게이션 기능을 활성화해서 이동 시작
        agent.isStopped = false;
    }

    // List를 타입에 상관없이(Generic) Shuffle하여 리턴하는 함수
    public List<T> GetShuffleList<T>(List<T> _list){

        // List의 인덱스 맨 뒤부터 맨 앞으로 for문 실행
        for (int i = _list.Count - 1; i > 0; i--)
        {
            // 맨 처음(0번 인덱스)부터 i번째까지 중 랜덤 인덱스 추출
            int rnd = UnityEngine.Random.Range(0, i);

            // 현재 i번째 있는 인덱스를 temp에 저장
            T temp = _list[i];
            // 추출한 랜덤 인덱스에 있는 원소를 i번째에 저장
            _list[i] = _list[rnd];
            // temp에 있던 기존 i번째 원소를 랜덤 인덱스 번째 자리에 저장
            _list[rnd] = temp;
        }

        // Shuffle된 인덱스 반환
        return _list;
    }

    // 순찰 및 추적을 정지시키는 함수
    public void Stop() {
        // 정지
        agent.isStopped = true;

        // 바로 정지하기 위해 속도를 0으로 설정
        agent.velocity = Vector3.zero;

        // 순찰 모드 해제
        patrolling = false;
    }
}
