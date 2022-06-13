using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootingRaycast : MonoBehaviour
{
    
    public Transform barrel;
    public SoundManager soundManager;
    private RaycastHit hit;
    private Ray ray;
    public LayerMask alertMask;
    public LayerMask shootableMask;
    public PlayerAmmo playerAmmo;

    public float fireRate;
    public float nextTimeToFire = 0f;
    public float range;
    public float damage;
    public bool shootingPreviousFrame;
    private Animator animator;
    private Vector3 hitPointOfBarrel;
    private Canvas crosshairCanvas;
    private Vector3 mousePos;
    public GameObject muzzleFlashSpriteObject;
    public GameObject muzzleFlashLightObject;
    public Color startColor;
    public Color endColor;
    private AnimationBlendTreeController treeController;
    bool reloadPressed;

    void Start()
    {

        shootingPreviousFrame = false;
        animator = gameObject.transform.root.GetComponent<Animator>();
        soundManager = gameObject.transform.root.GetComponent<SoundManager>();
        playerAmmo = gameObject.transform.root.GetComponent<PlayerAmmo>();
        treeController = gameObject.transform.root.GetComponent<AnimationBlendTreeController>();
    }
    void Update()
    {

        GetMousePos();
        bool shootingThisFrame = false;

        //ATES ET
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && playerAmmo.currentAmmo > 0 && (animator.GetLayerWeight(1) == 1 || animator.GetLayerWeight(2) == 1 || animator.GetLayerWeight(3) == 1) && animator.GetLayerWeight(4) ==0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            soundManager.PlayGunShot();
            

            if(animation !=null)
            {
                StopCoroutine(animation);
            }
            
            animation = (PlayMuzzleParticle());
            StartCoroutine(animation);
        }
        if (Input.GetButton("Fire1") && playerAmmo.currentAmmo > 0 && (animator.GetLayerWeight(1) == 1 || animator.GetLayerWeight(2) == 1 || animator.GetLayerWeight(3) == 1) && animator.GetLayerWeight(4) == 0)
        {
            shootingThisFrame = true;
        }
        if (shootingPreviousFrame && !shootingThisFrame)
        {
            soundManager.PlayGunDecharge();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(waitForReload());
        }
        //MERMÝYÝ ANÝMASYONUN SONUNDA OYNATIOMUS GIBI OLMASI ICIN 1 SN BEKLE
        IEnumerator waitForReload() 
        {
            yield return new WaitForSeconds(1f);
            playerAmmo.ReloadMagazine();
        }
     

        shootingPreviousFrame = shootingThisFrame;
    }
    //RAYCAST ILE ATIS YAP
    void Shoot()
    {
        //TAKILI OLAN OBJEDEN ILERI DUZ BIR CIZGI YARAT
        ray = new Ray(gameObject.transform.position, gameObject.transform.forward);

        //GERCEK MERMI YONU ILE NAMLU ARASINDAKI YONU AL
        Vector3 dirr = (ray.GetPoint(range) - barrel.transform.position).normalized;

        //NAMLUDAN CIKAN MERMI IZININ GIDECEGI SON POZISYON
        hitPointOfBarrel = barrel.transform.position + dirr * range;


        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        
        if (Physics.Raycast(ray, out hit,range,shootableMask))
        {
            //EGER DUSMANA CARPTIYSA HASAR VER       
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {                
                Debug.Log("we hit an enemy");
                hit.transform.GetComponent<Stats>().TakeDamage(damage);               
            }
            //EGER ENGELE CARPTIYSA ENGELDE IZI KES
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles") && hit.collider != null)
            {
                hitPointOfBarrel = barrel.transform.position + dirr * hit.distance;
            }
         
        }

        //EGER DUSMANIN YAKININA ATES ETMIS ISEK DUSMANLARI ALARM DURUMUNA GECIR
        RaycastHit alertHit;
        if (Physics.Raycast(ray, out alertHit, range,alertMask))
        {
            alertHit.transform.GetComponent<EnemyAwareness>().AlertEnemies();
            Debug.Log(alertHit.transform.gameObject.name);
        
        }

        //MERMI IZI OLUSTUR
        BulletTrailUtil.PlayTrailAnimation(
        barrel.transform.position, 
        hitPointOfBarrel,
        startColor,
        endColor);

        //ANLIK MERMIYI AYARLA
        playerAmmo.setCurrentAmmo(playerAmmo.currentAmmo - 1);

    }
    
    //FARE POZISYONU AL
    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePos = new Vector3(hit.point.x,gameObject.transform.position.y,hit.point.z);
        }
    }
    //NAMLU ISIGI OYNAT
    IEnumerator PlayMuzzleParticle()
    {
        muzzleFlashLightObject.SetActive(true);
        muzzleFlashSpriteObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlashLightObject.SetActive(false);
        muzzleFlashSpriteObject.SetActive(false);
    }
    IEnumerator animation;
   

}
