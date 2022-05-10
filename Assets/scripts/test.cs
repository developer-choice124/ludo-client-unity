using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= new Vector3(Mathf.SmoothStep(0,10,.2f),0,0);
    }
}
