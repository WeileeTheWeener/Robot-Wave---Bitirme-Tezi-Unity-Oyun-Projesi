using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{  
    public void LoadNextScene() 
    {
        // Build settings den aktif sahneden bir sonraki sahneyi yükle.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    public void ExitGame()
    {
        // Oyundan çýk ve editördeki oyunu durdur.

         Application.Quit();
         EditorApplication.isPlaying = false;
        
    }
}
