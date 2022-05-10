using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Models;
using System.Linq;
public class roomobjdata{
    public int fee;
    public int total;
    public string objid ;
}
public class GetCoins : MonoBehaviour
{

    RequestHelper requestHelper;
    public enum ManagerMode{
        RoomsButtonManager,
        BuyButtonManager,
        CreateRoomManager
    }
    public string apiCall="get-buy-coin-list";
    
    public ManagerMode instantiateManager;
    public Transform container;
    public GameObject button_prefab;
    public int maxplayers;

    public  List<string> options=new List<string>();
    public   List<roomobjdata> roomObjIds=new List<roomobjdata>();
    
    void Start()
    {
        GameEvents.Fetch_UserData_Event+=RefreshGameData;
        if(instantiateManager==ManagerMode.BuyButtonManager){
             GetObjects(0);
        }
       
    }
    public void RemoveObjects(){
        if(instantiateManager!=ManagerMode.CreateRoomManager){
            Transform[] childs = container.Cast<Transform>().ToArray();
        childs.ToList().ForEach((t)=>{
            Destroy(t.gameObject);
        });
        }else if(instantiateManager==ManagerMode.CreateRoomManager){
            container.GetComponent<Dropdown>().options.Clear();
        }
        
    }
    void RefreshGameData(){
        if(instantiateManager==ManagerMode.BuyButtonManager){
             GetObjects(0);
        }
        
       
    }
    public void GetObjects(int players)
    {   
        if(instantiateManager!=ManagerMode.CreateRoomManager){
             container.GetComponentsInChildren<Transform>().ToList().ForEach(t=>{
            if(t!=container.transform){
                  Destroy(t.gameObject);
            }
          
        });
        }
      
        maxplayers=players;
        if(!apiCall.Contains("get-")){
            Debug.Log("wrong api address of-"+this.gameObject.name);
            return;
        }

        if(options.Count>1){
            if(instantiateManager==ManagerMode.CreateRoomManager){
                    container.GetComponent<Dropdown>().AddOptions(options);
                    return;
            }
        }
        Dictionary<string, string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));

        Debug.Log("JWT "+ PlayerPrefs.GetString("token"));
        
        requestHelper = new RequestHelper
        {
            
            Uri = MainUrls.BASE_URL + "/"+apiCall,
            Headers= headers,
        };
        RestClient.Get<Coins>(requestHelper).Then(res =>
        {

            if (res.errorvalue)
            {
                GameEvents.notify_event_invoke(res.error,Color.red);
                Debug.Log("get coins error responce");
                return;
            }
            else
            {
                Debug.Log(apiCall+" calls once");
                options.Clear();
                roomObjIds.Clear();
                int p=players;
                Debug.Log(JsonUtility.ToJson(res) );
                // Debug.Log(JsonUtility.ToJson( res.data[3]));
                res.data.ToList().ForEach((obj)=>{
                     if (obj.is_active == 1)
                    {
                        //   Debug.Log(p);
                        if (instantiateManager == ManagerMode.RoomsButtonManager)
                        {
                            // Debug.Log("loop");
                            if(p==2){
                                 GameObject btn = GameObject.Instantiate(button_prefab, container);
                            btn.GetComponent<RoomButton>().SetUp(players,obj.coins,obj.players.twoplayers.totalcoins,obj._id);
                            }
                            else
                             if(p==3){
                                 GameObject btn = GameObject.Instantiate(button_prefab, container);
                            btn.GetComponent<RoomButton>().SetUp(players,obj.coins,obj.players.threeplayers.totalcoins,obj._id);
                            }
                            else
                            if(p==4){
                                GameObject btn = GameObject.Instantiate(button_prefab, container);
                                btn.GetComponent<RoomButton>().SetUp(players,obj.coins,obj.players.fourplayers.totalcoins,obj._id);
                            }

                        }else if(instantiateManager == ManagerMode.BuyButtonManager){

                            GameObject btn = GameObject.Instantiate(button_prefab, container);
                            btn.GetComponent<BuyButton>().SetUp(obj.coins,obj.price,obj._id);

                        }else if(instantiateManager == ManagerMode.CreateRoomManager&&players!=0){
                           

                            if(p==2){
                                roomObjIds.Add(new roomobjdata{objid=obj._id,fee=obj.coins,total=obj.players.twoplayers.totalcoins});
                            options.Add(obj.coins.ToString()+" coins");
                            }
                            else
                             if(p==3){
                             roomObjIds.Add(new roomobjdata{objid=obj._id,fee=obj.coins,total=obj.players.threeplayers.totalcoins});
                            options.Add(obj.coins.ToString()+" coins");
                            }
                            else
                            if(p==4){
                                roomObjIds.Add(new roomobjdata{objid=obj._id,fee=obj.coins,total=obj.players.fourplayers.totalcoins});
                            options.Add(obj.coins.ToString()+" coins");
                                
                            }
                        }

                    }else{
                        Debug.Log("one object of reciver= "+this.name+"has set_active=false");
                    }
                });

                if(instantiateManager==ManagerMode.CreateRoomManager){
                    container.GetComponent<Dropdown>().AddOptions(options);
                }
                
            }

        }
        ).Catch(error =>{ GameEvents.notify_event_invoke(error.ToString(),Color.red);
         Debug.Log(error);});
    }
    void OnDisable(){
        GameEvents.Fetch_UserData_Event-=RefreshGameData;

    }
}
