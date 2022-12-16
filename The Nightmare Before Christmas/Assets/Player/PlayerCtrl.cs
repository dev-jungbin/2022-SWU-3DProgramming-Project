 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // 이동 속도 변수
    public float moveSpeed = 6.0f;

    // 회전 속도 변수
    public float rotSpeed = 360.0f;

    // Animation 컴포넌트를 지정하기 위한 변수
    Animation anim;

    // 점프하는 힘을 저장하기 위한 변수
    public float power = 10;
    // 강체를 저장할 변수
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Animation 컴포넌트를 변수에 할당
        anim = GetComponent<Animation>();

        // 강체 컴포넌트를 변수에 할당
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // 상하 입력
        float v = Input.GetAxis("Vertical"); // 좌우 입력
        float r = Input.GetAxis("Mouse X"); // 마우스 수평 입력

        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(이동 방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표)
        transform.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        // Vector3.up 축을 기준으로 rotSpeed만큼의 속도로 회전
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        // 키보드 입력값을 기준으로 동작할 애니메이션 수행
        if (v >= 0.1f || v <= -0.1f) // 전진하거나 후진할 경우
		{
            anim.CrossFade("Walking", 0.3f); // 달리기 애니메이션
		} else
		{
            anim.CrossFade("Idle (1)", 0.3f); // 정지 시 Idle 애니메이션
		}

        // 정지 상태이고, 스페이스바(점프)를 누르면
        if (Input.GetButtonDown("Jump"))
            rb.AddForce(0, power, 0, ForceMode.Impulse); // Y축 방향으로 power의 힘을 가함
    }
}