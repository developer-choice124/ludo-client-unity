using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using UnityEngine.UI;
using Models;


public class RegisterManeger : MonoBehaviour
{
    

    RequestHelper currentRequest;
    public Button sumbitPhn, otpVarify,sumbitDetails;
    public Text resendDiscription;
    public GameObject[] verificationPages;
    public GameObject logincanvas;
    public Dropdown Userprofile;
    public InputField phoneInput, otpInput, u_nameInput, u_fullnameInput;

     SceneHandler sceneHandler;
    
    void Start()
    {
        sceneHandler= FindObjectOfType<SceneHandler>();
        CheckRegisterdUser();

        phoneInput.onValueChanged.AddListener(delegate {IntracktableButtonByInputLimit(phoneInput,10,sumbitPhn);});
        otpInput.onValueChanged.AddListener(delegate {IntracktableButtonByInputLimit(otpInput,6,otpVarify);});
        // u_fullnameInput.onValueChanged.AddListener(delegate {IntracktableButtonByInputLimit(u_fullnameInput,10,sumbitDetails);});
        // u_nameInput.onValueChanged.AddListener(delegate {IntracktableButtonByInputLimit(u_nameInput,10,sumbitDetails);});

        sumbitPhn.onClick.AddListener(() => SubmitPhone());
        otpVarify.onClick.AddListener(()=>SubmitOtp());
        sumbitDetails.onClick.AddListener(()=>SumbitDetails());

    }
    private void IntracktableButtonByInputLimit(InputField input,int i,Button btn){
        input.characterLimit=i;
        if( input.text.Length==i){
            btn.interactable=true;
        }else{
            btn.interactable=false;
        }
    }
    private void SubmitPhone()
    {
        PostPhone(phoneInput.text);
    }
    private void SubmitOtp(){

        string id= PlayerPrefs.GetString("_id");
        string phone= PlayerPrefs.GetString("phone");
        string otp=otpInput.text;
        if(id!=null&&phone!=null){
             PostOTP(id,phone,otp);
        }
       
    }
    private void SumbitDetails(){
        PostDetails(u_fullnameInput.text,u_nameInput.text,Userprofile.value);
    }
    private void PostPhone(string phoneN) 
    {
        //setting phone number post request for LUDO MONGO API
        currentRequest = new RequestHelper
        {
            
            Uri = MainUrls.BASE_URL + "/register",
            Body = new Phone
            {
                phone = phoneN
            }
            
        };
        //post request to send phone and get id and phone in response 
        RestClient.Post<Response>(currentRequest).Then(res =>
        {
            if (res.errorvalue)
            {
                GameEvents.notify_event_invoke(res.error,Color.red);
                Debug.Log("response has some errors cant get id..maybe you mis-matched models fields");
            }
            else
            {
                Debug.Log("successfully recived response"+JsonUtility.ToJson(res));
                PlayerPrefs.SetString("_id", res.data._id);
                PlayerPrefs.SetString("phone", res.data.phone);
                PlayerPrefs.Save();
                resendDiscription.text= PlayerPrefs.GetString("phone")+"?";

            }
        }).Catch(err =>{
             GameEvents.notify_event_invoke(err.ToString(),Color.red);
             Debug.Log(err);
        } );


    }
    private void PostOTP(string id, string phone, string otp)
    {
        //setting phone number post request for LUDO MONGO API
        currentRequest = new RequestHelper
        {
            Uri = MainUrls.BASE_URL + "/verify-user",
            Body = new Varification
            {
               
                phone=phone,
                 otp=otp,
                _id=id
                
               
            }
        };
        //post request to send phone and get id and phone in response 
        RestClient.Post<Response>(currentRequest).Then(res =>
        {
            if (res.errorvalue)
            {
                GameEvents.notify_event_invoke(res.error,Color.red);
                Debug.Log("json error while reciving token");
                return;
            }
            else
            {
                if (res.data.token != null)
                {
                    PlayerPrefs.SetString("token", res.data.token);
                    PlayerPrefs.SetInt("verified", 1);
                    PlayerPrefs.Save();
                    Debug.Log("worked token ==" + res.data.token);
                     if(res.data.existinguser){
                        PlayerPrefs.SetInt("existing_user",1);
                    }else{
                        PlayerPrefs.SetInt("existing_user",0);
                    }
                     
                    if (res.data.existinguser)
                    {
                        PlayerPrefs.SetInt("registered", 1);
                        PlayerPrefs.SetString("name", res.data.name);
                        PlayerPrefs.SetString("username", res.data.username);
                        PlayerPrefs.SetInt("userprofile", res.data.userprofile);
                        PlayerPrefs.SetInt("usercoins", res.data.usercoins);
                        PlayerPrefs.Save();
                        CheckRegisterdUser();
                    }
                }
                
            }
        if(PlayerPrefs.GetInt("verified")==1){
            verificationPages[0].SetActive(false);
            verificationPages[1].SetActive(true);
        }
        }).Catch(err =>{
            GameEvents.notify_event_invoke(err.ToString(),Color.red);
             Debug.Log(err);
        });
    }
    private void PostDetails(string name, string username, int userprofile)
    {
        Debug.Log(PlayerPrefs.GetString("token"));
        Dictionary<string,string> headers=new Dictionary<string,string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        //setting phone number post request for LUDO MONGO API
        currentRequest = new RequestHelper
        {
            Headers= headers,
            Uri = MainUrls.BASE_URL + "/detail-user",
            Body = new UserDetail
            {
               name=name,
               username=username,
               userprofile=userprofile
            }
        };
        Debug.Log(currentRequest.Body.ToString());
        
        //post request to send phone and get id and phone in response 
        RestClient.Post<Response>(currentRequest).Then(res =>
        {
            
            Debug.Log(JsonUtility.ToJson(res));
            if(res.errorvalue){
                GameEvents.notify_event_invoke(res.error,Color.red);
                Debug.Log("json error");
                return;
            }else{
                Debug.Log(res.data.status);
                if(res.data.status!=null){
                    Debug.Log(res.data.status);
                   
                    PlayerPrefs.SetInt("registered",1);
                    PlayerPrefs.SetString("name",res.data.name);
                    PlayerPrefs.SetString("username",res.data.username);
                    PlayerPrefs.SetInt("userprofile",res.data.userprofile);
                    PlayerPrefs.SetInt("usercoins",res.data.usercoins);
                    PlayerPrefs.Save();
                    CheckRegisterdUser();
                }
            }
        }).Catch(err =>{
            Debug.Log(err);
            GameEvents.notify_event_invoke(err.ToString(),Color.red);
        } );
    }
    private void CheckRegisterdUser(){
            if(PlayerPrefs.GetInt("registered")==1){
                logincanvas.SetActive(false);
                sceneHandler.loadScene("MainMenu"); 
                //perform game enter logic
            }else{
                logincanvas.SetActive(true);
            }
    }
   
}
