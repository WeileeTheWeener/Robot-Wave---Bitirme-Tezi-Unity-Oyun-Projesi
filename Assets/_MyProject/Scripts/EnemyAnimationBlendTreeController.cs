using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationBlendTreeController : MonoBehaviour
{
    Animator anim;
    NavMeshAgent agent;
    Vector2 velocity = Vector2.zero;
    bool canSeePlayer;
    public float ýdleShootingWeight;
    public float walkShootingWeight;
    public float ýdleShootingWeightAcceleration;
    public float walkShootingWeightAcceleration;
    public float ýdleShootingWeightDeceleration;
    public float walkShootingWeightDeceleration;


    void Start()
    {
        //ANIMATOR VE NAVMESHAGENT COMPONENTLERINI AL
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

     
    }

    void Update()
    {

        //OYUNCUYU GORUYORMU BILGISINI AL
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;

        //NAVMESHAGENT HIZ BILGISINI AL
        velocity = new Vector2(agent.velocity.x, agent.velocity.z);

        bool shouldMove;

        //BOOL DEGISKENINI HIZA GORE DEGISTIR
        if (agent.velocity.magnitude > 0.1f)
        {
            shouldMove = true;
        }
        else shouldMove = false;


        //OYUNCUYU GORUYORSA VE DURUYORSA
        //ANIMATORDEKI IDLESHOOTINGI ARTTIR VE WALKSHOOTINGI DUSUR
        if (canSeePlayer && !shouldMove)
        {
            
           ýdleShootingWeight = Mathf.MoveTowards(ýdleShootingWeight, 1f, ýdleShootingWeightAcceleration * Time.deltaTime);
           anim.SetLayerWeight(1, ýdleShootingWeight);


           walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
           anim.SetLayerWeight(2, walkShootingWeight);   
            
        }
        //OYUNCUYU GORUYORSA VE HAREKET EDIYORSA
        //ANIMATORDEKI WALKSHOOTINGI ARTTIR VE IDLESHOOTINGI DUSUR
        if (canSeePlayer && shouldMove)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 1f, walkShootingWeightAcceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);


            ýdleShootingWeight = Mathf.MoveTowards(ýdleShootingWeight, 0f, ýdleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, ýdleShootingWeight);
        }
        //OYUNCUYU GORMUYORSA SHOOTING WEIGHTLERI DUSUR
        if (!canSeePlayer)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);

            ýdleShootingWeight = Mathf.MoveTowards(ýdleShootingWeight, 0f, ýdleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, ýdleShootingWeight);
        }

 
        //DEGISKENLERI ANIMATOR DEGISKENLERINE ATA
        anim.SetBool("move", shouldMove);
        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.y);
        
       
    }


}
