using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableMove : MonoBehaviour
{
    Vector3 originalPosition;
 
  
    //OYUNCU TARAFINDAN ALINABILIR OBJEYI YUKARI ASAGIYA HAREKET ETTIR
    void Update()
    {
        transform.position = new Vector3 (transform.position.x,Mathf.Sin(Time.time)+originalPosition.y+1f, transform.position.z);
       
    }
    public void SetOriginalPosition(Vector3 vector3)
    {
        originalPosition = vector3;
    }
}
