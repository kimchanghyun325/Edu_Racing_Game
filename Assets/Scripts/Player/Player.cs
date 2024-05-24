using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float moveSpeed = 10.0f;
    public float rotateSpeed = 10.0f;
    public const float MAX_SPEED = 10.0f;
    public Animator playerAnim;
    public static bool isRun = false;
    public static bool isRiding = false;
    public bool inPortal = false;

    float h, v;
    bool questPanelActive = false;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject radishOnHand;
    private MovePanel movePanelScript;
    private CanvasManager canvasScript;
    private QuestRabbit rabbitScript;
    private FindNPC NPCScript;

    private bool radishSoundPlay = false;
    // Start is called before the first frame update
    void Start()
    {
        canvasScript = GameObject.Find("PanelManager").GetComponent<CanvasManager>();
        movePanelScript = GameObject.Find("Canvas").transform.Find("MainPanel").GetComponent<MovePanel>();
    }

    // Update is called once per frame
    void Update()
    {

        PressQKey(); 
        PressEKey();
        if (SceneManager.GetActiveScene().name.Equals("City")) 
        {
            CarInteraction();
        }
        else if (SceneManager.GetActiveScene().name.Equals("Forest"))
        {
            if (NPCScript == null) NPCScript = GameObject.FindGameObjectWithTag("NPC").GetComponent<FindNPC>();
            else 
            {
                if (!NPCScript.NPCGetRabbit && rabbitScript == null) rabbitScript = GameObject.Find("Rabbit").GetComponent<QuestRabbit>();
                if(rabbitScript != null)  GetRadish();
            }

        }
        SoundManager.Instance.FootStep();

        if (this.gameObject.transform.position.y <= -7.5f)
            SceneManager.LoadScene("Gameover");
    }

    void FixedUpdate()
    {
        Movement();
    }

    /* 플레이어 이동 */
    private void Movement()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            isRun = true;
            playerAnim.SetBool("isRun", true);
        }
        else
        {
            isRun = false;
            playerAnim.SetBool("isRun", false);
        }

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        }
    }
                                                                                                                                              
    void PressQKey() 
    {
        if (Input.GetKeyDown(KeyCode.Q) && !questPanelActive)
        {
            canvasScript.explainPanel.SetActive(true);
            questPanelActive = true;
            canvasScript.mainPanel.SetActive(false);
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.explainSound);
        } 
        else if (Input.GetKeyDown(KeyCode.Q) && questPanelActive) 
        {
            canvasScript.explainPanel.SetActive(false);
            questPanelActive = false;
            
            movePanelScript.currentTime = 0.0f;
            canvasScript.mainPanel.SetActive(true);
            SoundManager.Instance.PlayPanelSound();
        }
    }

    void PressEKey()
    {
        if (!isRiding && Input.GetKeyDown(KeyCode.E) && inPortal && SceneManager.GetActiveScene().name.Equals("City"))
        {
            CanvasManager.Instance.pNum = PathManager.Instance.GetPathNum();
            CanvasManager.Instance.stage = PathManager.Instance.stage;

            canvasScript.sceneChange = true;
            SceneManager.LoadScene("Forest");
            SoundManager.Instance.audioSource.clip = SoundManager.Instance.background[1];
            SoundManager.Instance.audioSource.Play();
            QuestManager.Instance.SetQuestID(10);
            movePanelScript.currentTime = 0.0f;
            SoundManager.Instance.PlayPanelSound();

            QuestManager.Instance.playerPos = this.transform.position;
            QuestManager.Instance.carPos = carPrefab.transform.position;
            QuestManager.Instance.carRot = carPrefab.transform.rotation.eulerAngles;
            SoundManager.Instance.stepInteval = 0.0f;

        }

        if (Input.GetKeyDown(KeyCode.E) && inPortal && SceneManager.GetActiveScene().name.Equals("Forest"))
        {   
            canvasScript.sceneChange = true;
            QuestManager.Instance.questClear = false;
            QuestManager.Instance.throwItems = false;

            SceneManager.LoadScene("City");
            
            SoundManager.Instance.audioSource.clip = SoundManager.Instance.background[0];
            SoundManager.Instance.audioSource.Play();
            QuestManager.Instance.SetQuestID(0);
            movePanelScript.currentTime = 0.0f;
            SoundManager.Instance.PlayPanelSound();
        }
    }

    // void GameOver()
    // {
    //     if (this.gameObject.transform.position.y <= -8.0f) SceneManager.LoadScene("Gameover");
    // }

    // // 채집하기
    // void FlowerInteraction()
    // {
    //     if (QuestFlowerCollection.gatherArea && Input.GetMouseButtonDown(0) && !isRun)
    //     {
    //         playerAnim.SetTrigger("isGather");
    //         QuestFlowerCollection.diggingFlower = true;
    //     }
    // }

    void GetRadish()
    {
        if (rabbitScript.getRadish) 
        {            
            radishOnHand.SetActive(true);

        }
        else radishOnHand.SetActive(false);
    }

    // // 차 모든 상호작용
    void CarInteraction()
    {
        float xDir = this.transform.position.x - carPrefab.transform.position.x;
        float zDir = this.transform.position.z - carPrefab.transform.position.z;
        if (Mathf.Abs(xDir) < 5f && Mathf.Abs(zDir) < 5f && !isRiding && !CarController.coolTimeStart)
        {
            canvasScript.LeftSidePanelOn();
            if (!isRiding && Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.car[0]);
                int currentHP = Status.HP;
                this.gameObject.SetActive(false);
                this.gameObject.transform.parent = carPrefab.transform;
                this.gameObject.transform.parent.GetComponent<Status>().enabled = true;
                Status.HP = currentHP;
                isRiding = true;
                movePanelScript.currentTime = 0.0f;
                SoundManager.Instance.PlayPanelSound();
            }
        }
        else
        {
            canvasScript.GetLeftSidePanel().SetActive(false);
            canvasScript.GetLeftSidePanel().transform.position = canvasScript.GetLeftSidePanel().GetComponent<MovePanel>().startPosition.position;
            canvasScript.GetLeftSidePanel().GetComponent<MovePanel>().currentTime = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Portal") && other.name.Equals("Portal(Farm)"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.portalSound);
            movePanelScript.currentTime = 0.0f;
            inPortal = true;
        }

        if (other.CompareTag("Portal") && other.name.Equals("Portal(City)"))
        {
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.portalSound);
            movePanelScript.currentTime = 0.0f;
            inPortal = true;
        }

        if (other.CompareTag("Goal") && !isRiding && PathManager.Instance.stage >= 5)
        {
            SceneManager.LoadScene("Gameclear");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Portal") && other.name.Equals("Portal(Farm)"))
        {
            SoundManager.Instance.PlayPanelSound();
            movePanelScript.currentTime = 0.0f;
            inPortal = false;
        }

        // if (other.CompareTag("Portal") && other.name.Equals("Portal(City)"))
        // {
        //     movePanelScript.currentTime = 0.0f;
        //     inPortal = false;
        // }
    }
}
