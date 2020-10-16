using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    public float JumpForce = 10f;
    public bool isGrounded = false;
    public GameObject deathMenu;
    private AudioSource damageSound;
    private Rigidbody2D c_rigidBody2D;
    // Start is called before the first frame update
    void Start()
    {
        c_rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(c_rigidBody2D.velocity.y) < 0.001f)
        {
            c_rigidBody2D.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f , 0f);
        transform.position += movement * movespeed * Time.deltaTime;
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
        deathMenu.SetActive(true);
    }


}
