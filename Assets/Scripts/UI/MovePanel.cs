using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MovePanel : MonoBehaviour
{
    public Transform startPosition, endPosition;

    public float currentTime = 0f;
    float lerpTime = 1.0f; // 판넬 내려오는 시간

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MoveImage();
        /*
        if (QuestManager.Instance.GetQuestID() == 5)
        {
            gameObject.transform.localScale = new Vector3(2, 2);
        }*/
    }

    void MoveImage()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }
        // 스무스 스텝 계산
        float t = currentTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
    }
}

