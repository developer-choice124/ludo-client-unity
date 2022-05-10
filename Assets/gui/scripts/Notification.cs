using UnityEngine;
using UnityEngine.UI;


public class Notification : MonoBehaviour
{   

    public enum NotificationMode{
        dialogbox,
        topslide

    }
    public NotificationMode mode;
    Animator anim;
    AudioSource source;
    public Text MsgTxt;
   
    public Image icon;
    void Awake()
    {
        source=GetComponent<AudioSource>();
        anim= GetComponent<Animator>();
        if(mode==NotificationMode.topslide)
        GameEvents.notify_event+=topslide;

        if(mode==NotificationMode.dialogbox)
         GameEvents.ondialog_event+=dialogbox;
    }
    void Start(){
        GameEvents.notify_event_invoke("welcome "+PlayerPrefs.GetString("username"),Color.blue);
    }
    void topslide(string msg, Color color){
        MsgTxt.text=msg;
        icon.color=color;
        anim.SetTrigger("pop");
    }
    void dialogbox(string msg){
        MsgTxt.text=msg;
       Debug.Log("dialog");
        anim.SetTrigger("pop");


    }
    void OnDisable(){
         if(mode==NotificationMode.topslide)
         GameEvents.notify_event-=topslide;

          if(mode==NotificationMode.dialogbox)
         GameEvents.ondialog_event-=dialogbox;
    }
}
