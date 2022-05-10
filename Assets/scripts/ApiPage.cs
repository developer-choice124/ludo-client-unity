using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Proyecto26;
using UnityEngine.UI;
using System;

public class ApiPage : MonoBehaviour
{
    public InputField paynumber,upiid;
    public Dropdown Upioptions;
    public InputField redeemcoins,referralPhone,feedback;
    public GameObject waitingpage;
    RequestHelper currentRequest;
    private pages pgs;
    void Awake(){
        pgs=FindObjectOfType<pages>();
        if(paynumber)
        paynumber.text=PlayerPrefs.GetString("phone");
    }
    public void RequestRedeem()
    {
        if(upiid.text.Length < 3){
            GameEvents.notify_event_invoke(" Please enter your UPIID to make redeem.",Color.red);
            return;
        }
         if(paynumber.text.Length < 10){
            GameEvents.notify_event_invoke(" Your phone number should be 10 digits long.",Color.red);
            return;
        }
        waitingpage.SetActive(true);
        Debug.Log("redeem clicked");
        Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
         headers.Add("content-type","application/json");

        currentRequest=new RequestHelper{
            Headers=headers,
            Uri=MainUrls.BASE_URL+"/post-repay-payment-amount",
            Body=new redeemRequest{
                paynumber= Convert.ToInt64(paynumber.text),
                redeemcoins= Convert.ToInt64(redeemcoins.text),
	        	paytype=Upioptions.options[Upioptions.value].text,
                payupiid=upiid.text
                 
            }
            
        };
        // Debug.Log(paynumber.text);
        // Debug.Log("coins"+Convert.ToInt64(redeemcoins.text));
        
       
        Debug.Log("number"+ Convert.ToInt64(paynumber.text));
        RestClient.Post<Response>(currentRequest).Then(res=>{
            Debug.Log(res.errorvalue);
              Debug.Log("under res");
            if(res.errorvalue==true){
                GameEvents.notify_event_invoke("failed: "+res.error,Color.red);
                Debug.Log("error while redeem");
                 waitingpage.SetActive(false);
            }else{ 
                waitingpage.SetActive(false);
                foreach(GameObject g in pgs.redeemhistory){
                    g.SetActive(true);
                }
                 foreach(GameObject g in pgs.redeem){
                    g.SetActive(false);
                }
                 GameEvents.notify_event_invoke(res.data.status+ ": this should take 1-2 working days to reflect into your paytm account",Color.green);
                 GameEvents.Fetch_UserData_Event_invoke();
            }
        }).Catch( err =>{
             waitingpage.SetActive(false);
             GameEvents.notify_event_invoke(err.ToString(),Color.green);
        } );


    }
	public void RequestRedeemBank()
	{
		if(upiid.text.Length < 3){
			GameEvents.notify_event_invoke(" Please enter your UPIID to make redeem.",Color.red);
			return;
		}
		if(paynumber.text.Length < 10){
			GameEvents.notify_event_invoke(" Your phone number should be 10 digits long.",Color.red);
			return;
		}
		waitingpage.SetActive(true);
		Debug.Log("redeem clicked");
		Dictionary<string,string> headers=new Dictionary<string, string>();
		headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
		headers.Add("content-type","application/json");

		currentRequest=new RequestHelper{
			Headers=headers,
			Uri=MainUrls.BASE_URL+"/post-repay-payment-amount",
			Body=new redeemRequest{
			paynumber= Convert.ToInt64(paynumber.text),
			redeemcoins= Convert.ToInt64(redeemcoins.text),
			paytype=Upioptions.options[Upioptions.value].text,
			payupiid=upiid.text
                 
            }
            
        };
		// Debug.Log(paynumber.text);
		// Debug.Log("coins"+Convert.ToInt64(redeemcoins.text));
        
       
		Debug.Log("number"+ Convert.ToInt64(paynumber.text));
		RestClient.Post<Response>(currentRequest).Then(res=>{
			Debug.Log(res.errorvalue);
			Debug.Log("under res");
			if(res.errorvalue==true){
				GameEvents.notify_event_invoke("failed: "+res.error,Color.red);
				Debug.Log("error while redeem");
				waitingpage.SetActive(false);
			}else{ 
				waitingpage.SetActive(false);
				foreach(GameObject g in pgs.redeemhistory){
					g.SetActive(true);
				}
				foreach(GameObject g in pgs.redeem){
					g.SetActive(false);
				}
				GameEvents.notify_event_invoke(res.data.status+ ": this should take 1-2 working days to reflect into your paytm account",Color.green);
				GameEvents.Fetch_UserData_Event_invoke();
			}
		}).Catch( err =>{
			waitingpage.SetActive(false);
			GameEvents.notify_event_invoke(err.ToString(),Color.green);
		} );


	}
    public void PostReferral()
    {
          waitingpage.SetActive(true);
          Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        
        currentRequest= new RequestHelper{
            Headers=headers,
            Uri=MainUrls.BASE_URL+"/share-repay-by-coin-request",
            Body=new Phone{
                phone=referralPhone.text
            }
        };
        RestClient.Post<Response>(currentRequest).Then(res=>{
            if(res.errorvalue){
                  waitingpage.SetActive(false);
                GameEvents.notify_event_invoke(res.error,Color.red);
            }else{
                  waitingpage.SetActive(false);
                GameEvents.notify_event_invoke("successfully used referral number",Color.green);
               GameEvents.Fetch_UserData_Event_invoke();
            }
            
        }).Catch(err=>{
              waitingpage.SetActive(false);
        } );
        
    }
    public void Postfeedback()
    {
          waitingpage.SetActive(true);
          Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        if(!feedback)
         return;
        currentRequest= new RequestHelper{
            Headers=headers,
            Uri=MainUrls.BASE_URL+"/feedback-user",
            Body=new msg{
                message=feedback.text
            }
        };
        RestClient.Post<Response>(currentRequest).Then(res=>{
            if(res.errorvalue){
                waitingpage.SetActive(false);
                GameEvents.notify_event_invoke(res.error,Color.red);
            }else{
                waitingpage.SetActive(false);
                GameEvents.notify_event_invoke("successfully submited feedback",Color.green);
                foreach(GameObject g in pgs.aboutpage){
                    g.SetActive(true);
                }
                foreach(GameObject g in pgs.feedback){
                    g.SetActive(false);
                }
                
            }
            
        }).Catch(err=>{
            GameEvents.notify_event_invoke(err.ToString(),Color.red);
              waitingpage.SetActive(false);
        } );
        
    }
}
