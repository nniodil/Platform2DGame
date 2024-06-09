using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonScript : MonoBehaviour
{   
    private RedEnnemyScript RedScript;
    public GameObject endGame;
    public int demonHealth;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endGame.activeInHierarchy)
        {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {



        if (other.CompareTag("bullets"))
        {

            demonHealth -= 5;
            Destroy(other.gameObject);

            if (demonHealth <= 0)
            {
                gameObject.SetActive(false);
                endGame.SetActive(true);
                
            }
        }

    }
}