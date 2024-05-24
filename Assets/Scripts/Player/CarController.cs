using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 100;

    float time = 0.0f, cooltime = 0.0f;
    Vector3 pastPos, currentPos;
    public static float absXDir, absZDir;
    public static bool getOutCar = false;
    public static bool coolTimeStart = false;

    Rigidbody rigidbody;
    GameObject player;

    private MovePanel movePanelScript;
    private bool carDriving = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
        movePanelScript = GameObject.Find("Canvas").transform.Find("MainPanel").GetComponent<MovePanel>();
    }
    public void GetInput()
    {
        if (Player.isRiding)
        {

            m_horizontalInput = Input.GetAxis("Horizontal");
            m_verticalInput = Input.GetAxis("Vertical");

        }
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();

        // 초당 차의 거리를 계산하여 현재 플레이어가 내릴 수 있는지 판별
        CalDistance();
        GetOutOfTheCar();

        if (this.gameObject.transform.position.y <= -7.5f)
            SceneManager.LoadScene("Gameover");
    }

    // 차가 이동한 거리 초단위로 계산
    void CalDistance()
    {
        if (Player.isRiding)
        {
            time += Time.deltaTime;
            if (pastPos == null) pastPos = transform.position;
            if (((int)time) % 1 == 0)
            {
                if (currentPos == null) currentPos = transform.position;
                else
                {
                    pastPos = currentPos;
                    currentPos = transform.position;
                }
            }
        }

        if (pastPos != null && currentPos != null)
        {
            absXDir = Mathf.Abs(currentPos.x - pastPos.x);
            absZDir = Mathf.Abs(currentPos.z - pastPos.z);
        }

        if (absXDir < 0.05f || absZDir < 0.05f)
        {
            getOutCar = true;
        }
    }

    // 차에서 하차 + 쿨타임 3초
    void GetOutOfTheCar()
    {
        if (player.transform.parent != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.car[1]);
                int currentHP = Status.HP;
                this.GetComponent<Status>().enabled = false;
                player.gameObject.transform.parent = null;
                Player.isRiding = false;
                Status.HP = currentHP;

                player.SetActive(true);
                coolTimeStart = true;
                movePanelScript.currentTime = 0.0f;
                SoundManager.Instance.PlayPanelSound();
            }
        }

        if (coolTimeStart)
        {
            cooltime += Time.deltaTime;

            if (cooltime > 3f)
            {
                coolTimeStart = false;
                cooltime = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal") && Player.isRiding && PathManager.Instance.stage >= 5)
        {
            SceneManager.LoadScene("Gameclear");
        }
    }
}

