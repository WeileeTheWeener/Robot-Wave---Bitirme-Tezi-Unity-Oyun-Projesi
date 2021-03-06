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
    public float ędleShootingWeight;
    public float walkShootingWeight;
    public float ędleShootingWeightAcceleration;
    public float walkShootingWeightAcceleration;
    public float ędleShootingWeightDeceleration;
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
            
           ędleShootingWeight = Mathf.MoveTowards(ędleShootingWeight, 1f, ędleShootingWeightAcceleration * Time.deltaTime);
           anim.SetLayerWeight(1, ędleShootingWeight);


           walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
           anim.SetLayerWeight(2, walkShootingWeight);   
            
        }
        //OYUNCUYU GORUYORSA VE HAREKET EDIYORSA
        //ANIMATORDEKI WALKSHOOTINGI ARTTIR VE IDLESHOOTINGI DUSUR
        if (canSeePlayer && shouldMove)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 1f, walkShootingWeightAcceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);


            ędleShootingWeight = Mathf.MoveTowards(ędleShootingWeight, 0f, ędleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, ędleShootingWeight);
        }
        //OYUNCUYU GORMUYORSA SHOOTING WEIGHTLERI DUSUR
        if (!canSeePlayer)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);

            ędleShootingWeight = Mathf.MoveTowards(ędleShootingWeight, 0f, ędleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, ędleShootingWeight);
        }

 
        //DEGISKENLERI ANIMATOR DEGISKENLERINE ATA
        anim.SetBool("move", shouldMove);
        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.y);
        
       
    }


}
