using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpdateMananger : MonoBehaviour
{
    public List<Image> userDps=new List<Image>(); 
    public List<Text> userCoins=new List<Text>();
    public List<Text> userNames=new List<Text>();
    public List<Text> names=new List<Text>();
    public List<Text> userphones=new List<Text>();
    public utils profilesprites;
    
    void Awake(){
       GameEvents.Refresh_UIData_Event+=this.Refresh;
       Refresh();
    }
    public void Refresh(){
        
        // Debug.Log("tring to refresh user ui data");
        GameObject[] userdpobjs= GameObject.FindGameObjectsWithTag("userDpImg");
        foreach(GameObject g in userdpobjs){
            if(!userDps.Contains(g.GetComponent<Image>())){
                 userDps.Add(g.GetComponent<Image>());
            }
        }
         GameObject[] userPhoneobjs= GameObject.FindGameObjectsWithTag("userPhoneTxt");
        foreach(GameObject g in userPhoneobjs){
            if(!userphones.Contains(g.GetComponent<Text>())){
                 userphones.Add(g.GetComponent<Text>());
            }
        }
         //find all gameobjs  with tag usercointxt and set its image componenet to userdp image array
        GameObject[] userCoinsobjs= GameObject.FindGameObjectsWithTag("userCoinTxt");
        foreach(GameObject g in userCoinsobjs){
            if(!userCoins.Contains(g.GetComponent<Text>())){
            userCoins.Add(g.GetComponent<Text>());
            }
        }
         //find all gameobjs  with tag usernametxt and set its image componenet to userdp image array

        GameObject[] userNamesobjs= GameObject.FindGameObjectsWithTag("userNameTxt");
        foreach (GameObject g in userNamesobjs){
            if(!userNames.Contains(g.GetComponent<Text>())){
                userNames.Add(g.GetComponent<Text>());
            }
        }
        GameObject[] userFullNamesobjs= GameObject.FindGameObjectsWithTag("userFullNameTxt");
        foreach (GameObject g in userFullNamesobjs){
            if(!names.Contains(g.GetComponent<Text>())){
                names.Add(g.GetComponent<Text>());
            }
        }
        //find all gameobjs  with tag userDptxt and set its image componenet to userdp image array
        if(PlayerPrefs.HasKey("name")){
            foreach(Text udp in names){
                udp.text=PlayerPrefs.GetString("name");
            }
        }else{
            foreach(Text udp in names){
                udp.text="null";
            }
        }
        if(PlayerPrefs.HasKey("userprofile")){
            foreach(Image udp in userDps){
                if(udp)
                udp.sprite=profilesprites.sprites[PlayerPrefs.GetInt("userprofile")];
            }
        }else{
            foreach(Image udp in userDps){
                udp.sprite=profilesprites.sprites[0];
            }
        }
        if(PlayerPrefs.HasKey("usercoins")){
            foreach(Text ctxt in userCoins){
                ctxt.text=PlayerPrefs.GetInt("usercoins").ToString();
            }
           
        }else{
            foreach(Text ctxt in userCoins){
                ctxt.text="null";
            }
        }
        if(PlayerPrefs.HasKey("username")){
            
             foreach(Text untxt in userNames){
                untxt.text=PlayerPrefs.GetString("username");
             }
           
        }else{
            foreach(Text untxt in userNames){
                untxt.text="null";
             }
        }
        if(PlayerPrefs.HasKey("phone")){
            
             foreach(Text untxt in userphones){
                untxt.text=PlayerPrefs.GetString("phone");
             }
           
        }else{
            foreach(Text untxt in userNames){
                untxt.text="null";
             }
        }
        // Debug.Log("user ui data refreshed");
    }
    public void LogOut(){
        PlayerPrefs.DeleteAll();
        SceneHandler sh = FindObjectOfType<SceneHandler>();
        sh.loadScene("login");
    }
    void OnDisable(){
        GameEvents.Refresh_UIData_Event-=this.Refresh;
    }
}
