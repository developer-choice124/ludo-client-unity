using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Models;
using Proyecto26;

[System.Serializable]
public class BankTransfer : Requester
{
    public InputField name,accountNumber,ifscNumber,amount,branch;


    public RequestHelper BuildRequest(){

        if(name.text.Length<3){
            GameEvents.notify_event_invoke("Please enter your name!",Color.yellow);
            return null;
        }
        if(accountNumber.text.Length<9){
            
            GameEvents.notify_event_invoke("Account number should be around 9-16 digits!",Color.yellow);
            return null;
        }
        if(ifscNumber.text.Length<11){
            
            GameEvents.notify_event_invoke("Ifsc number should be 11 digits long!",Color.yellow);
            return null;
        }
         if(Convert.ToInt64(amount.text)<51){
            
            GameEvents.notify_event_invoke("redeem amount should be greater then 50!",Color.yellow);
            return null;
        }
        var body = new redeemRequest{
            paytype="account",
	        account=new Bank{
                accountname=name.text,
                accountnum=Convert.ToInt64(accountNumber.text),
                accountifsc=ifscNumber.text,
                accountbranchname= branch.text
            },
            redeemcoins=Convert.ToInt64(amount.text)
        };

        Debug.Log("bank- "+ JsonUtility.ToJson(body));

        var req = new RequestHelper{
            Uri=MainUrls.BASE_URL+"/post-repay-payment-amount",
            Body=body,
            Headers= new Dictionary<string, string>{
                {"authorization","JWT "+PlayerPrefs.GetString("token")},
            }
        };
        return req;
    }
    public void AssignResponse(Models.Response data){
        name.text=  data.data.name;
    }
}