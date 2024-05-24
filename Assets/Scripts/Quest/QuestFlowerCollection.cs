using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFlowerCollection : MonoBehaviour
{
    GameObject player;
    float gatherTime = 0.0f;
    public static bool gatherArea = false;
    public static bool getFlower = false;
    public static bool diggingFlower = false;

    GameObject flower;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // 채집
        if (diggingFlower)
        {
            gatherTime += Time.deltaTime;
            if (gatherTime > 1.5f)
            {
                diggingFlower = false;
                gatherArea = false;
                getFlower = true;
                Destroy(gameObject);
            }
        }
    }

    // 채집가능한 장소 여부 확인 및
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            gatherArea = true;
        }

        if (other.gameObject.tag == "Obstacle")
        {
            //transform.position = new Vector3(Random.Range(-15, 15), 1, Random.Range(-15, 15));
            this.gameObject.transform.position = new Vector3(Random.Range(-110, 105), 2, Random.Range(-68, 100));
        }
    }
}
