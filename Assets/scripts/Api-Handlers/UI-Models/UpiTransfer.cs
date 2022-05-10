using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Models;
using Proyecto26;
using UnityEngine.Events;



[System.Serializable]
public class UpiTransfer :Requester
{
    public InputField paynumber,upiid,redeemcoins;
    public Dropdown Upioptions;

    public RequestHelper BuildRequest(){

        var body = new redeemRequest{
            paytype=Upioptions.options[Upioptions.value].text,
            paynumber= Convert.ToInt64(paynumber.text),
            redeemcoins= Convert.ToInt64(redeemcoins.text),
            payupiid=upiid.text
        };

        if(body.payupiid.Length<3){
            GameEvents.notify_event_invoke("Invalid upi id!",Color.red);
            return null;
        }
         if(body.paynumber<10){
            GameEvents.notify_event_invoke("Invalid number!",Color.red);
            return null;
        }
          if(Convert.ToInt64(redeemcoins.text)<51){
            
            GameEvents.notify_event_invoke("redeem amount should be greater then 50!",Color.yellow);
            return null;
        }
        var req = new RequestHelper{
            Uri=MainUrls.BASE_URL+"/post-repay-payment-amount",
            Body=body,
            Headers = new Dictionary<string, string>{
                {"authorization","JWT "+PlayerPrefs.GetString("token")},
                {"content-type","application/json"}
            }
        };
        return req;
    }
    public void AssignResponse(Models.Response data){
        // name.text=  data.data.name;
    }
    public void Execute(){
        RestClient.Post<Response>(BuildRequest()).Then(result =>{
            Debug.Log((result));
        }).Catch( err =>{
            Debug.Log((err));
        });
    }
}