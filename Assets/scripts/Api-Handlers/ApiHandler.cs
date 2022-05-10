using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Proyecto26;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

[Serializable]
public class Requester {
    public UnityEvent onSuccess;
    public UnityEvent onFailed;
    public virtual RequestHelper BuildRequest(){
        return null;
    }
}
public class ApiHandler : MonoBehaviour
{

   
    [Header("UI-Setup"),Space()]
	public UpiTransfer upiTransfer =new UpiTransfer();
	public BankTransfer bankTransfer = new BankTransfer();

    public ReferralSubmit referralSubmit= new ReferralSubmit();

    
    public void RedeemBank()
    {
        var req = bankTransfer.BuildRequest();
        if(req==null){
            Debug.LogWarning("Redeem bank request is invalid!");
            return;
        }
        RestClient.Post<Response>(req).Then(result =>{
            Debug.Log(JsonUtility.ToJson(result));
            bankTransfer.onSuccess.Invoke();
        }).Catch(err =>{
            bankTransfer.onFailed.Invoke();
            Debug.Log(err);
        });
        
    }
    public void RedeemUpi()
    {   
        var req =upiTransfer.BuildRequest();
        if(req==null){
            Debug.LogWarning("Redeem request is invalid!");
            return;
        }
        RestClient.Post<Response>(req).Then(result =>{
            Debug.Log(JsonUtility.ToJson(result));
            upiTransfer.onSuccess.Invoke();
        }).Catch(err =>{
            upiTransfer.onFailed.Invoke();
            Debug.Log(err);
        });
        
    }
    public void ReferralSubmit()
    {
        RestClient.Post<Response>(referralSubmit.BuildRequest()).Then(result =>{
            Debug.Log(JsonUtility.ToJson(result));
        }).Catch(err =>{
            Debug.Log(err);
        });
        
    }
}
