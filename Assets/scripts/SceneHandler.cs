using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{   
   
    void Start(){
       
      
      
    }
    public void loadScene(int index){
       
        SceneManager.LoadScene(index);
    }
    public void loadScene(string name){
       
        SceneManager.LoadScene(name);
    }
    

}
