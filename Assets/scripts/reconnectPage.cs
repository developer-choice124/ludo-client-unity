using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
using System.Linq;
using UnityEngine.UI;
public class reconnectPage : MonoBehaviour
{
    RequestHelper requestHelper;
    public Text msg,buttonText;
    public Button enter;

    void Start()
    {
        Debug.LogWarning(PlayerPrefs.GetInt("gameover")+ "= game over");

        if(PlayerPrefs.HasKey("gameover")){
            if(PlayerPrefs.GetInt("gameover")==0){
                // CheckLastGame();
             this.gameObject.SetActive(true);
            }else{
                this.gameObject.SetActive(false);
            }
        }else{
                this.gameObject.SetActive(false);
        }
      
    }

    public void CheckLastGame(){
         Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
            requestHelper = new RequestHelper
            {
                Uri = MainUrls.BASE_URL + "/" + "get-transaction-list-by-user",
                Headers = headers,
            };
            RestClient.Get<Transactions>(requestHelper).Then(res =>
            {   
                List<Transaction> list= res.data.ToList();
                list.Reverse();
            //    && list[0].date_modified!="Invalid Date"
                if(list[0].coly_room_id == PlayerPrefs.GetString("previous_room_id") && list[0].date_modified!="Invalid Date"){
                    Debug.LogWarning(list[0].coly_room_id+"is game history id and "+PlayerPrefs.GetString("previous_room_id")+" is previous id");
                    this.gameObject.SetActive(true);
                   
                    if(list[0].winnerId==PlayerPrefs.GetString("_id")){
                        msg.text= "Congratulation you have won your previous match";
                    }else{
                        msg.text= " Unfortunately you have lost your previous match";
                    }
                    buttonText.text="Ok";
                    enter.onClick.RemoveAllListeners();
                    enter.onClick.AddListener(()=>{
                        this.gameObject.SetActive(false);
                    });

                }else{
                    this.gameObject.SetActive(true);
                    enter.gameObject.SetActive(true);
                    msg.text= " click Enter button to enter your previous match agian.";
                     buttonText.text="Enter";
                    enter.onClick.RemoveAllListeners();
                    enter.onClick.AddListener(()=>{
                        settings._Instance.reconnectgame();
                    });
                }
            }).Catch(err =>
            {
                GameEvents.notify_event_invoke(" cant load transactions at the moment", Color.red);
            });
    }
    public  void setgameover()
    {
        
        if(PlayerPrefs.HasKey("gameover")){
           PlayerPrefs.SetInt("gameover",1);
        }
        
    }
}
