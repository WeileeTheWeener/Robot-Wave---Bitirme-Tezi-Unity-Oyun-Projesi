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
    //public int currentAmmo;
    //public int maxMagazines;
    //public int currentMagazines;
    //public int maxAmmo;
    public bool shootingPreviousFrame;
    //public Text ammoText;
    private Animator animator;
    private Vector3 hitPointOfBarrel;
    private Canvas crosshairCanvas;
    private Vector3 mousePos;
    public GameObject muzzleFlashSpriteObject;
    public GameObject muzzleFlashLightObject;
    public Color startColor;
    public Color endColor;

    // Start is called before the first frame update
    void Start()
    {

        shootingPreviousFrame = false;
        //currentAmmo = maxAmmo;
        //currentMagazines = maxMagazines; currentAmmo > 0 
        animator = gameObject.transform.root.GetComponent<Animator>();
        soundManager = gameObject.transform.root.GetComponent<SoundManager>();
        playerAmmo = gameObject.transform.root.GetComponent<PlayerAmmo>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePos();
        bool shootingThisFrame = false;

 
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
            //ReloadMagazine();
            playerAmmo.ReloadMagazine();
        }
        //ammoText.text = currentMagazines + "/" + currentAmmo;
        //playerAmmo.ammoText.text = playercurrentMagazines + "/" + currentAmmo;

        shootingPreviousFrame = shootingThisFrame;
    }
    void Shoot()
    {
        ray = new Ray(gameObject.transform.position, gameObject.transform.forward);

        Vector3 dirr = (ray.GetPoint(range) - barrel.transform.position).normalized;
        hitPointOfBarrel = barrel.transform.position + dirr * range;


        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        
        if (Physics.Raycast(ray, out hit,range,shootableMask))
        {
                    
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {                
                Debug.Log("we hit an enemy");
                hit.transform.GetComponent<Stats>().TakeDamage(damage);               
            }
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles") && hit.collider != null)
            {
                hitPointOfBarrel = barrel.transform.position + dirr * hit.distance;
            }
         
        }

        RaycastHit alertHit;

        
        if (Physics.Raycast(ray, out alertHit, range,alertMask))
        {
            alertHit.transform.GetComponent<EnemyAwareness>().AlertEnemies();
            Debug.Log(alertHit.transform.gameObject.name);
        
        }


        BulletTrailUtil.PlayTrailAnimation(
        barrel.transform.position, 
        hitPointOfBarrel,
        startColor,
        endColor);

        //currentAmmo -= 1;
        playerAmmo.setCurrentAmmo(playerAmmo.currentAmmo - 1);

    }
    /*void ReloadMagazine()
    {
        if (currentMagazines > 0)
        {
            currentAmmo = maxAmmo;
            currentMagazines -= 1;
            soundManager.PlayReload();
            
        }
    
    }*/
    void GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            mousePos = new Vector3(hit.point.x,gameObject.transform.position.y,hit.point.z);
        }
    }
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
