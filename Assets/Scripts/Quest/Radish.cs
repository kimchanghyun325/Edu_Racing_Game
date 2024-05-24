using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : MonoBehaviour
{
    float rotSpeed = 30f;

    private QuestRabbit rabbitScript;
    private float timer = 0.0f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.0f)
            rabbitScript = GameObject.FindGameObjectWithTag("Rabbit").GetComponent<QuestRabbit>();
            
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime); 
    }

    // 플레이어의 무 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.getRadishSound);
            rabbitScript.getRadish = true;
            Destroy(gameObject);
        }
    }

    // 무 소환 위치 결정
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            //gameObject.transform.position = new Vector3(Random.Range(-10, 10), 1.5f, Random.Range(-10, 10));
            this.gameObject.transform.position = new Vector3(Random.Range(-15f, 100f), 4, Random.Range(-65f, 65f));
        }
    }
}
