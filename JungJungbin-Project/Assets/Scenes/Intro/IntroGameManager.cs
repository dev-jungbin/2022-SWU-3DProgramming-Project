using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroGameManager : MonoBehaviour
{
    public GameObject startButton;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart() {
        // Game 씬 로드
        SceneManager.LoadScene("Game");

    }
}
