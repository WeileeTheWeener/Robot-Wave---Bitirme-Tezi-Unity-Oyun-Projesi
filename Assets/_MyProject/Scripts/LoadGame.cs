using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{  
    
    public void LoadNextScene() 
    {
        // BUILD SETTINGSDEKI AKTIF SAHNEDEN BIR SONRAKI SAHNEYE GEC

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    public void ExitGame()
    {
        // OYUNDAN CIK VE EDITORDEKI OYUNU DURDUR

         Application.Quit();
         //EditorApplication.isPlaying = false;
        
    }
}
