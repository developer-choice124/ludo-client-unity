using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class roomDetails : MonoBehaviour
{
   
   public Text entry,total;
    void Start()
    {
        if(total!=null&&GameClient._Instance.roomconfig.total!=null){
             total.text=GameClient._Instance.roomconfig.total.ToString();
        }
       
          if(entry!=null&&GameClient._Instance.roomconfig.roomName!=null){
              entry.text=GameClient._Instance.roomconfig.roomName;
          }
            setroomdata();

    }
    void OnEnable(){
        GameEvents.Fetch_UserData_Event_invoke();

         if(total!=null)
        total.text=PlayerPrefs.GetInt("usercoins").ToString();

    }
    public async void setroomdata(){
        
        await Task.Delay(8000);

        if(total!=null&&GameClient._Instance.roomconfig.total!=null){

             total.text=GameClient._Instance.roomconfig.total.ToString();

        }
        if(entry!=null&&GameClient._Instance.roomconfig.roomName!=null){

              entry.text=GameClient._Instance.roomconfig.roomName;

        }
        
    }
   
}
