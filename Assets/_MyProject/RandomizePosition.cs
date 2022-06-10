using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomizePosition : MonoBehaviour
{
    
    public  int rangeMin;
    public  int rangeMax;
    private  int range;
    public  float maxDistance;
    private  Vector3 randomDirection;
    static  NavMeshHit hit;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RandomizeGameObjectPosition()
    {
               
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            range = Random.Range(rangeMin, rangeMax);
            Vector3 point = gameObject.transform.position + randomDirection * range;


            if (NavMesh.SamplePosition(point, out hit, maxDistance, 1))
            {
                point = hit.position;
            }
            else
            {
                point = gameObject.transform.position;
            }

            gameObject.transform.position = point;
        

            
        

        
    }
}
