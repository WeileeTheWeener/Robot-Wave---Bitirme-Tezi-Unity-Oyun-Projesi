using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{  
    public void LoadNextScene() 
    {
        // Build settings den aktif sahneden bir sonraki sahneyi y�kle.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    public void ExitGame()
    {
        // Oyundan ��k ve edit�rdeki oyunu durdur.

         Application.Quit();
         EditorApplication.isPlaying = false;
        
    }
}
