using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float JumpForce = 10f;
    
    public GameObject deathMenu;
    public Transform groundCheck;
    public Vector2 groundDistance;
    public LayerMask groundMask;

    private bool isGrounded = false;
    private bool isDead = false;
    private AudioSource damageSound;
    private Rigidbody2D c_rigidBody2D;
    private ContactFilter2D filter;
    private Collider2D collider2d;

    void Start()
    {
        c_rigidBody2D = GetComponent<Rigidbody2D>();
        damageSound = GetComponent<AudioSource>();

        collider2d = gameObject.GetComponent<BoxCollider2D>();
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    void Update()
    {
        
        float x = Input.GetAxis("Horizontal");
        Vector2 movement = c_rigidBody2D.velocity;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundDistance, 0, groundMask);

        if (!isDead)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                movement.y = JumpForce;
            }

            movement.x = x * moveSpeed;

            c_rigidBody2D.velocity = movement;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Damage"))
        {
            KillPlayer();
            damageSound.Play();
        }

        if(collision.gameObject.tag == "Finish")
        {
            Scene scene = SceneManager.GetActiveScene();
            int next_scene = scene.buildIndex + 1;
            SceneManager.LoadScene(next_scene);
        }
    }

    private void KillPlayer()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        isDead = true;
        deathMenu.SetActive(true);
    }

    private bool CheckCollisions(Collider2D collider, Vector2 direction, float distance)
    {
        if(collider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];

            Debug.Log("Hello");
            return true;
        }
        Debug.Log("Bye");
        return false;
    }
}
