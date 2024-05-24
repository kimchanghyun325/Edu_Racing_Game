using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNPC : MonoBehaviour
{
    [SerializeField] private GameObject portal;
    private GameObject target; // 플레이어
    private QuestRabbit rabbitScript;
    public bool NPCGetRabbit = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (NPCGetRabbit) 
        {
            QuestManager.Instance.questClear = true;
            NPCGetRabbit = false;
            portal.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어와 상호작용이 가능한 위치인지?
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < 4f
            && Mathf.Abs(transform.position.z - target.transform.position.z) < 4f)
        {
            this.transform.LookAt(target.transform.position); // 플레이어와 가까운 위치에서 항상 플레이어를 바라본다.
        }


    }
}
