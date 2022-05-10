using UnityEngine;
public static class MyGameEvents{
    public  delegate void OnUIdata();
    public static event OnUIdata Refresh_UIData_Event; 
    public  delegate void OnUserdata();
    public static event OnUserdata Fetch_UserData_Event; 
    public  delegate void OnUserjoin(int number,int uprofile,string uname);
    public static event OnUserjoin Userjoin_Event; 
    public  delegate void OnNotification(string msg ,Color clr);
    public  delegate void OnDialogBox(string msg);
    public static event OnDialogBox ondialog_event;
    public static event OnNotification notify_event;

    public static void notify_event_invoke(string msg ,Color clr){
        notify_event(msg,clr);
        
        
    }
    public static void Refresh_UIData_Event_invoke(){
        Refresh_UIData_Event();
    }
    public static void Fetch_UserData_Event_invoke(){
        Fetch_UserData_Event();
    }
    public static void invoke_dialogbox(string msg){
            ondialog_event(msg);
    }
    public static void invoke_user_join_event(int number,int uprofile,string uname){
        Userjoin_Event(number,uprofile,uname);
    }

}