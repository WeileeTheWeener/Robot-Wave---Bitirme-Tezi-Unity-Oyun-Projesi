using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform Cam;
    
    void Start()
    {
        //SAHNEDEKI ANA KAMERAYI AL
        Cam = Camera.main.transform;
        
    }

   
    void LateUpdate()
    {
        //TAKILI OBJENIN ÝLERÝ EKSENÝNÝ KAMERAYA BAKICAK SEKILDE AYARLA
        transform.LookAt(transform.position + Cam.transform.forward);
    }
}
