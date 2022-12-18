using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 가짜 산타 캐릭터가 출현할 위치를 담을 배열
    public Transform[] points;
    // 가짜 산타 캐릭터 프리팹을 저장할 변수
    public GameObject fakeSanta;
    // 게임 오버 여부를 판단할 변수
    public bool isGameOver = false;
    // 게임 클리어 여부를 판단할 변수
    public bool isGameClear = false;
    // Kill Count 표시
    public Text killText;
    // 적을 죽인 횟수
    public float killCount = 0;
    // 초 변수 선언
    public float sec;
    // 분 변수 선언
    public int min;
    // 타이머 텍스트
    public Text timerText;

    // GameClear 텍스트 UI를 담을 변수
    public GameObject gameClearText;
    //GameOver 텍스트 UI를 담을 변수
    public GameObject gameOverText;


    void Start()
    {
        // 하이어라키 뷰의 SpawnPointGroup을 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        CreateFakeSanta();
    }

    // Update is called once per frame
    void Update()
    {
        // 게임이 실행 중일 경우
        if (!isGameOver && !isGameClear) {
            // 타이머 실행
            Timer();
        } else {
            // 시간 흐르는 속도를 0으로 두어 씬 정지
            Time.timeScale = 0;
        }
    }

    // fake santa 캐릭터를 생성하는 코루틴 함수
    void CreateFakeSanta() {

        // fake santa 캐릭터가 출현할 SpawnPointGroup의 Point 개수만큼 fake santa 캐릭터 생성 반복
        for (int idx = 0; idx < points.Length - 1; idx++){
            // fake santa 캐릭터  생성
            Instantiate(fakeSanta, points[idx].position, points[idx].rotation);
        }

    }

    public void AddKillCount() {
        // 가짜 산타를 죽인 횟수 증가
        ++killCount;
        // killText 변경
        killText.text = "남은 가짜 산타: " + (15 - killCount) + " 명";

        // Game Clear 처리: killCount == 15인 경우
        if (killCount >= 15) {
            // gameClear text ui를 활성화
            gameClearText.SetActive(true);
            // isGameClear 변수 true로 변경
            isGameClear = true;
        }
    }

    public void Timer() {
        // 초 받아오기
        sec += Time.deltaTime;
        // timerText에 time -> string으로 포매팅하여 text 설정
        timerText.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);

        // 만약 59초가 넘어갔을 경우
        if ((int)sec > 59) {
            // 초를 0으로 초기화
            sec = 0;
            // 분을 1 올리기
            min++;
        }
        
        // 만약 5분이 넘었을 경우
        if (min >= 5) {
            // Game over text UI 활성화
            gameOverText.SetActive(true);
            // isGameOver true로 변경
            isGameOver = true;
        }
    }

    // 진짜 산타가 총에 맞았을 경우 game over 처리
    public void RealSantaDie() {
        // Game over text UI 활성화
        gameOverText.SetActive(true);
        // isGameOver true로 변경
        isGameOver = true;
    }
}
