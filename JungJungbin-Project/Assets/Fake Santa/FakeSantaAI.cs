using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakeSantaAI : MonoBehaviour
{
    // Fake Santa의 상태를 표현하기 위한 열거형 변수 정의
    public enum State {
        WALKING,
        RUNNING,
        ZOMBIE,
        LOOKAROUND,
        KNEELINGDOWN,
        DIE
    }

    // 상태를 저장할 변수
    public State state = State.WALKING;

    // 사망 여부를 판단할 변수
    public bool isDie = false;

    // 주인공 위치 저장 변수
    Transform playerTr;

    // 산타의 위치 저장할 변수
    Transform santaTr;

    // 코루틴에서 사용할 지연시간 변수
    WaitForSeconds ws;

    // Fake Mode State를 담은 List
    List<State> fakeModeStateList = new List<State>();

    // 이동을 제어하는 MoveAgent 클래스를 저장할 변수
    FakeSantaMoveAgent moveAgent;

    // Animator 컴포넌트를 저장할 변수
    Animator animator;

    void Awake() {

        // Fake Mode State List를 설정하는 함수 호출
        SetFakeModeStateList();

        // 주인공 게임오브젝트 추출
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 주인공의 Trnasform 컴포넌트 추출
        if (player != null) {
            playerTr = player.transform;
        }

        // 산타 캐릭터의 Transform 컴포넌트 추출
        santaTr = GetComponent<Transform>();

        // 이동을 제어하는 FakeSantaMoveAgent 클래스 추출
        moveAgent = GetComponent<FakeSantaMoveAgent>();

        // Animator 컴포넌트 추출
        animator = GetComponent<Animator>();

        // 코루틴 지연시간 생성
        ws = new WaitForSeconds(3f);
    }

    void OnEnable() {
        // CheckState 코루틴 함수 실행
        StartCoroutine(CheckState());

        // Action 코루틴 함수 실행
        StartCoroutine(Action());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 상태에 따라 산타 캐릭터의 행동을 처리하는 코루틴 함수
    IEnumerator Action() {
        // 캐릭터가 살아 있는 경우만 실행
        while (!isDie) {
            yield return ws;

            switch (state) // 상태에 따라 분기처리
            {
                case State.WALKING:
                    // 순찰 움직임 실행
                    moveAgent.SetPatrolling(true);

                    // 순찰 모드 활성화
                    animator.SetBool("isWalking", true);

                    // 나머지 모드 비활성화
                    animator.SetBool("isZombie", false);
                    animator.SetBool("isLookAround", false);
                    animator.SetBool("isKneelingDown", false);
                    animator.SetBool("isRunning", false);
                    break;

                case State.RUNNING:
                    // 주인공의 위치를 넘겨 추적 움직임 실행
                    moveAgent.SetTraceTargert(playerTr.position);

                    // 추적 달리기 모드 활성화
                    animator.SetBool("isRunning", true);

                    // 나머지 모드 비활성화
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isZombie", false);
                    animator.SetBool("isLookAround", false);
                    animator.SetBool("isKneelingDown", false);
                    break;
                case State.ZOMBIE:
                    // 순찰 움직임 실행
                    moveAgent.SetPatrolling(true);

                    // 좀비 모드 활성화
                    animator.SetBool("isZombie", true);

                    // 나머지 모드 비활성화
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isLookAround", false);
                    animator.SetBool("isKneelingDown", false);
                    animator.SetBool("isRunning", false);
                    break;
                case State.LOOKAROUND:
                    // 순찰 움직임 멈춤
                    moveAgent.Stop();

                    // 좌우 둘러보기 모드 활성화
                    animator.SetBool("isLookAround", true);

                    // 나머지 모드 비활성화
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isZombie", false);
                    animator.SetBool("isKneelingDown", false);
                    animator.SetBool("isRunning", false);
                    break;
                case State.KNEELINGDOWN:
                    // 순찰 움직임 멈춤
                    moveAgent.Stop();
                    // 앉기 모드 활성화
                    animator.SetBool("isKneelingDown", true);

                    // 나머지 모드 비활성화
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isZombie", false);
                    animator.SetBool("isLookAround", false);
                    animator.SetBool("isRunning", false);
                    break;
                case State.DIE:
                    // 사망 처리
                    isDie = true;
                    // killCount 횟수 증가 함수 호출
                    GameObject.Find("GameManager").GetComponent<GameManager>().AddKillCount();
                    // 순찰 정지
                    moveAgent.Stop();
                    // 사망 애니메이션 실행
                    animator.SetTrigger("isDie");
                    // Collider 무효화
                    GetComponent<CapsuleCollider>().enabled = false;
                    
                    break;
            }
        }
    }

    // 사망 처리
    void OnCollisionEnter(Collision other) {
        // 총알이 들어왔을 경우
        if (other.collider.tag == "BULLET") {
            // 총알 삭제
            Destroy(other.gameObject);
            // 캐릭터의 상태를 DIE로 변경
            state = State.DIE;
        }
    }

    // 산타 캐릭터의 상태를 검사하는 코루틴 함수
    IEnumerator CheckState() {
        // 산타 캐릭터가 사망하기 전까지 도는 무한루프
        while (!isDie) {
            // 상태가 사망이면 코루틴 함수 종료
            if (state == State.DIE) yield break;
            // 주인공과 적 캐릭터 간 거리 계산
            float dist = Vector3.Distance(playerTr.position, santaTr.position);

            // FAKE MODE (좀비, 달리기, 둘러보기, 무릎 꿇기)일 때, 다음 state를 walking으로 설정
            if (state == State.ZOMBIE || state == State.RUNNING || state == State.LOOKAROUND || state == State.KNEELINGDOWN) {
                state = State.WALKING;
            } else { // FAKE MODE가 아닐 때, 다음 state를 Fake Mode 중 랜덤하게 설정
                // 다음 state를 Fake Mode에서 랜덤하게 추출
                State nextState = GetShuffleList(fakeModeStateList)[0];

                // 추출한 다음 state 지정
                state = nextState;
            }
            // 3초 대기하는 동안 제어권 양보
            yield return ws;
        }
    }

    // Fake Mode State List를 설정하는 함수
    public void SetFakeModeStateList() {

        // State에서 Fake Mode인 State만 따로 fakeModeStateList에 추가하는 코드(fake mode 4개)
        fakeModeStateList.Add(State.ZOMBIE);
        fakeModeStateList.Add(State.RUNNING);
        fakeModeStateList.Add(State.LOOKAROUND);
        fakeModeStateList.Add(State.KNEELINGDOWN);
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
}
