using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class Stats : MonoBehaviour
{
    public float currentHealth;
    public float minHealth;
    public float maxHealth;

    public HealthBar myHealthBar;

    public GameObject ammoObjectPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        myHealthBar = gameObject.GetComponentInChildren<HealthBar>();
        myHealthBar.setMaxSliderValue(maxHealth);
        myHealthBar.setSliderValue(currentHealth);

        //OLUSTUGU ANDA TAGI ENEMY ISE KALAN DUSMAN SAYISINI ARTTIR
        if (CompareTag("Enemy"))
        {
            ObjectSpawner.remainingEnemies++;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        ClampHealth();
    }
    //CAN DEGERININ LIMITLERINI AYARLAR
    void ClampHealth()
    {
        if (currentHealth <= minHealth)
        {
            currentHealth = minHealth;
            gameObject.SetActive(false);

            //EGER CAN BITMIS ISE VE TAGI ENEMY ISE DUSMAN SAYISINI AZALT
            if(CompareTag("Enemy"))
            {
                ObjectSpawner.remainingEnemies--;

                //BELIRLI BIR SANS ILE ITEM DUSUR
                if(Random.Range(0,4) == 0)
                {
                    GameObject ammoObject = GameObject.Instantiate(ammoObjectPrefab);
                    ammoObject.transform.position = transform.position;
                    ammoObject.GetComponent<PickupableMove>().SetOriginalPosition(transform.position);
                }
            }

            Debug.Log(gameObject.transform.name + " died");
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }
    //HASAR ALMAYI SAGLAR
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        myHealthBar.setSliderValue(currentHealth);
    }
}
