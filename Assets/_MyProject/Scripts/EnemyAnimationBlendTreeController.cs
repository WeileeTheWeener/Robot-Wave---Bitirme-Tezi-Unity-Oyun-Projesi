using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationBlendTreeController : MonoBehaviour
{
    Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    bool shouldShoot;
    bool canSeePlayer;
    public float shootingWeight;
    public float shootingWeightAcceleration;
    public float shootingWeightDeceleration;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Dont update position automatically
        agent.updatePosition = false;
     
    }

    void Update()
    {
        canSeePlayer = gameObject.transform.root.GetComponent<FieldOfView>().canSeePlayer;

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        if(canSeePlayer)
        {
            shouldShoot = true;
        }
        if(!canSeePlayer)
        {
            shouldShoot = false;
        }

        if (shouldShoot)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 1f, shootingWeightAcceleration * Time.deltaTime);
            anim.SetLayerWeight(1, shootingWeight);
            //animator.SetLayerWeight(1, 1f);

        }
        else if (!shouldShoot)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 0f, shootingWeightDeceleration * Time.deltaTime);
            anim.SetLayerWeight(1, shootingWeight);
            //animator.SetLayerWeight(1, 0f);

        }

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("VelocityX", velocity.x);
        anim.SetFloat("VelocityZ", velocity.y);

        // GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;

        //look at
        /*LookAt lookAt = GetComponent<LookAt>();
        if (lookAt)
            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward; */
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
    }
}
