using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class ShapeFillRotater : MonoBehaviour
{
    public Shape shape;
    public float speed=.2f;

    void Start()
    {
        
    }

    // Update is called once per frame
   private void LateUpdate() {
       float fill = shape.settings.fillRotation;
       shape.settings.fillRotation=fill+speed;
       if( Mathf.Abs(fill)>359){
           shape.settings.fillRotation=0;
       }
   }
}
