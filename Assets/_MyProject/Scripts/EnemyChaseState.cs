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
        //EGER OYUNCUYU GORUYORSA SALDIRI DURUMUNA GEC
        if (canSeePlayer)
        {
            Debug.Log("return to attackstate");
            return attackState;
           
        }
        //EGER BOSA DONME FONKSIYONU TRUE ISE BOS DURUMUNA GEC
        if(ReturnToIdle())
        {
            Debug.Log("return to idle state");
            return idleState;
            
        }
        else
        {
     
            return this;
        }

    }
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
    public override void _Update()
    {
    //EGER BOS DURUMUNA GERI DONMUYOR ISE
    if(!isGoingBack)
        {
            gameObject.transform.root.GetComponent<FieldOfView>().radius = radius;
            gameObject.transform.root.GetComponent<FieldOfView>().angle = angle;

            if (player != null)
            {
                distanceBetweenTwoObjects = Vector3.Distance(player.transform.position, gameObject.transform.position);
            }
            canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;

            //OYUNCUYU ARAMA ZAMANININI AZALT DURDUGU NOKTADA BELIRLI BIR SURE BEKLE VE ARAMAYA DEVAM ET
            if (searchTimeRemaining > 0)
            {
                searchTimeRemaining -= Time.deltaTime;

                if (Time.time >= nextTimeToMove)
                {
                    nextTimeToMove = Time.time + idleTime;
                    Move(GetNextPoint());
                }
            }//ZAMAN DOLMUS ISE BASLANGIC POZISYONUNA DON
            else
            {
                Debug.Log("Time has run out!");
                isGoingBack = true;
                Move(startLoc);
            }

           /* if (searchTimeRemaining > 0)
            {
                if (Time.time >= nextTimeToMove)
                {
                    nextTimeToMove = Time.time + idleTime;          
                    Move(GetNextPoint());
                }

            }
            else
            {
                Move(startLoc);

            } */    
         }     
    }
    //NAVMESHAGENT HEDEF NOKTASI BELIRLE
    private void Move(Vector3 point)
    {
        gameObject.transform.root.GetComponent<NavMeshAgent>().SetDestination(point);
    }
    //OYUNCUYU TAKIP EDERKEN GEREKLI OLAN BIR SONRAKI NOKTAYI BELIRLE
    private Vector3 GetNextPoint()
    {

        float distance = Random.Range(-0.5f, 0.5f);
        Vector3 forward = forwardDist * (player.transform.position -  gameObject.transform.root.transform.position).normalized;
        Vector3 x = Vector3.Cross(forward, Vector3.up).normalized;
        Vector3 point = forward + x * distance + gameObject.transform.root.transform.position; 
        return point;
    }
    //BOS DURUMA DONMESI ICIN GEREKEN MANTIK
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
