using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : State
{
    public EnemyChaseState chaseState;
    public EnemyIdleState idleState;
    public EnemySoundManager enemySoundManager;
    private NavMeshAgent rootNavMeshAgent;
    public Animator animator;
    private GameObject player;
    public Transform barrel;
    private RaycastHit hit;
    private Ray ray;
    private Vector3 startPosition;
    private Vector3 roamPosition;
    private Vector3 currentPosition;
    private Vector3 hitPointOfBarrel;
    private float nextTimeToFire;
    public  float fireRate;
    private float nextTimeToRoam;
    public  float idleTime;
    public  float roamingDistance;
    public float range;
    public float damage;
    public float radius;
    public float angle;
    public bool canSeePlayer;      
    public Color startColor;
    public Color endColor;
    public float gunshotVolume;

    public override State RunCurrentState()
    {       
        if(!canSeePlayer)
        {
      
            chaseState.gameObject.SetActive(true);
            return chaseState;
            
         
        }
        else
        {
            return this;
        }

            
    }

    // Start is called before the first frame update
    void Start()
    {

        
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;
        player = GameObject.FindWithTag("Player");
        rootNavMeshAgent = gameObject.transform.root.GetComponent<NavMeshAgent>();      
        startPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        roamPosition = startPosition;
        gunshotVolume = enemySoundManager.gunshotVolume;
  

    }   
    // Update is called once per frame
    public override void _Update()
    {
        
        gameObject.transform.root.GetComponent<FieldOfView>().radius = radius;
        gameObject.transform.root.GetComponent<FieldOfView>().angle = angle;

        animator.SetBool("isInAttackState", true);
        currentPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;         
        gameObject.transform.root.LookAt(new Vector3(player.transform.position.x,gameObject.transform.root.position.y,player.transform.position.z), Vector3.up);


        if (Time.time >= nextTimeToFire && (animator.GetLayerWeight(1) == 1 || animator.GetLayerWeight(2) == 1))
        {
                 nextTimeToFire = Time.time + fireRate;
                 Shoot(barrel);
                 
        }
  
        if (Vector3.Distance(currentPosition, roamPosition) <= 0.1f && Time.time >= nextTimeToRoam)
        {
            nextTimeToRoam = Time.time + idleTime;
           
       

            NavMeshHit hit;
            Vector3 roamPosition = EnemyIdleState.GetValidPosition(currentPosition, roamingDistance);
            SetRoamPoint(roamPosition);
            MoveToRoamPoint(roamPosition);

  
        }

        if (rootNavMeshAgent.remainingDistance > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

    }
    private void MoveToRoamPoint(Vector3 position)
    {
       
        rootNavMeshAgent.SetDestination(position);
    }
    private void SetRoamPoint(Vector3 position)
    {
        
        roamPosition = position;


    }
    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
    public static Vector3 GetRandomPosition(Vector3 currentPos, float roamingDistance)
    {
        return currentPos + GetRandomDir() * roamingDistance;
    }
    void Shoot(Transform barrel)
    {
        
        ray = new Ray(gameObject.transform.position, gameObject.transform.forward);

        Vector3 dirr = (ray.GetPoint(range) - barrel.transform.position).normalized;
        hitPointOfBarrel = barrel.transform.position + dirr * range;


        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("we hit the player");
                hit.transform.GetComponent<Stats>().TakeDamage(damage);
            }
            if (hit.collider.gameObject.CompareTag("Obstacle") && hit.collider != null)
            {
                hitPointOfBarrel = barrel.transform.position + dirr * hit.distance;
            }
        }
        Vector3[] positions = new Vector3[2];

        BulletTrailUtil.PlayTrailAnimation(
            barrel.transform.position, 
            hitPointOfBarrel,
            startColor,
            endColor);

        enemySoundManager.PlayGunShot(gunshotVolume);

    }


}



