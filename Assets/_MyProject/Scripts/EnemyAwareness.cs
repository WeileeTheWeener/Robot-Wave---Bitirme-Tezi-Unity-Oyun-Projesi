using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAwareness : MonoBehaviour
{
    public static List<GameObject> enemyList;
    public EnemyStateManager stateManager;
    public float alertRadius;

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
    //DUSMANI LISTEYE EKLE
    public void Awake()
    {
        if(enemyList == null)
        {
            enemyList = new List<GameObject>();
           
        }

        enemyList.Add(gameObject);
    }
    //SUANKI DURUMU ATTACKSTATE YAP
    public void AlertEnemies()
    {
        gameObject.transform.root.GetComponentInChildren<EnemyStateManager>().currentState = gameObject.transform.root.GetComponentInChildren<EnemyAttackState>();

        foreach (GameObject enemy in enemyList)
        {
            if(enemy.active == false)
            {
                continue;
            }

            //EGER ARADAKI MESAFE KUCUK ISE LISTEDEKI DUSMANLARI SALDIRI DURUMUNA GECIR
            if(Vector3.Distance(enemy.transform.position,transform.position) < alertRadius)
            {
                enemy.transform.root.GetComponentInChildren<EnemyStateManager>().currentState = enemy.transform.root.GetComponentInChildren<EnemyAttackState>();

            }

            

        }
        Debug.Log("Im alert");
    }

}
