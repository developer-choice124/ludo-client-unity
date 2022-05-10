using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : MonoBehaviour
{
    public Dice dice;
    public utils profileImages;
    public Text username,lazymoves,goaled,opened;
    public Image user_image,Timer;
    public Button diceButton;
    public GameObject userImagePanel;
    public Transform left,right;


    public void SetPicture(int i){
        user_image.sprite= profileImages.sprites[i];
        Debug.Log("picture synced");
    }
    public void SetUsername(string s){
        username.text=s;
        // Debug.Log("name synced");
    }
    public void ArrangePanel(){
        if(transform.position.x<0){
             transform.position = new Vector3(transform.position.x+1f,transform.position.y,transform.position.z);
            userImagePanel.transform.position=left.position;
        }else if(transform.position.x>0){
            transform.position = new Vector3(transform.position.x-1f,transform.position.y,transform.position.z);
            userImagePanel.transform.position=right.position;
        }
    }
    public void UpdateTime(int t){
         float fade= Remap(t,0,MainUrls.TIME_LIMIT,1,0);
         Timer.fillAmount=Mathf.Lerp(Timer.fillAmount,fade,.2f);
         Timer.color=Color.LerpUnclamped(Color.red, Color.green,fade);

    }
   public  float Remap ( float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }
}
