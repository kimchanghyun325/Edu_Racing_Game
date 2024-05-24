using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCoin : MonoBehaviour
{
    float rotSpeed = 40f;
    // Update is called once per frame
    void Update()
    {
        RotCoin();
    }

    // 플레이어 코인 획득
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // QuestManager.Instance.coinCnt++;
            Destroy(this.gameObject);
        }
    }

    // 코인 소환 위치 결정
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            //this.gameObject.transform.position = new Vector3(Random.Range(-15, 15), 2, Random.Range(-15, 15));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 4, Random.Range(-68, 100));
        }
    }
    void RotCoin()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
