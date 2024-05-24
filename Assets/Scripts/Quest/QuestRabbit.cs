using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QuestRabbit : MonoBehaviour
{
    private float waitTime = 0.0f; // 
    private float lookAtObsTime = 0.0f;
    //float moveSpeed = 6f;  
    //float rotateSpeed = 10.0f; 
    private bool waitForSecond = false; // 대기 시간
    private bool rabbitFollowMe = false; // 토끼가 따라오는지 여부

    public bool isSafePos = false; // 안전한 위치에서 소환 여부
    public bool getRadish = false; // 채소 획득 여부
    
    private Animator rabbitAnim;

    [SerializeField] private NavMeshAgent rabbitAgent;
    [SerializeField] private GameObject radishPrefab;
    private GameObject target; 

    private FindNPC NPCScript;
    public static Vector3 randPos;
    Vector3  rabbitPos;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] rabbitStep;
    private float soundInteval = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        rabbitAnim = GetComponent<Animator>();
        rabbitAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        SpawnRadish();
        Settings();
    }

    void Start()
    {
        NPCScript = GameObject.FindGameObjectWithTag("NPC").GetComponent<FindNPC>();
        audioSource = this.GetComponent<AudioSource>();
    }

    void SpawnRadish()
    {
        Instantiate(radishPrefab, new Vector3(Random.Range(-15f, 100f), 4, Random.Range(-65f, 65f)), Quaternion.identity);
        //Instantiate(radishPrefab, new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10)), Quaternion.identity);
    }

    void Settings() // 초기화
    {
        lookAtObsTime = 0.0f;
        waitTime = 0.0f;
        //randPos = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10)); 
        randPos = new Vector3(Random.Range(-55, 80), transform.position.y, Random.Range(-70, 110));
        waitForSecond = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitForSecond)
        {
            soundInteval += Time.deltaTime;
            
            if (soundInteval >= 0.9f)
            {
                audioSource.PlayOneShot(rabbitStep[Random.Range(0, 1)]);
                soundInteval = 0.0f;
            }
                
            Movement();
            if (!rabbitFollowMe) CheckGoalPos();
        }

        /* 토끼가 장애물을 바라보는 시간이 1.5초 이상이거나 타겟위치에 도착하면. */
        if (!rabbitFollowMe)
        {
            if (Mathf.Abs(transform.position.x - randPos.x) <= 2f
                && Mathf.Abs(transform.position.z - randPos.z) <= 2f
                || lookAtObsTime > 1.5f)
            {
                Wait();
                if (waitTime > 1.0f) Settings();
            }
        }

        // 무를 획득하면 일정 범위에서 토끼를 유인
        if (getRadish) InvitationRabbit();
    }

    /* 토끼가 멈춰선다. */
    void Wait()
    {
        waitTime += Time.deltaTime;
        waitForSecond = true;
        rabbitAnim.SetBool("isRun", false);
    }

    /* 토끼가 움직인다. */
    void Movement()
    {
        rabbitPos = transform.position;
        rabbitAnim.SetBool("isRun", true);

        //this.transform.LookAt(randPos);

        if (rabbitFollowMe) rabbitAgent.SetDestination(target.transform.position);
        else if (!waitForSecond) rabbitAgent.SetDestination(randPos);

    }

    /* 토끼가 바라보는 방향에 장애물이 존재하는지  */
    void CheckGoalPos()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(rabbitPos, fwd, out hit, 2) != false
                            && hit.collider.gameObject.tag == "Obstacle")
        {
            lookAtObsTime += Time.deltaTime;
        }
    }

    /* 일정 범위 내에 채소가 있으면 토끼가 따라온다. */
    void InvitationRabbit()
    {
        Vector3 vDir = this.transform.position - target.transform.position;

        if (Mathf.Abs(vDir.x) < 10f && Mathf.Abs(vDir.z) < 10f)
        {
            rabbitFollowMe = true;
        }
        else rabbitFollowMe = false;
    }

    void LateUpdate()
    {
        isSafePos = true;
    }

    /* 토끼를 NPC에게 갖다주기 */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "NPC")
        {
            NPCScript.NPCGetRabbit = true;
            getRadish = false; 
            gameObject.SetActive(false);
        } 
    }

    private void OnCollisionStay(Collision col)
    {
        /* 토끼의 소환 위치 변경 */
        if (col.gameObject.tag == "Obstacle" && !isSafePos)
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
            this.gameObject.transform.position = new Vector3(Random.Range(-15f, 100f), 4, Random.Range(-65f, 65f));
        }
        else isSafePos = true;
    }
}
