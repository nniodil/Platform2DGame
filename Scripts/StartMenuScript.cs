using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMeny : MonoBehaviour
{
   public void StartMenu()
    {
        SceneManager.LoadScene("Level 1");
        
        Cursor.visible = false;
    }
}
