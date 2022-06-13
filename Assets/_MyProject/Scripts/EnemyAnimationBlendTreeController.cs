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
    public float �dleShootingWeight;
    public float walkShootingWeight;
    public float �dleShootingWeightAcceleration;
    public float walkShootingWeightAcceleration;
    public float �dleShootingWeightDeceleration;
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
            
           �dleShootingWeight = Mathf.MoveTowards(�dleShootingWeight, 1f, �dleShootingWeightAcceleration * Time.deltaTime);
           anim.SetLayerWeight(1, �dleShootingWeight);


           walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
           anim.SetLayerWeight(2, walkShootingWeight);   
            
        }
        //OYUNCUYU GORUYORSA VE HAREKET EDIYORSA
        //ANIMATORDEKI WALKSHOOTINGI ARTTIR VE IDLESHOOTINGI DUSUR
        if (canSeePlayer && shouldMove)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 1f, walkShootingWeightAcceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);


            �dleShootingWeight = Mathf.MoveTowards(�dleShootingWeight, 0f, �dleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, �dleShootingWeight);
        }
        //OYUNCUYU GORMUYORSA SHOOTING WEIGHTLERI DUSUR
        if (!canSeePlayer)
        {
            walkShootingWeight = Mathf.MoveTowards(walkShootingWeight, 0f, walkShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(2, walkShootingWeight);

            �dleShootingWeight = Mathf.MoveTowards(�dleShootingWeight, 0f, �dleShootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, �dleShootingWeight);
        }

 
        //DEGISKENLERI ANIMATOR DEGISKENLERINE ATA
        anim.SetBool("move", shouldMove);
        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.y);
        
       
    }


}
