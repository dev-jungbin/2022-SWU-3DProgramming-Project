using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    // 총알 프리팹
    public GameObject bullet;

    // 탄피 추츨 파티클
    public ParticleSystem cartridge;

    // 총알 발사좌표
    public Transform firePos;

    // 오디오 클립을 저장할 변수
    public AudioClip fireSound;

    // 총구 화염 파티클
    ParticleSystem muzzleFlash;

    // AudioSource 컴포넌트 저장할 변수
    AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        // FirePos 하위 컴포넌트 추출
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();

        // AudioSource 컴포넌트 추출
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 Fire 함수 호출
        if (Input.GetMouseButtonDown(0))
		{
            Fire();
		}
    }

    void Fire()
	{
        // Bullet 프리팹 동적 생성
        Instantiate(bullet, firePos.position, firePos.rotation);

        // 파티클 실행
        cartridge.Play();

        // 총구 화염 파티클 실행
        muzzleFlash.Play();

        // 사운드 발생
        _audio.PlayOneShot(fireSound, 0.5f);
	}
}
