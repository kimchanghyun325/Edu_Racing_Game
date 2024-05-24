using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathManager : MonoBehaviour
{
    public GameObject[] path;
    public int pNum = 0;
    public int stage = 0;

    private static PathManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static PathManager Instance
    { 
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetPathNum(int pNum)
    {
        this.pNum = pNum;
    }
    
    public int GetPathNum() // 지나친 패스의 개수
    {
        return pNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Gameover") ||
            SceneManager.GetActiveScene().name.Equals("Gameclear"))
            this.gameObject.SetActive(false);

    }

    public void NextPath()
    {
        pNum += 2;

        for (int i = pNum - 2; i < pNum; i++)
        {
            path[i].SetActive(false);
        }

        stage = pNum / 2;
    }

    public void PathSetting()
    {
        for (int i = 0; i < pNum; i++)
        {
            path[i].SetActive(false);
        }
    }

    // 통과한 경로가 정답인지 아닌지 확인
    public void PathCheck(string ans)
    {
        if (ans.Equals("Correct"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.correctSound);
            NextPath();
        }
        else
        {
            SceneManager.LoadScene("Gameover");
        }
    }
}
