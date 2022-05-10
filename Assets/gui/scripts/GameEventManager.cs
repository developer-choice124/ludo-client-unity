using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    // Start is called before the first frame update
    RequestHelper currentRequest;
    public Text version, New, bugfix, linktxt,date;
    public string link;
    public GameObject updatePage,updateclose;
    public GameObject[] refrerrpage;
    void Awake(){
     
	    CheckForUpdate();
	    //checkforRefrral();
       
    }
    public void RefreshDB()
    {
        GameEvents.Fetch_UserData_Event_invoke();
    }
    
    public void RefreshUI()
    {
        GameEvents.Refresh_UIData_Event_invoke();
    }
    public void CheckForUpdate(){
      
        Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        headers.Add("apisecret",MainUrls.APP_SECRET);
        
        currentRequest= new RequestHelper{
            Headers=headers,
            Uri=MainUrls.BASE_URL+"/get-app-update-data"
        };
        RestClient.Get<update>(currentRequest).Then(res=>{
            if(res.errorvalue){
                GameEvents.notify_event_invoke(res.error,Color.green);
                updatePage.SetActive(false);
            }else{
                PlayerPrefs.SetString("applink",res.data.link);
                if(res.data.type){
                    updateclose.SetActive(false);
                }else{
                     updateclose.SetActive(true);
                }
                Debug.Log("upadate"+ JsonUtility.ToJson(res));
                if(res.data.version!=Application.version){
                      Debug.Log("popup");
                    if(New)
                    New.text+= res.data.whatsnew;
                    
                    if(version)
                    version.text+=res.data.version_type;

                    if(linktxt)
                    linktxt.text+=res.data.link;
                      
                    link=res.data.link;
                    
                    if(updatePage)
                    updatePage.SetActive(true);
                }else if(res.data.version_type==Application.version){
                   
                    updatePage.SetActive(false);
                }
               
            }
            
        }).Catch(err=> Debug.Log(err));
    }
    public void openUpdateLink(){
        Application.OpenURL(link);
    }
    public void checkforRefrral(){
             
        
        Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        currentRequest= new RequestHelper{
            Headers=headers,
            Uri=MainUrls.BASE_URL+"/check-refeered-already-by-user"
        };
        RestClient.Post<update>(currentRequest).Then(res=>{
            if(res.errorvalue){
                foreach(GameObject g in refrerrpage){
                g.SetActive(false);
                }
            }else{
                 foreach(GameObject g in refrerrpage){
                g.SetActive(true);
                }
            }
        }).Catch(err=> Debug.Log(err));
        
    }
   
}