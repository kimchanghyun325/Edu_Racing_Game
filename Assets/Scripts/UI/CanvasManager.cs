using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject leftSidePanel, rightSidePanel;
    public GameObject mainPanel, explainPanel;
    [SerializeField] private Text[] mainText;
    [SerializeField] private Text leftSideText, playTimeText;

    private static CanvasManager instance;
    private Player playerScript;
    private GameObject carPrefab;
    private GameObject canvas;

    private int minute;
    public bool sceneChange = false;
    public int pNum, stage;

    private string[] questionData = {
        "자바에서 키보드로 입력받기 위해 불려지는 클래스는?",
        "C에서 srtcmp로 문자열을 비교하여 같을 때 출력되는 값은?",
        "후입선출의 구조를 가지는 자료구조는?",
        "파이썬에서 함수를 정의하는데 사용되는 키워드는?",
        "다음 중 정렬 알고리즘이 아닌 것은?",
        "앞에 보이는 포탈로 들어가세요!"
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static CanvasManager Instance
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
        leftSideText.alignment = TextAnchor.MiddleCenter;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SoundManager.Instance.PlayPanelSound();
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Gameover") ||
            SceneManager.GetActiveScene().name.Equals("Gameclear"))
        {
            canvas.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {
            PlayTimeText();
            MainText();
        }


        if (SceneManager.GetActiveScene().name.Equals("City")) SideText();

        TimeSettings();
        TextSettings();

        if ((SceneManager.GetActiveScene().name.Equals("Forest") && sceneChange) ||
            SceneManager.GetActiveScene().name.Equals("City") && sceneChange)
        {
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


            if (SceneManager.GetActiveScene().name.Equals("City") && carPrefab == null)
            {
                carPrefab = GameObject.FindGameObjectWithTag("Car").gameObject;
                carPrefab.transform.position = QuestManager.Instance.carPos;
                carPrefab.transform.rotation = Quaternion.Euler(QuestManager.Instance.carRot);
                playerScript.gameObject.transform.position = QuestManager.Instance.playerPos;

                PathManager.Instance.stage = CanvasManager.Instance.stage;
                PathManager.Instance.SetPathNum(CanvasManager.Instance.pNum);
                PathManager.Instance.PathSetting();
            }

            sceneChange = false;
        }
    }

    public void RightSidePanelOff()
    {
        rightSidePanel.SetActive(false);
    }

    public void MainPanelOff()
    {
        mainPanel.SetActive(false);
    }

    public void LeftSidePanelOn()
    {
        leftSidePanel.SetActive(true);
    }

    public GameObject GetMainPanel()
    {
        return mainPanel;
    }

    public GameObject GetLeftSidePanel()
    {
        return leftSidePanel;
    }

    public Text LeftGetSideText()
    {
        return leftSideText;
    }

    void MainText()
    {
        if (!Player.isRiding && SceneManager.GetActiveScene().name == "City")
        {
            if (playerScript.inPortal)
            {
                mainText[0].text = "농장으로 이동하기(E키)";

            }
            else
            {
                mainText[0].text = "설명 창 보기(Q키)";
            }

            // mainText[0].alignment = TextAnchor.MiddleCenter;
        }

        else
        {
            switch (QuestManager.Instance.GetQuestID())
            {
                case (int)QuestID.CAR_GAME:
                    if (Player.isRiding && PathManager.Instance.stage <= 5)
                    {
                        
                        mainText[0].text = questionData[PathManager.Instance.stage];
                    }
                    break;

                case (int)QuestID.FARM_GAME:
                    if (QuestManager.Instance.questClear)
                    {
                        if (playerScript.inPortal)
                        {
                            mainText[0].text = "도시로 돌아가기(E키)";
                        }
                    }
                    else
                    {
                        mainText[0].text = "떨어진 무로 토끼를 유인해서 갖다줘!";
                    }
                    break;
            }
        }
    }

    void SideText()
    {
        if (Player.isRiding)
            leftSideText.text = "F키 눌러서 하차하기";
        else if (!Player.isRiding)
            leftSideText.text = "F키 눌러서 승차하기";
    }

    void PlayTimeText()
    {
        playTimeText.text = "플레이 시간\n" + minute + "분 " + (int)QuestManager.Instance.playTime + "초";
        if ((int)QuestManager.Instance.playTime / 60 >= 1)
        {
            minute++;
            QuestManager.Instance.playTime = 0.0f;
        }
    }

    void TimeSettings()
    {
        // if (QuestManager.Instance.questClear || Player.isRiding) GetMainPanel().GetComponent<MovePanel>().currentTime = 0.0f;
    }

    void TextSettings()
    {
        switch (QuestManager.Instance.GetQuestID())
        {
            case (int)QuestID.CAR_GAME:
                if (Player.isRiding)
                {
                    
                    mainPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 80);
                }
                else
                {
                    mainPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 80);
                }

                break;
        }
    }
}
