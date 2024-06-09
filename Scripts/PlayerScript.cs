using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb2D;
    public float jumpForce;
    float horizontal = 0f;
    public GameObject player;
    Animator anim;

    public GameObject laserBeam;
    public Transform laserPosRight;
    public Transform laserPosLeft;
    public bool facingRight;
    private float health;
    public Transform currentCheckPoint;
    SpriteRenderer spriteRenderer;
    public bool disabled = false;
    public Slider healthSlider;
    public float maxHealth;
    public int lives;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;
    public Transform startPosition;
    public GameObject screamer;
    public GameObject screamerCollider;
    




    // Start is called before the first frame update
    void Start()
    {
        
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        livesText.text = "Lives: " + lives;

    }

    // Update is called once per frame
    void Update()
    {
        

        if (gameOverPanel.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }

        if (disabled)
        {
            return;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            player.GetComponent<SpriteRenderer>().flipX = true;
            facingRight = false;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            player.GetComponent<SpriteRenderer>().flipX = false;
            facingRight = true;
        }

        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontal);

        anim.SetFloat("speed", Mathf.Abs(horizontal));


        //no Air control
        if (Mathf.Abs(rb2D.velocity.y) < 0.01)
        {
            horizontal = Input.GetAxis("Horizontal");
        }

        //Animations
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2D.velocity.y) < 0.01)
        {
            rb2D.AddRelativeForce(Vector2.up * jumpForce);
        }

        if (!anim.GetBool("jumping") && Mathf.Abs(rb2D.velocity.y) > 0.05)
        {
            anim.SetBool("jumping", true);
        }
        if (anim.GetBool("jumping") && Mathf.Abs(rb2D.velocity.y) < 0.05)
        {
            anim.SetBool("jumping", false);
        }
        if (!anim.GetBool("jumping") && Input.GetAxis("Horizontal") == 0 && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("jumping", true);
        }

        if (anim.GetBool("shooting") == false && Input.GetButtonDown("Fire1") && anim.GetBool("jumping") == false && anim.GetBool("runandshoot") == false)
        {
            anim.SetBool("shooting", true);


        }
        if (anim.GetBool("shooting") == true && anim.GetBool("jumping") == true)
        {
            anim.SetBool("shooting", false);
        }

        if (Input.GetButtonUp("Fire1") == true)
        {
            anim.SetBool("shooting", false);

        }

        Destroy(GameObject.Find("bullets(Clone)"), 0.5f);

        if (startPosition == null)
        {
            startPosition = GameObject.FindGameObjectWithTag("start point").transform;
            transform.position = startPosition.position;

        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    void LaserAttack()
    {
        if (facingRight)
        {
            Instantiate(laserBeam, laserPosRight.position, transform.rotation);


        }
        else
        {
            Instantiate(laserBeam, laserPosLeft.position, Quaternion.Euler(0f, 0f, 180f));

        }

    }
    void Respawn()
    {
        transform.position = currentCheckPoint.position;
        spriteRenderer.enabled = true;
        disabled = false;
        health = maxHealth;
        healthSlider.value = 1f;


    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            health -= 5;


            //Check for death
            if (health <= 0)
            {
                lives--;

                //Check to see if we are out of lives
                if (health <= 0 && lives == 0)
                {
                    gameOverPanel.SetActive(true);
                    
                    
                }
                else
                {

                    Invoke("Respawn", 1f);

                }

                health = 0;
                spriteRenderer.enabled = false;
                transform.position = currentCheckPoint.position;
                disabled = true;
                livesText.text = "Lives: " + lives;



            }
            healthSlider.value = health / maxHealth;



        }

        if (collision.gameObject.CompareTag("deadbarrier"))
        {

            lives--;

            //Check to see if we are out of lives
            if (lives == 0)
            {
                gameOverPanel.SetActive(true);
            }
            else
            {

                Invoke("Respawn", 1f);

            }

            health = 0;
            spriteRenderer.enabled = false;
            transform.position = currentCheckPoint.position;
            disabled = true;
            livesText.text = "Lives: " + lives;

        }
        healthSlider.value = health / maxHealth;

        if (collision.gameObject.CompareTag("demon"))
        {
            health -= 5;


            //Check for death
            if (health <= 0)
            {
                lives--;

                //Check to see if we are out of lives
                if (health <= 0 && lives == 0)
                {
                    gameOverPanel.SetActive(true);
                    Cursor.visible= true;
                }
                else
                {

                    Invoke("Respawn", 1f);

                }

                health = 0;
                spriteRenderer.enabled = false;
                transform.position = currentCheckPoint.position;
                disabled = true;
                livesText.text = "Lives: " + lives;


            }


        }
    }
        //this function will run whenever the player collides with a trigger collider
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("checkpoint"))
            {
                currentCheckPoint = other.transform;

            }
            if (other.CompareTag("exit point"))
            {
                if (SceneManager.GetActiveScene().name == "Level 1")
                {
                    SceneManager.LoadScene("Level 2");
                }
                if (SceneManager.GetActiveScene().name == "Level 2")
                {
                    SceneManager.LoadScene("Level 3");
                }
                if (SceneManager.GetActiveScene().name == "Level 3")
                {
                    SceneManager.LoadScene("Level 4");
                }
                if (SceneManager.GetActiveScene().name == "Level 4")
                {
                    SceneManager.LoadScene("Level 5");
                }
            }

            if (other.CompareTag("screamer"))
            {
                screamer.SetActive(true);
                Destroy(screamer, 2f);
                screamerCollider.SetActive(false);

            }
        }



    

    public void PlayAgain()
    {
        Cursor.visible = false;
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            SceneManager.LoadScene("Level 1");
        }
        if (SceneManager.GetActiveScene().name == "Level 2")
        {
            SceneManager.LoadScene("Level 1");
        }
        if (SceneManager.GetActiveScene().name == "Level 3")
        {
            SceneManager.LoadScene("Level 2");
        }
        if (SceneManager.GetActiveScene().name == "Level 4")
        {
            SceneManager.LoadScene("Level 3");
        };
        if (SceneManager.GetActiveScene().name == "Level 5")
        {
            SceneManager.LoadScene("Level 4");
        };
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}



