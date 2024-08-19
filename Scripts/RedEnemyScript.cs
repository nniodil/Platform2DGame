using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedEnnemyScript : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform currentPatrolPoint;
    public int currentPatrolPointIndex = 0;
    public float speed;
    public int health;
    
    
    void Start()
    {
        currentPatrolPoint = patrolPoints[currentPatrolPointIndex];
        if (currentPatrolPoint.position.x < transform.position.x)
        {
            speed = -1f;
        }
        else
        {
            speed = 1f;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);

        if (Vector2.Distance(currentPatrolPoint.position, transform.position) < 0.4f)
        {
            if (currentPatrolPointIndex < patrolPoints.Length - 1)
            {
                currentPatrolPointIndex++;
            }
            else
            {
                currentPatrolPointIndex = 0;
            }
            
            currentPatrolPoint = patrolPoints[currentPatrolPointIndex];
            
            if(gameObject.tag == "demon")
            {
                if (currentPatrolPoint.position.x < transform.position.x)
                {
                    speed = -1f;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    speed = 1f;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
            
            if(gameObject.tag == "enemy")
            {
                if (currentPatrolPoint.position.x < transform.position.x)
                {
                    speed = -1f;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    speed = 1f;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bullets"))
        {
            health -= 5;
            
            Destroy(other.gameObject);
            
            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
