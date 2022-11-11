using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : Subject
{
    public float speed;
    private Rigidbody rb;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables = 9;

    public bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
            return;

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rb.velocity = new Vector3(x, rb.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            //other.gameObject.SetActive(false);
            //NotifyObservers();

            if(collectablesPicked == maxCollectables)
            {
                End();
            }
        }
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
        //NotifyObservers();
    }

    public void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        playButton.SetActive(true);
        LeaderboardUI.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        //NotifyObservers();
    }
}
