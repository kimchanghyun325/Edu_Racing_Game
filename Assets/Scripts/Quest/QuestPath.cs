using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPath : MonoBehaviour
{
    GameObject player;
    public string answer;
    public int stage;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Car");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && Player.isRiding && this.stage == PathManager.Instance.stage)
        {
            PathManager.Instance.PathCheck(answer);
        }
    }
}
