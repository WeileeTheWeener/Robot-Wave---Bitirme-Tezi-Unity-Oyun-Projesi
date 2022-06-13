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
        
        //EGER OYUNCUYU GORUYOSA VEYA HASAR ALGILANDIYSA ATTACK STATE E GEC
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

   
    void Start()
    {
    
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;
        player = GameObject.FindWithTag("Player");    
        nextTimeToRoam = 0f;       
        startPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        roamPosition = startPosition;
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;


    }
    private void OnEnable()
    {
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;
    }

    
    public override void _Update()
    {
        gameObject.transform.root.GetComponent<FieldOfView>().radius = radius;
        gameObject.transform.root.GetComponent<FieldOfView>().angle = angle;

        animator.SetBool("isInAttackState", false);
        currentPosition = new Vector3(gameObject.transform.root.position.x, 0, gameObject.transform.root.position.z);
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;


        //EGER ONCEDEN BELIRLI BIR ROTASI YOK ISE
        if(!hasDistinctPath)
        {
          
            //EGER KENDI POZISYONU VE HEDEF POZISYONU ARASINDAKI MESAFE AZ ISE VE DOLASMA ZAMANI GELMIS ISE HAREKET ET
            if (Vector3.Distance(currentPosition, roamPosition) <= 0.1f && Time.time >= nextTimeToRoam)
            {
                nextTimeToRoam = Time.time + idleTime; //BIDAHAKI DOLASMA ZAMANI SUANKI SURE + BEKLEME SURESI

                roamPosition = GetValidPosition(currentPosition, roamingDistance);

                SetRoamPoint(roamPosition);
                MoveToRoamPoint(roamPosition);

             
            }

        }
        //EGER ONCEDEN BELIRLI BIR ROTASI VAR ISE
        if(hasDistinctPath)
        {

            //EGER SUANKI POZISYON ILE BASLANGIC POZISYONU YAKIN ISE VE
            //BEKLEME ZAMANI DOLMUS ISE HEDEFE GIT
            if (Vector3.Distance(currentPosition, startPosition) <= 0.1f && Time.time >= nextTimeToRoam)
            {
                nextTimeToRoam = Time.time + idleTime; //BIDAHAKI DOLASMA ZAMANI SUANKI SURE + BEKLEME SURESI
                SetRoamPoint(destinationPoint.transform.position);
         
            }
            else
            {
                MoveToRoamPoint(roamPosition);
            }
            //EGER SUANKI POZISYON ILE HEDEF POZISYON YAKIN ISE VE
            //BEKLEME ZAMANI DOLMUS ISE BASLANGIC POZISYONUNA DON
            if (Vector3.Distance(currentPosition, destinationPoint.transform.position) <= 0.1f && Time.time >= nextTimeToRoam) 
            {
                nextTimeToRoam = Time.time + idleTime;
                SetRoamPoint(startPosition);
             
            }
            else
            {
                MoveToRoamPoint(roamPosition);
            }

        }
        //EGER NAVMESHAGENTIN HEDEF NOKTASINA MESAFESI VARSA
        //HAREKET EDIYORDUR YOKSA ETMIYORDUR
        if(rootNavMeshAgent.remainingDistance>0)
        {
            animator.SetBool("isMoving", true);
           
        }
        else
        {
            animator.SetBool("isMoving", false);
            
           
        }
        //ANLIK CANI KAYDET
        cachedHealth = gameObject.transform.root.GetComponent<Stats>().currentHealth;

    }
    //ANLIK POZISYON VE HEDEF ARASI CIZGI CIZ
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.root.position, roamPosition);
    }
    //NAVMESHAGENTIN GIDICEGI YERI AYARLAR VE GITMESINI SAGLAR
    private void MoveToRoamPoint(Vector3 position)
    {
        rootNavMeshAgent.SetDestination(position);
    }
    //GIDIS YERI BELIRLE
    private void SetRoamPoint(Vector3 position)
    { 
        roamPosition = position;
       
    }
    //RASTGELE YÖN DÖNDÜR
    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f)).normalized;
    }
    // RASTGELE POZISYON DONDUR
    public static Vector3 GetRandomPosition(Vector3 currentPos,float roamingDistance)
    {     
        //RASTGELE YÖN VEKTORU ILE MESAFEYI CARPARAK POZISYON ELDE ET
        return currentPos + GetRandomDir() * roamingDistance; 
    }
    //POZISYON NAVMESHIN YANE YURUNEBILIR ALANIN USTUNDEMI KONTROL ET
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
    //EGER SUANKI CAN BIR ONCEKI CANA ESIT DEGILSE HASAR TESPIT EDILMISTIR
    public bool damageDetected()
    {
        return cachedHealth != gameObject.transform.root.GetComponent<Stats>().currentHealth;


    }


}
