using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool playerIsDead;
    private RoadManager roadManager;
    private PlayerController playerController;
    private Spawner spawner;

    private float score;
    private Text scoreText;
    private Text restartText;

	// Use this for initialization
	void Awake() {
        score = 0;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        roadManager = GameObject.Find("RoadManager").GetComponent<RoadManager>();
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        restartText = GameObject.Find("ScoreCanvas").transform.GetChild(1).GetComponent<Text>();
        restartText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScore();
       
        if (playerIsDead) {
            if (Input.GetKeyDown(KeyCode.Space))
                Application.LoadLevel(Application.loadedLevel);

            if (!restartText.enabled)
                restartText.enabled = true;
        }
	}

    void UpdateScore()
    {
        if (!playerIsDead) {
            score += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString("F0");
        }
    }

    public void EndGame()
    {
        playerIsDead = true;
        roadManager.scrollSpeed = 0;
        GameObject.Destroy(playerController.gameObject);
        spawner.enabled = false;
    }
}
