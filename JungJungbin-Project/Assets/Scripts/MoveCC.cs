using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCC : MonoBehaviour
{
    public float speed = 10; // 이동 속력
    CharacterController cc; // 캐릭터 컨트롤러 인스턴스
    Vector3 dir; // 이동 방향
    Vector3 dis; // 이동 거리

    // Start is called before the first frame update
    void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 얻기
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // 이동
        Jump(); // 점프

        cc.Move(dis * Time.deltaTime); // 초당 dis 만큼 이동
    }

    void Move()
    {
        dir.x = Input.GetAxis("Horizontal"); // 수평 이동치(-1.0~1.0)
        dir.z = Input.GetAxis("Vertical"); // 수직 이동치(-1.0 ~ 1.0)
        dir = transform.TransformDirection(dir); // 로컬에서 월드 좌표계로 변환

        // 모든 이동 방향에 대해 속도를 동일하게 조절
        if (dir.magnitude > 1)
            dir.Normalize();

        dis.x = dir.x * speed; // X축 이동 거리
        dis.z = dir.z * speed; // Z축 이동 거리
    }

    void Jump()
    {
        if (cc.isGrounded)
        { // 지면에 닿았다면
            if (Input.GetButton("Jump")) // 점프 버튼을 눌렀다면
                dis.y = 7; // Y축 이동 거리
            else
                dis.y = 0;
        }
        else
        {
            dis.y -= 9.8f * Time.deltaTime; // 중력 적용
        }
    }
}
