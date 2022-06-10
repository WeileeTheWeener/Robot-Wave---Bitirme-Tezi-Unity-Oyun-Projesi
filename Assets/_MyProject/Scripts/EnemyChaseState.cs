using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class EnemyChaseState : State
{
    public EnemyAttackState attackState;
    public EnemyStateManager stateManager;
    public EnemyIdleState idleState;
    public GameObject player;
    private Vector3 startLoc;
    public float nextTimeToMove;
    public float idleTime;
    public float chasePlayerRange;
    public float attackRange;
    public float distanceBetweenTwoObjects;
    public float radius;
    public float angle;
    public float searchTime;
    public float searchTimeRemaining;
    public bool isInAttackRange;
    public bool stopChasing;
    public bool canSeePlayer;
    public bool makeEnemyATurret;
    public bool timerIsRunning;
    public bool isGoingBack;
    public float forwardDist;
    public override State RunCurrentState()
    {

        if (canSeePlayer)
        {
            Debug.Log("return to attackstate");
            return attackState;
           
        }
        if(ReturnToIdle())
        {
            Debug.Log("return to idle state");
            return idleState;
            
        }
        else
        {
            //Debug.Log("return this");
            return this;
        }



    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        startLoc = gameObject.transform.root.position;

        searchTimeRemaining = searchTime;

    }
    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player");
        searchTimeRemaining = searchTime;
        isGoingBack = false;
    }


    // Update is called once per frame
    public override void _Update()
    {
    if(!isGoingBack)
        {
            gameObject.transform.root.GetComponent<FieldOfView>().radius = radius;
            gameObject.transform.root.GetComponent<FieldOfView>().angle = angle;

            if (player != null)
            {
                distanceBetweenTwoObjects = Vector3.Distance(player.transform.position, gameObject.transform.position);
            }

            canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;


            //StartTimer();


            if (searchTimeRemaining > 0)
            {
                searchTimeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                isGoingBack = true;
                


            }


            if (searchTimeRemaining > 0)
            {
                if (Time.time >= nextTimeToMove)
                {
                    nextTimeToMove = Time.time + idleTime;
                    //Move(player.transform.position);
                    Move(GetNextPoint());
                }

            }
            else
            {
                Move(startLoc);

            }
            /*if(!timerIsRunning && Vector3.Distance(gameObject.transform.root.position, startLoc) <= 0.1f)
            {
                timeRemaining = searchTime;
            }*/


            //checkIfCanSeePlayer();
            //chechIfPlayerIsInAttackRange();
         }     

    }


    private void Move(Vector3 point)
    {
        gameObject.transform.root.GetComponent<NavMeshAgent>().SetDestination(point);
    }
    private Vector3 GetNextPoint()
    {

        float distance = Random.Range(-0.5f, 0.5f);
        Vector3 forward = forwardDist * (player.transform.position -  gameObject.transform.root.transform.position).normalized;
        Vector3 x = Vector3.Cross(forward, Vector3.up).normalized;
        Vector3 point = forward + x * distance + gameObject.transform.root.transform.position; 
        return point;
    }
    /*private void StartTimer()
    {

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }


    }*/
    private bool ReturnToIdle()
    {
        if (Vector3.Distance(gameObject.transform.root.position, startLoc) <= 1f && searchTimeRemaining <= 0f)
        {
            return true;

        }
        else
        {
           
            return false;
        }
    }


}
