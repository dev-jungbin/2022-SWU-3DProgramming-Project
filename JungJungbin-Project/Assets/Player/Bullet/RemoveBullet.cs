using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // 스파크 프리팹 저장할 변수
    public GameObject sparkEffect;

    // 충돌 시작 시 발생하는 이벤트
    void OnCollisionEnter(Collision coll) {
        // 충돌한 게임오브젝트의 태그값 비교
        if (coll.collider.tag == "BULLET")
        {
            // 스파크 효과 함수 호출
            ShowEffect(coll);
            // 충돌한 게임오브젝트 삭제
            Destroy(coll.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowEffect(Collision coll) {
        // 충돌 지점 정보 추출
        ContactPoint contact = coll.contacts[0];

        // 법선 벡터가 이루는 회전각도 추출
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, contact.normal);
        
        // 스파크 효과 생성
        GameObject spark = Instantiate(sparkEffect, contact.point - (contact.normal * 0.05f), rot);

        // 스파크 효과의 부모를 충돌한 물체로 설정
        spark.transform.SetParent(this.transform);
    }
}
