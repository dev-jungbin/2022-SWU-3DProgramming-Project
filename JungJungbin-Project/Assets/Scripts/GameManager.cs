using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 가짜 산타 캐릭터가 출현할 위치를 담을 배열
    public Transform[] points;
    // 가짜 산타 캐릭터 프리팹을 저장할 변수
    public GameObject fakeSanta;
    // 가짜 산타 캐릭터를 생성할 주기
    public float createTime = 0.5f;
    // 가짜 산타 캐릭터의 최대 생성 개수
    public int maxFakeSanta = 15;
    // 게임 오버 여부를 판단할 변수
    public bool isGameOver = false;
    // 게임 클리어 여부를 판단할 변수
    public bool isGameClear = false;

    void Start()
    {
        // 하이어라키 뷰의 SpawnPointGroup을 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        CreateFakeSanta();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // fake santa 캐릭터를 생성하는 코루틴 함수
    void CreateFakeSanta() {

        // fake santa 캐릭터가 출현할 SpawnPointGroup의 Point 개수만큼 fake santa 캐릭터 생성 반복
        for (int idx = 0; idx < points.Length; idx++){
            // fake santa 캐릭터  생성
            Instantiate(fakeSanta, points[idx].position, points[idx].rotation);
        }

    }
}
