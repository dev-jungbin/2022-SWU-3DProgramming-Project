using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 총알 발사 속도
    public float speed = 1000.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody 컴포넌트를 얻어서 speed의 속도로, forward 방향으로 힘을 가함
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
