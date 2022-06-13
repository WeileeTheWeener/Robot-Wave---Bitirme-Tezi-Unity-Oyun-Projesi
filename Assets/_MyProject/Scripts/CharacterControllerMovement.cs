using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    CharacterController characterController;
    public float Velocity;
    public float walkVelocity;
    public float runVelocity;
    public float runAcceleration;
    public float runDeceleration;
    public float walkSpeedMultiplyer;
    public float runSpeedMultiplyer;
    private float speedMult;
    private Camera cam;
    private Vector3 mouseWorldPosition;
    public LayerMask layerMask;
    public Vector3 inputVector;
    public Vector3 moveDirection;

    void Start()
    {
        //KARAKTER KONTROLCU COMPONENTINI VE KAMERAYI AL
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
     
    }


    void Update()
    {
        //FONKSIYONLARI CALISTIR
        LookAtMouseDirection();
        HandleMovement();
        HandleGravity();     
    }

    void HandleGravity()
    {
        //RIGIDBODY KULLANMADIGIMIZ ICIN YERCEKIMINI BASIT BIR SEKILDE SIMULE ET
        if (characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -0.1f, 0));
        }
        else if (!characterController.isGrounded)
        {
            characterController.Move(new Vector3(0, -9.8f, 0));
        }
    }

    void HandleMovement()
    {
        
        bool leftShiftPressed = Input.GetKey(KeyCode.LeftShift);

        //GIRDI EKSENLERINI X VE Z YI AL BASILI OLDUGU AN 1 OLUR
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
       
        //EGER SHIFTE BASILI ÝSE VELOCITY DEGISKENINI SUANKI HALINDEN RUNVELOCITYE DOGRU CEK 
        if (leftShiftPressed)
        {
            Velocity = Mathf.MoveTowards(Velocity, runVelocity, runAcceleration * Time.deltaTime);
            speedMult = runSpeedMultiplyer;
        }
        //EGER SHIFTE BASILI DEGILSE VELOCITY DEGISKENINI SUANKI HALINDEN WALKVELOCITYE DOGRU CEK 
        if (!leftShiftPressed)
        {
            Velocity = Mathf.MoveTowards(Velocity, walkVelocity, runDeceleration * Time.deltaTime);
            speedMult = walkSpeedMultiplyer;
        }
        //EGER GIRDI YOKSA HIZI SIFIRLA VEYA DUSUR
        if(inputVector.x == 0f && inputVector.z ==0f)
        {
            Velocity = 0f;
        }
        if (!leftShiftPressed && inputVector.x == 0f && inputVector.z == 0f)
        {
            Velocity = Mathf.MoveTowards(Velocity, 0f, runDeceleration * Time.deltaTime);
        }

        //HAREKET YONUYLE USTTEKI HIZ(Velocity) ILE VE HIZ KATSAYISI VE TIME.DELTA TIME ILE CARPARAK HAREKET ET
        //NOT: TIME.DELTATIME OYUNUN KARE SAYISINA BAGIMSIZ IS YAPMAYI SAGLAR, 1/ KARE SAYISIDIR.
        characterController.Move(inputVector * Velocity * Time.deltaTime * speedMult);
       
       

    }
    private void LookAtMouseDirection()
    {
        //ISIN VE ISIN CARPISMA BILGISINI TUTAN DEGISKENLERINI OLUSTUR
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //ISINI EKRANA CIZ
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        //RAY ISIMLI ISINI FIRLAT,CARPISMA VERISINI HIT DEGISKENINE ATA,CARPISMADA MASKE UYUSUYOR ISE DEVAM ET
        if (Physics.Raycast(ray, out hit,layerMask))
        {
            //KARAKTERI FARE POZISYONUNA DOGRU CEVIR
            mouseWorldPosition = new Vector3(hit.point.x,transform.position.y,hit.point.z);
            gameObject.transform.forward = (mouseWorldPosition - gameObject.transform.position);
        }

      

    }



}
