using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBlendTreeController : MonoBehaviour
{
    Animator animator;
    float VelocityX;
    float VelocityZ;
    public float movementAcceleration = 2f;
    public float movementDeceleration = 2f;
    public float maximumRunVelocity = 2f;
    public float maximumWalkVelocity = 0.5f;
    public float shootingWeightAcceleration;
    public float shootingWeightDeceleration;
    public float reloadWeightAcceleration;
    public float reloadWeightDeceleration;
    public float shootingWeight;
    public float reloadWeight;
    public float currentMagazineCount;
    public bool keyUpToggle;
    public bool leftShiftPressedToggle;
    public bool hasAmmo;  
    public bool reloadPressed;
    public bool isReloading;
    public bool reloadTimerIsRunning;
    public float reloadTimeRemaining;
    private float reloadTimeStart;
  
    void Start()
    {
        //ANIMATOR DEGISKENINI AL
        animator = gameObject.GetComponent<Animator>();

        shootingWeight = 0f;
        hasAmmo = false;
        reloadPressed = false;
        reloadTimerIsRunning = false;
        reloadTimeStart = reloadTimeRemaining;
        keyUpToggle = false;
        leftShiftPressedToggle = false;
    }
    void Update()
    {   

       bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            leftShiftPressedToggle = !leftShiftPressedToggle;
        }

      
        currentMagazineCount = GetComponent<PlayerAmmo>().currentMagazines;

        if (currentMagazineCount > 0)
        {
            hasAmmo = true;
        }
        else hasAmmo = false;
        
       
      
        float currentMaxVelocity = leftShiftPressed ? maximumRunVelocity : maximumWalkVelocity;

        Vector2 worldInput = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z).normalized;
        Vector2 right = new Vector2(transform.right.x, transform.right.z).normalized;

        Vector2 input = new Vector2(Vector2.Dot(worldInput, right), Vector2.Dot(worldInput, forward)).normalized;

        VelocityX = Mathf.MoveTowards(VelocityX, input.x * currentMaxVelocity, Time.deltaTime * movementAcceleration);
        VelocityZ = Mathf.MoveTowards(VelocityZ, input.y * currentMaxVelocity, Time.deltaTime * movementAcceleration);

        if(input.x == 0 && input.y == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else animator.SetBool("isMoving", true);






        //BOSTA VURMA AGIRLIK DEGERINI ARTTIR
        if (Input.GetMouseButton(0) && (VelocityX == 0 && VelocityZ == 0) && !isReloading)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 1f, shootingWeightAcceleration * Time.deltaTime );
            animator.SetLayerWeight(1,shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float walkingWeight = Mathf.MoveTowards(animator.GetLayerWeight(2), 0f, shootingWeightDeceleration * Time.deltaTime);
            float runningWeight = Mathf.MoveTowards(animator.GetLayerWeight(3), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(2, walkingWeight);
            animator.SetLayerWeight(3, runningWeight);

        }
        //YURURKEN VURMA AGIRLIK DEGERINI ARTTIR
        if (Input.GetMouseButton(0) && !leftShiftPressed && (VelocityX!=0 || VelocityZ !=0) && !isReloading)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 1f, shootingWeightAcceleration * Time.deltaTime);
            animator.SetLayerWeight(2, shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float idleWeight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, shootingWeightDeceleration * Time.deltaTime);
            float runningWeight = Mathf.MoveTowards(animator.GetLayerWeight(3), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(1, idleWeight);
            animator.SetLayerWeight(3, runningWeight);

        }
        //KOSARKEN VURMA AGIRLIK DEGERINI ARTTIR
        if (Input.GetMouseButton(0) && leftShiftPressed && !isReloading)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 1f, shootingWeightAcceleration * Time.deltaTime);
            animator.SetLayerWeight(3, shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float idleWeight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, shootingWeightDeceleration * Time.deltaTime);
            float walkingWeight = Mathf.MoveTowards(animator.GetLayerWeight(2), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(1, idleWeight);
            animator.SetLayerWeight(2, walkingWeight);

        }
        //BOSTA VURMA AGIRLIK DEGERINI AZALT
        if (!Input.GetMouseButton(0) && (VelocityX == 0 && VelocityZ == 0))
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(1,shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float walkingWeight = Mathf.MoveTowards(animator.GetLayerWeight(2), 0f, shootingWeightDeceleration * Time.deltaTime);
            float runningWeight = Mathf.MoveTowards(animator.GetLayerWeight(3), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(2, walkingWeight);
            animator.SetLayerWeight(3, runningWeight);

        }
        //YURURKEN VURMA AGIRLIK DEGERINI AZALT
        if (!Input.GetMouseButton(0) && !leftShiftPressed && (VelocityX != 0 || VelocityZ != 0))
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(2, shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float idleWeight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, shootingWeightDeceleration * Time.deltaTime);
            float runningWeight = Mathf.MoveTowards(animator.GetLayerWeight(3), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(1, idleWeight);
            animator.SetLayerWeight(3, runningWeight);
        }
        //KOSARKEN VURMA AGIRLIK DEGERINI AZALT
        if (!Input.GetMouseButton(0) && leftShiftPressed)
        {
            shootingWeight = Mathf.MoveTowards(shootingWeight, 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(3, shootingWeight);

            //DIGER 2 AGIRLIGI AZALT 
            float idleWeight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, shootingWeightDeceleration * Time.deltaTime);
            float walkingWeight = Mathf.MoveTowards(animator.GetLayerWeight(2), 0f, shootingWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(1, idleWeight);
            animator.SetLayerWeight(2, walkingWeight);
        }

        //SARJOR DEGISTIRMEYI HALLET
        if (reloadTimerIsRunning)
        {
            
            if (reloadTimeRemaining > 0)
            {
              
                reloadTimeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                reloadTimeRemaining = 0;
                reloadTimerIsRunning = false;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadPressed = !reloadPressed;
        }
        if (animator.GetLayerWeight(4) == 1)
        {
            reloadPressed = false;
        }
        if (animator.GetLayerWeight(4) == 0)
        {
            reloadTimeRemaining = reloadTimeStart;
        }
        if (reloadPressed && hasAmmo)
        {
            reloadWeight = Mathf.MoveTowards(reloadWeight, 1f, reloadWeightAcceleration * Time.deltaTime);
            animator.SetLayerWeight(4, reloadWeight);
            reloadTimerIsRunning = true;
        }      
        if (!reloadPressed)
        {
            if(reloadTimeRemaining == 0)
            {
                reloadWeight = Mathf.MoveTowards(reloadWeight, 0f, reloadWeightDeceleration * Time.deltaTime);
                animator.SetLayerWeight(4, reloadWeight);
                
            }
         
        }
        //IF SHOOTING DECREASE IT WHILE RELOADING
        if (animator.GetLayerWeight(4) != 0)
        {
            isReloading = true;
           
            float idleWeight = Mathf.MoveTowards(animator.GetLayerWeight(1), 0f, reloadWeightDeceleration * Time.deltaTime);
            float walkingWeight = Mathf.MoveTowards(animator.GetLayerWeight(2), 0f, reloadWeightDeceleration * Time.deltaTime);
            float runningWeight = Mathf.MoveTowards(animator.GetLayerWeight(3), 0f, reloadWeightDeceleration * Time.deltaTime);
            animator.SetLayerWeight(3, runningWeight);
            animator.SetLayerWeight(1, idleWeight);
            animator.SetLayerWeight(2, walkingWeight);
        }
        if(animator.GetLayerWeight(4) == 0)
        {
            isReloading = false;
        }


        // DEGISKENLERI ANIMATOR AGIRLIK DEGERLERINE ATA
        animator.SetFloat("VelocityZ", VelocityZ); 
        animator.SetFloat("VelocityX", VelocityX);


    }
}
