using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject player;
    public UIScript uiScript;
    public AudioSource musicPlayer;
    public Light light;
    public List<GameObject> GameObjectsToRandomize;
    public float spawnRate;
    public float waitForWaveSeconds;
    public float nextTimeToSpawn;
    public float nextTimeToStartNewWave;
    public float currentWavesMaxSpawnNumber;
    public float currentWavesCurrentSpawnNumber;
    public int currentWaveNumber; 
    public bool allWaveEnemiesSpawned;
    public List<GameObject> currentWaveEnemies;
    public Transform[] spawnerTransforms;
    public static int remainingEnemies;
    public IEnumerator musicFade;

    // Start is called before the first frame update
    void Start()
    {
        currentWavesCurrentSpawnNumber = 0;
        currentWaveNumber = 1;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {


        if (currentWavesCurrentSpawnNumber >= currentWavesMaxSpawnNumber)
        {
           
            allWaveEnemiesSpawned = true;

            if (AllEnemiesDead())
            {
                if(musicFade != null)
                {
                    StopCoroutine(musicFade);
                }
                musicFade = FadeOut();
                StartCoroutine(musicFade);

                currentWaveNumber++;
                DeleteWavesEnemies();
                currentWavesCurrentSpawnNumber = 0;
                currentWavesMaxSpawnNumber += 3;
                allWaveEnemiesSpawned = false;
                nextTimeToStartNewWave = Time.time + waitForWaveSeconds;

               foreach(GameObject gameObject in GameObjectsToRandomize)
               {
                    gameObject.GetComponent<RandomizePosition>().RandomizeGameObjectPosition();
               }

            }

        }      
        if (Time.time >= nextTimeToSpawn && !allWaveEnemiesSpawned)
        {
            if(Time.time < nextTimeToStartNewWave)
            {
                goto waveSpawnBreak;
            }
            nextTimeToSpawn = Time.time + spawnRate;
            SpawnObject();

            if(currentWavesCurrentSpawnNumber == 0)
            {
                if (musicFade != null)
                {
                    StopCoroutine(musicFade);
                }
                musicFade = FadeIn();
                StartCoroutine(musicFade);
            }
            currentWavesCurrentSpawnNumber++;

          
        }
        waveSpawnBreak:;


        uiScript.SetRemainingEnemy(remainingEnemies);
        uiScript.SetCurrentWave(currentWaveNumber);

        if(nextTimeToStartNewWave - Time.time > 0)
        {
            uiScript.nextWaveStartTime.gameObject.SetActive(true);
        }
        else uiScript.nextWaveStartTime.gameObject.SetActive(false);

        uiScript.SetNextWaveStartTime(nextTimeToStartNewWave-Time.time);

    }

    void SpawnObject()
    {
        GameObject enemy = GameObject.Instantiate(prefab);       
        Bounds bounds = GetClosestSpawn().GetComponent<Collider>().bounds;
        Vector3 randomPointInsideCollider = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 0, Random.Range(bounds.min.z, bounds.max.z));
        enemy.GetComponent<Transform>().position = randomPointInsideCollider;
        currentWaveEnemies.Add(enemy);

    }
    void DeleteWavesEnemies()
    {
        currentWaveEnemies.Clear();
    }
    Transform GetClosestSpawn()
    {
        float closestDistance;
        Transform closestTransform = null;

        closestDistance = Mathf.Infinity;

        foreach (Transform transformm in spawnerTransforms)
        {
            if(Vector3.Distance(player.transform.position,transformm.position) <= closestDistance)
            {
                closestDistance = Vector3.Distance(player.transform.position, transformm.position);
                closestTransform = transformm;

            }
        }
        return closestTransform;
    }
    public IEnumerator FadeIn()
    {
        for(int i = 0; i < 100; i++)
        {
            musicPlayer.volume = (float)i / 100f;
            light.colorTemperature = ((1f-(float)i / 100f)) * 6000 +1000;
            yield return new WaitForSeconds(0.025f);
        }
    }
    public IEnumerator FadeOut()
    {
        for (int i = 100; i >= 0; i--)
        {
            musicPlayer.volume = (float)i / 100f;
            light.colorTemperature = ((1f - (float)i / 100f)) * 6000 + 1000;
            yield return new WaitForSeconds(0.025f);
        }
    }
    bool AllEnemiesDead()
    {

        foreach (GameObject enemy in currentWaveEnemies)
        {
            if (enemy.active)
            {
                return false;
            }
        }
        return true;
    }
}