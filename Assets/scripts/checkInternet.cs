using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkInternet : MonoBehaviour
{
    public void checkBOOL()
    {
       
        if( NativeInternet.IsNetworkAvailable){
           GameEvents.notify_event_invoke("internet available",Color.green);
           NativeInternet.ShowToast("internet available");
       }else{
            NativeInternet.ShowToast("internet not available");
           GameEvents.notify_event_invoke("net not available",Color.red);
           
       }
    }
   
    public void ShowToast(string MSG)
    {

        NativeInternet.ShowToast(MSG);
    }

    public void checkboolstatic()
    {
      
       if(NativeInternet.IsNetworkAvailableStatic){
           GameEvents.notify_event_invoke("internet available",Color.green);
       }else{
           GameEvents.notify_event_invoke("internet not available",Color.red);
           
       }
    }
}
