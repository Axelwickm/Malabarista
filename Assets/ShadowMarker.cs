using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMarker : MonoBehaviour
{
    Rigidbody rg;
    Vector3 v3;
    RaycastHit hit;

    //public GameObject shadow;

    LineRenderer line; 

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        line.SetWidth(0.0f, 0.0f);
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position - new Vector3(0, 100, 0));
        if (rg.velocity.y < -0)
        {
            line.SetWidth((0.005f * rg.transform.position.y), (0.005f*rg.transform.position.y)); // 0.01f
            
        
        }

       
    }
}
