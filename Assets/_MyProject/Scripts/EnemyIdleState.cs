using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : State
{
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public NavMeshAgent rootNavMeshAgent;
    public Animator animator;    
    public GameObject player;
    public GameObject destinationPoint;
    private Vector3 startPosition;
    private Vector3 roamPosition;
    private Vector3 currentPosition;
    private float nextTimeToRoam;
    public float idleTime;
    public float roamingDistance;
    public float radius;
    public float angle;
    public bool canSeePlayer;
    public bool hasDistinctPath;
    public float cachedHealth;
    public override State RunCurrentState()
    {
        

        if (canSeePlayer || damageDetected())
        {          
            return attackState;
           
        }
        else
        {
            chaseState.gameObject.SetActive(false);
            return this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;
        player = GameObject.FindWithTag("Player");    
        //rootNavMeshAgent = gameObject.transform.parent.root.GetComponent<NavMeshAgent>();
        //animator = gameObject.transform.root.GetComponent<Animator>();
        nextTimeToRoam = 0f;       
        startPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        roamPosition = startPosition;
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;


    }
    private void OnEnable()
    {
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;
    }

    // Update is called once per frame
    public override void _Update()
    {
        gameObject.transform.root.GetComponent<FieldOfView>().radius = radius;
        gameObject.transform.root.GetComponent<FieldOfView>().angle = angle;

        animator.SetBool("isInAttackState", false);
        currentPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;
        //gameObject.transform.root.LookAt(new Vector3(roamPosition.x, gameObject.transform.root.position.y, roamPosition.z), Vector3.up);

        if(!hasDistinctPath)
        {
            if(damageDetected())
            {
                Debug.Log("damage detected");

            }
            if (Vector3.Distance(currentPosition, roamPosition) <= 0.1f && Time.time >= nextTimeToRoam)
            {
                nextTimeToRoam = Time.time + idleTime;
                
                //MoveToRoamPoint();


                roamPosition = GetValidPosition(currentPosition, roamingDistance);

                SetRoamPoint(roamPosition);
                MoveToRoamPoint(roamPosition);

                /*Debug.Log(Vector3.Distance(hit.position, roamPosition));
                Debug.DrawLine(hit.position, hit.position + Vector3.down, Color.yellow, 10f);
                Debug.DrawLine(roamPosition, roamPosition + Vector3.up, Color.blue, 10f);*/
            }

        }
        if(hasDistinctPath)
        {


            if (Vector3.Distance(currentPosition, startPosition) <= 0.1f && Time.time >= nextTimeToRoam)
            {
                nextTimeToRoam = Time.time + idleTime;
                SetRoamPoint(destinationPoint.transform.position);
                //MoveToRoamPoint();

            }
            else
            {
                MoveToRoamPoint(roamPosition);
            }
            if (Vector3.Distance(currentPosition, destinationPoint.transform.position) <= 0.1f && Time.time >= nextTimeToRoam) 
            {
                nextTimeToRoam = Time.time + idleTime;
                SetRoamPoint(startPosition);
                //MoveToRoamPoint();

            }
            else
            {
                MoveToRoamPoint(roamPosition);
            }

        }

        if(rootNavMeshAgent.remainingDistance>0)
        {
            animator.SetBool("isMoving", true);
           
        }
        else
        {
            animator.SetBool("isMoving", false);
            
           
        }
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.root.position, roamPosition);
    }
    private void MoveToRoamPoint(Vector3 position)
    {
        //rootNavMeshAgent.SetDestination(roamPosition);
        rootNavMeshAgent.SetDestination(position);
    }
    private void SetRoamPoint(Vector3 position)
    { 
        roamPosition = position;
       
    }
    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f)).normalized;
    }
    public static Vector3 GetRandomPosition(Vector3 currentPos,float roamingDistance)
    {     
        return currentPos + GetRandomDir() * roamingDistance;
    }
    public static Vector3 GetValidPosition(Vector3 currentPos, float roamingDistance)
    {
        NavMeshHit hit;
        Vector3 randomPos;

        do
        {
            randomPos = GetRandomPosition(currentPos, roamingDistance);
            NavMesh.SamplePosition(randomPos, out hit, 10f, NavMesh.AllAreas);
        }
        while (Vector3.Distance(hit.position, randomPos) > 0.1f);
        return randomPos;
    }
    public bool damageDetected()
    {
        return cachedHealth != gameObject.transform.root.GetComponent<Stats>().currentHealth;

        


    }


}
