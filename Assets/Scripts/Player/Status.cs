using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    public static int MAX_HP = 100;
    public static int HP = 100;
    public static bool useItem = false;

    private float decreaseHPTime = 0.0f;
    private float decreaseTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        decreaseHPTime += Time.deltaTime;
        calculateHP();
        Death();
    }

    void calculateHP() // 1초마다 1의 체력을 소모
    {
        if (!useItem) decreaseTime = 1.5f;
        else decreaseTime = 1f;

        if (decreaseHPTime > decreaseTime)
        {
            HP -= 1;
            decreaseHPTime = .0f;
        }
    }

    void Death()
    {
        if (HP <= 0)
        {
            SceneManager.LoadScene("Gameover");
        }
    }
}
