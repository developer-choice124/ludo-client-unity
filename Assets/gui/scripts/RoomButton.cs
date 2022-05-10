using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Models;
using UnityEngine.SceneManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class RoomButton : MonoBehaviour
{
    public Text r_name,winning_price;
    RequestHelper requestHelper;
    private int maxPlayers;
    public string roomName="room";
    private string roomid,coly_roomid;
    private string total;
    public int mode=0;
    
    private bool ispressed=false;
    public GetCoins objgenerator;
    public Dropdown moneydropdown;
    public InputField roomJoinidInput;
   
    void Start(){
        
        if(mode==0){
            GetComponent<Button>().onClick.AddListener(()=>{
                if (ispressed == false)
                {
                    GameClient._Instance.roomconfig = new RoomConfig { method = "joinorcreate", maxPlayers = this.maxPlayers, roomName = this.roomName, roomid = roomid, total = total };
                    EnterRoomWaiting();
                    ispressed = true;
                }

            });
        }
        else if (mode == 1)
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (ispressed == false)
                {

                    PrepareCustomRoomDetails();
                    ispressed = true;
                }
            });
        }
        else if (mode == 2)
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (ispressed == false)
                {

                    PrepareJoinRoomDetails();
                    ispressed = true;
                }
            });
        }

    }
    public  void PrepareJoinRoomDetails(){
        string link=roomJoinidInput.text;
        List<string> results=new List<string>();
        Regex regexobj=new Regex(":.*?:");
        MatchCollection matches=regexobj.Matches(link);

     
        foreach(Match m in matches){
            results.Add(m.Value.Replace(":",""));
            Debug.LogWarning(m.Value.Replace(":",""));
        }

          
        // results.ForEach(s=>{
        //      Debug.LogWarning(s);
        // });
        coly_roomid=results[0];
        roomid=results[1];
        maxPlayers= int.Parse(results[2]);
        Debug.LogWarning("link res"+coly_roomid);
        Debug.LogWarning("link res"+roomid);
        Debug.LogWarning("link res"+maxPlayers);

        GameClient._Instance.roomconfig= new RoomConfig{method="join",col_roomid=this.coly_roomid,roomid=roomid,total=total,maxPlayers=maxPlayers};
        EnterRoomWaiting();
        
    }
    public void PrepareCustomRoomDetails(){
       roomid= objgenerator.roomObjIds[moneydropdown.value].objid;
        roomName= objgenerator.roomObjIds[moneydropdown.value].fee.ToString();
        total= objgenerator.roomObjIds[moneydropdown.value].total.ToString();
        maxPlayers=objgenerator.maxplayers;
        // SetUp(p,f,w,ri);
        showinfo(roomName,total.ToString());
        GameClient._Instance.roomconfig= new RoomConfig{method="create",maxPlayers=this.maxPlayers,roomName=this.roomName,roomid=roomid,total=total};
        EnterRoomWaiting();
    }
    public void SetUp(int players,int entry,int total,string id)
    {   
        roomid=id;

        maxPlayers=players;

        if(r_name)
        r_name.text=entry.ToString()+" Coins";

        roomName=entry.ToString();

        if(winning_price)
        winning_price.text= "win"+ total.ToString();

        this.total=total.ToString();

    }
    private void EnterRoomWaiting(){
    

        
        Dictionary<string, string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        requestHelper = new RequestHelper
        {
            
            Uri = MainUrls.BASE_URL + "/post-room-entered-coin-charges",
            Headers= headers,
            Body= new BuyCoins{
                _id=roomid
            },
        };
        Debug.Log(JsonUtility.ToJson(requestHelper.Body) );
        RestClient.Post<Response>(requestHelper).Then(res =>
        {
            Debug.Log(JsonUtility.ToJson(res));
            if(res.errorvalue){
                 GameEvents.notify_event_invoke(res.error,Color.red);
                Debug.Log("cant open payment page error while response"+JsonUtility.ToJson(res));
            }else{
                Debug.Log( "check"+JsonUtility.ToJson(res));
              if(this.maxPlayers==2){
                SceneManager.LoadScene("2players");
             }else if(this.maxPlayers==4){
                SceneManager.LoadScene("4players");
             }else if(this.maxPlayers==3){
                SceneManager.LoadScene("3players");
             }
            }
           
        }).Catch(error =>{
             Debug.Log(error);
             GameEvents.notify_event_invoke(error.ToString(),Color.red);
        });
    



        

    }
    public async void showinfo(string f,string w){
        await Task.Delay(1000);
         GameEvents.notify_event_invoke(f+ " coins will be charge after game start, "+w+" will added after win",Color.yellow);
    }
      
}
