using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Color originalColor;
    private Color originalEndColor;
    public Color powerupColor;
    public Color powerupEndColor;
    private LayerMask originalLayer;
    public LayerMask powerupLayer;
    

    private void Start()
    {
        originalColor = gameObject.GetComponentInChildren<PlayerShootingRaycast>().startColor;
        originalEndColor = gameObject.GetComponentInChildren<PlayerShootingRaycast>().endColor;
        originalLayer = gameObject.GetComponentInChildren<PlayerShootingRaycast>().shootableMask;
    }
    //YERDEN OBJEYI ALMIS ISE CAN,MERMI VE POWERUP FONKSIYONUNU CALISTIR
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pickupable"))
        {
            gameObject.GetComponent<Stats>().currentHealth += 20;
            gameObject.GetComponent<PlayerAmmo>().currentAmmo += 20;          
            Destroy(other.gameObject);
            gameObject.GetComponent<SoundManager>().PlayPickup();
            StartCoroutine(Powerup());
        }
    } 
    //OYUNCU MERMI IZI RENGINI DEGISTIR VE MERMININ DUVARDAN GECMESINI SAGLA
    IEnumerator Powerup()
    {
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().startColor = powerupColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().endColor = powerupEndColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().shootableMask = powerupLayer;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().damage += 10;
        yield return new WaitForSeconds(10f);
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().startColor = originalColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().endColor = originalEndColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().shootableMask = originalLayer;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().damage -= 10;
    }
}
