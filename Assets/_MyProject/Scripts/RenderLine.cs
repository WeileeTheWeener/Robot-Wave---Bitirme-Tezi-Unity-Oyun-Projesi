using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3[] points;
    public float size;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        points = new Vector3[4];

    }

    // Update is called once per frame
    void Update()
    {
        DrawLines();
    }
    void DrawLines()
    {
             
            points[0] = (new Vector3(gameObject.transform.position.x - size / 2, gameObject.transform.position.y, gameObject.transform.position.z + size));
            points[1] = (new Vector3(gameObject.transform.position.x + size / 2, gameObject.transform.position.y, gameObject.transform.position.z + size));
            points[2] = (new Vector3(gameObject.transform.position.x + size / 2, gameObject.transform.position.y, gameObject.transform.position.z - size));
            points[3] = (new Vector3(gameObject.transform.position.x - size / 2, gameObject.transform.position.y, gameObject.transform.position.z - size));
       

         lineRenderer.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }



    }
}
