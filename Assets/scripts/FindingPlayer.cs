using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Colyseus;
public class FindingPlayer : MonoBehaviour
{
    public User user1,user2,user3,user4;
    public Sprite emptyslot;
    public utils userimages;
    public InputField roomid;
    public Text finding,timecount,onlinecount;
    private List<User> users= new List<User>();
    public GameObject cancelBtn,tryBotPage;
    
    float timec;

    void Start()
    {   
        
        tryBotPage.SetActive(false);
        
      
        // InvokeRepeating("OnlinePlayers",3f,1);
        users.Add(user2);users.Add(user3);users.Add(user4);
         enablecancel();
        
        if(GameClient._Instance.roomconfig.maxPlayers==2){
            user1.gameObject.SetActive(true);
            user2.gameObject.SetActive(false);
            user3.gameObject.SetActive(true);
            user4.gameObject.SetActive(false);
        }
        if(GameClient._Instance.roomconfig.maxPlayers==3){
            user1.gameObject.SetActive(true);
            user2.gameObject.SetActive(true);
            user3.gameObject.SetActive(true);
            user4.gameObject.SetActive(false);

        }
        if(GameClient._Instance.roomconfig.maxPlayers==4){
            user1.gameObject.SetActive(true);
            user2.gameObject.SetActive(true);
            user3.gameObject.SetActive(true);
            user4.gameObject.SetActive(true);

        }
        user1.profile.sprite=userimages.sprites[PlayerPrefs.GetInt("userprofile")];
        user1.Name.text=PlayerPrefs.GetString("username");
       
        if(GameClient._Instance.roomconfig.method=="create"){
            finding.text="waiting...";
        
            
        }else{
             roomid.text="#########";
        }
      
    }
    public async void OnSceneLoaded(Scene scene, LoadSceneMode mode){

        await Task.Delay(3000);
        if(GameClient._Instance.roomconfig.method=="create"){
             roomid.text =(":"+GameClient._Instance.roomconfig.col_roomid+":"+":"+GameClient._Instance.roomconfig.roomid+":"+":"+GameClient._Instance.roomconfig.maxPlayers.ToString()+":");
        }
       
        // GameClient._Instance.roomconfig.method="";
    }  
    void Update(){
        if(timecount!=null){
            timec+=Time.deltaTime;
	        if(timec>=MainUrls.FOUND_TIME_LIMIT){
                playernotfound();
            }
            timecount.text=Mathf.RoundToInt(timec).ToString();
        }
    }
    public  void userjoin (int action,int uprofile,string uname){
        if (action == 1)
        {
            foreach (User u in users)
            {

                if (u.Name.text == "finding..")
                {
                    u.profile.sprite = userimages.sprites[uprofile];
                    u.Name.text = uname;

                    break;
                }
            }
        }
        else if (action == 0)
        {
              foreach (User u in users)
            {

                if (u.Name.text == uname)
                {
                    u.profile.sprite = userimages.sprites[uprofile];
                    u.profile.sprite = emptyslot;
                    u.Name.text = "finding..";

                    break;
                }
            }

        }


    }
    void OnDisable(){
             GameEvents.Userjoin_Event-=userjoin;
                SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnEnable(){
         GameEvents.Userjoin_Event+=userjoin;
        SceneManager.sceneLoaded += OnSceneLoaded;
        

    }
    async void enablecancel(){
        if(cancelBtn!=null){
            cancelBtn.SetActive(false);
        await  Task.Delay(10000);
            cancelBtn.SetActive(true);
        }
        
    }
    public void playernotfound(){
        if(GameClient._Instance.lobby.Count<2){
            tryBotPage.SetActive(true);

            // settings._Instance.playernotfound(0);
            timec=0;
        }
        
    } 
    public void OnlinePlayers(){
        
    } 
}
