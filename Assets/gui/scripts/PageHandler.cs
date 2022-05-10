using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class PageHandler : MonoBehaviour
{
    public Toggle[] playersToggles;
    public GetCoins objectInstantiater;
    public int players; 
    void Start()
    {    
        foreach(Toggle T in playersToggles){
            T.onValueChanged.AddListener((b)=>{

                if(GetPlayerNumber(T.name,b)!=0){
                    CreateObjects(GetPlayerNumber(T.name,b));
                }
            });
            if(T.isOn){
                if(GetPlayerNumber(T.name,T.isOn)!=0){
                    CreateObjects(GetPlayerNumber(T.name,T.isOn));
                }
            }
        }
        
    }
    public void CreateObjects(int p){
        players=p;
        DeleteObjects();
        if(objectInstantiater)
        objectInstantiater.GetObjects(p);
    }
    
    public void DeleteObjects(){
        if(objectInstantiater)
        objectInstantiater.RemoveObjects();
    }
    public int GetPlayerNumber(string n ,bool b){
            
       if(n=="twoplayer"&b){
         
           return 2;
       }else if(n=="twoplayer"&&!b){
          return 0; 
       }else if(n=="threeplayer"&&b){
          
          return 3; 
       }
       else if(n=="threeplayer"&&!b){
          return 0; 
       }
       else if(n=="fourplayer"&&b){
          
          return 4; 
       }else if(n=="fourplayer"&&!b){
          return 0; 
       }else{
           return 0;
       } 

    }
  
}
