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
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pickupable"))
        {
            gameObject.GetComponent<Stats>().currentHealth += 20;
           //gameObject.GetComponentInChildren<PlayerShootingRaycast>().currentAmmo += 20;
            gameObject.GetComponent<PlayerAmmo>().currentAmmo += 20;
            Destroy(other.gameObject);
            gameObject.GetComponent<SoundManager>().PlayPickup();
            StartCoroutine(Powerup());
        }
    }
    
    IEnumerator Powerup()
    {
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().startColor = powerupColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().endColor = powerupEndColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().shootableMask = powerupLayer;
        yield return new WaitForSeconds(10f);
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().startColor = originalColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().endColor = originalEndColor;
        gameObject.GetComponentInChildren<PlayerShootingRaycast>().shootableMask = originalLayer;
    }
}
