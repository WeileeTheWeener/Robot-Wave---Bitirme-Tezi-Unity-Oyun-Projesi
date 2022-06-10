using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Canvas crosshairCanvas;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        crosshairCanvas = gameObject.transform.parent.GetComponent<Canvas>();
        player = GameObject.Find("Player");
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //crosshairCanvas.transform.position = new Vector3(hit.point.x,player.transform.position.y,hit.point.z);
            crosshairCanvas.transform.position = new Vector3(hit.point.x,player.transform.position.y,hit.point.z);
        }
    }
}

