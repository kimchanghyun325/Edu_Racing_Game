using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public AudioClip panelSound, explainSound, portalSound, correctSound, getRadishSound, itemDropSound, itemPickUpSound;
    public AudioSource audioSource;
    public AudioClip[] footStep, background, car;
    public float stepInteval = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("Gameover") ||
            SceneManager.GetActiveScene().name.Equals("Gameclear"))
        {
            this.gameObject.SetActive(false);
        }
    }

    public void PlayPanelSound()
    {
        audioSource.PlayOneShot(panelSound);
    }

    public void FootStep()
    {
        if (Player.isRun)
        {
            stepInteval += Time.deltaTime;

            if (stepInteval >= 0.34f)
            {
                if (SceneManager.GetActiveScene().name == "City")
                    audioSource.PlayOneShot(footStep[Random.Range(0, 4)]);
                else
                    audioSource.PlayOneShot(footStep[Random.Range(5, 8)]);
                stepInteval = 0.0f;
            }
        }
    }
}
