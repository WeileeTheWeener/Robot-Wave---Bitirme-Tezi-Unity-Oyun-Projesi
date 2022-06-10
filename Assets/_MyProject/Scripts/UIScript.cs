using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    public TextMeshProUGUI currentWave;
    public TextMeshProUGUI remainingEnemy;
    public TextMeshProUGUI nextWaveStartTime;

    public void SetRemainingEnemy(int enemyCount)
    {
        remainingEnemy.SetText("Enemies Remaining: " + enemyCount);
    }
    public void SetCurrentWave(int waveCount)
    {
        currentWave.SetText("Current Wave: " + waveCount); 
    }
    public void SetNextWaveStartTime(float nextWaveTime)
    {
        nextWaveStartTime.SetText("Next Wave Starts In: " + ((int)nextWaveTime));
    }

}
