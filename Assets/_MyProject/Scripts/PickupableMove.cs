using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableMove : MonoBehaviour
{
    Vector3 originalPosition;
 
  
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (transform.position.x,Mathf.Sin(Time.time)+originalPosition.y+1f, transform.position.z);
       
    }
    public void SetOriginalPosition(Vector3 vector3)
    {
        originalPosition = vector3;
    }
}
