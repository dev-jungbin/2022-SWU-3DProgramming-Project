using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveZWheel : MonoBehaviour
{
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 휠 스크롤(-0.1 ~ 0.1)
        float mw = Input.GetAxis("Mouse ScrollWheel");
        float len = mw * speed; // 이동 거리

        transform.Translate(0, 0, len); // Z축으로 len 만큼 이동
    }
}
