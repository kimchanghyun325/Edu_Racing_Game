using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject cinemachine;

    public float offsetX = 0.0f;
    public float offsetY = 10.0f;
    public float offsetZ = -10.0f;

    public float CameraSpeed = 10.0f;
    bool targetisCar = false;
    Vector3 TargetPos;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        CheckTarget();


        TargetPos = new Vector3(
        Target.transform.position.x + offsetX,
        Target.transform.position.y + offsetY,
        Target.transform.position.z + offsetZ
        );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }

    // 현재 카메라의 타겟이 변경되었는지 확인하는 메소드
    void CheckTarget()
    {
        if (Player.isRiding && !targetisCar)
        {
            targetisCar = true;
            cinemachine.SetActive(true);
            Target = GameObject.FindGameObjectWithTag("Car");
        }
        
        if (!Player.isRiding && targetisCar)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            targetisCar = false;
            cinemachine.SetActive(false);
            Target = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
