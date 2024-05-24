using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum QuestID
{
    CAR_GAME = 0, FARM_GAME = 10
    // NPC = 0, Coin, Rabbit, Flower, Car, Path
}

public class QuestManager : MonoBehaviour
{
    private int questId;

    public float playTime = 0.0f;

    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject radish;
    [SerializeField] private GameObject[] Items;

    private static QuestManager instance = null;
    private GameObject NPC, Portal;
    private MovePanel movePanelScript;

    public Vector3 playerPos, carPos, carRot;

    private bool rabbitSpawn = false;
    public bool questClear = false;
    public bool throwItems = false;

    void Awake() // 초기화
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

    void Start()
    {
        // questId = 0;
        movePanelScript = GameObject.Find("Canvas").transform.Find("MainPanel").GetComponent<MovePanel>();
    }

    public static QuestManager Instance
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

    public void SetQuestID(int id)
    {
        this.questId = id;
    }
    public int GetQuestID() // 현재 퀘스트의 ID를 반환
    {
        return questId;
    }

    private void SetPlayTime() // 플레이 시간을 측정
    {
        playTime += Time.deltaTime;
    }

    private void SetPosObject()
    {
        if (!rabbitSpawn && GetQuestID() == (int)QuestID.FARM_GAME)
        {
            Instantiate(radish, new Vector3(Random.Range(-15f, 100f), 4, Random.Range(-65f, 65f)), Quaternion.identity);
            rabbitSpawn = true;
        }

        if (questClear)
        {
            if (!throwItems)
                StartCoroutine(CreateItem());
        }
    }

    void Update()
    {
        SetPlayTime();
        SetPosObject();

        if (NPC == null && GetQuestID() == (int)QuestID.FARM_GAME)
        {
            NPC = GameObject.FindGameObjectWithTag("NPC");
        }

        if (SceneManager.GetActiveScene().name.Equals("Gameover") ||
            SceneManager.GetActiveScene().name.Equals("Gameclear"))
            this.gameObject.SetActive(false);
    }

    IEnumerator CreateItem()
    {
        movePanelScript.currentTime = 0.0f;
        throwItems = true;

        for (int i = 0; i < 5; i++)
        {
            Vector3 randItemPos = new Vector3(NPC.transform.position.x + Random.Range(-5f, 5f),
                                              4,
                                              NPC.transform.position.z + Random.Range(-1f, -4f));

            int randIdx = Random.Range(0, 5);

            Instantiate(Items[randIdx], randItemPos, Quaternion.identity);
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.itemDropSound);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
