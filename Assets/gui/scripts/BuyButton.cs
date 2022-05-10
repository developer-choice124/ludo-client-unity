using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Models;

public class BuyButton : MonoBehaviour
{
    public Text price,coins;
    public Button ButyBtn;
    private RequestHelper requestHelper;
    private string objectId;
    private webviewmanager wm;

    void Start(){
        wm=FindObjectOfType<webviewmanager>();
        ButyBtn.onClick.AddListener(()=>BuyCoins());
    }
    public void SetUp(int c,int p,string _id)
    {
        
       
        objectId=_id;
        coins.text= c.ToString();
        price.text="₹"+p.ToString();
    }

    private void BuyCoins(){
        Debug.Log(objectId);

        Dictionary<string, string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        requestHelper = new RequestHelper
        {
            
            Uri = MainUrls.BASE_URL + "/buy-coins",
            Headers= headers,
            Body= new BuyCoins{
                _id=objectId
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
                string url=MainUrls.BASE_URL+"/make-payment/" +res.data.orderID;
                Debug.Log( "url"+url);
                wm.CreateWebView(url);
            }
           
        }).Catch(error =>{
             Debug.Log(error);
             GameEvents.notify_event_invoke(error.ToString(),Color.red);
        });
    }



   
}
