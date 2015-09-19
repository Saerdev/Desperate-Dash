using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Side-to-side movement speed
    public float horizontalSpeed = 10.0f;
    public float jumpForce = 7.0f;

    private Rigidbody rb;
    private Transform player;
    // For clamping
    private Vector3 pos;

    private int health;
    public int maxHealth = 5;

    private GameManager gm;

	// Use this for initialization
	void Start () {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        CheckHealth();
    }

    void Movement()
    {
        transform.Translate(Vector3.right * horizontalSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime);

        // Clamp the player's movement to the floor's width
        pos = player.position;
        pos.x = Mathf.Clamp(player.position.x, -4.5f, 4.5f);
        player.position = pos;

        // Jumping
        if (Input.GetButtonDown("Jump"))
            rb.velocity = Vector3.up * jumpForce;
    }

    void OnTriggerEnter(Collider other)
    {
        string color = other.CompareTag("Powerup") ? "green>" : "red>";
        Debug.Log("Collided with <color=" + color + other.name + "</color>");

        if (other.CompareTag("Powerup")) {
            other.gameObject.Recycle();
        }
        else if (other.CompareTag("Obstacle")) {
            health--;
            other.gameObject.Recycle();
        }
    }

    void CheckHealth()
    {
        if (health <= 0)
            gm.EndGame();
    }
}
