using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
   
    void Start()
    {
      
        SetQuality(PlayerPrefs.GetInt("graphics"));
    }

    // Update is called once per frame
    public void SetQuality(int q)
    {
        // switch(q){
        //     case 0:
        //     ppv.weight=0;
        //     break;
        //     case 1:
        //     ppv.weight=1;

        //     break;
        //     case 2:

        //     break;
        // }
    }
}
