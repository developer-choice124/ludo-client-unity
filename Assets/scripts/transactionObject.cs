using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class transactionObject : MonoBehaviour
{
	public Text amount,statetxt,date,Name,paynumber,phonenumber,type;
	public Image profile,statusbg;
    public utils profiles;
    public void SetUp(int a,string s,string d,string pn,string pn2)
    {  

        amount.text=a.ToString()+" coins";
        statetxt.text=s;
        date.text=d;
        Name.text=PlayerPrefs.GetString("username");
        profile.sprite=profiles.sprites[PlayerPrefs.GetInt("userprofile")];
        paynumber.text=pn;
        phonenumber.text=pn2;
    }
    public void SetUp(int a,string l,string d,string pn,string pn2,int s)
    {  
	    amount.text="₹"+a.ToString(); 
      
        if(s==1){
             statetxt.text="Success!";
             statusbg.color=  new Color(.3f,1f,0,.5f);
        }else{
            
             statetxt.text="Pending.";
             statusbg.color= Color.yellow;
        }
       
        date.text=d;
        Name.text=PlayerPrefs.GetString("username");
        profile.sprite=profiles.sprites[PlayerPrefs.GetInt("userprofile")];
        paynumber.text=pn;
        phonenumber.text=pn2;
    }
    public void SetUp(string id,int a,string d,int players)
    {  
        if(id==PlayerPrefs.GetString("_id")){
            statetxt.text="Win";
            statetxt.color=Color.green;
            amount.text="+ "+(a*players).ToString();
            amount.color=Color.green;
        }else{
            statetxt.text="Lose";
            statetxt.color=Color.red;
            amount.text="- "+a.ToString();
            amount.color=Color.red;
        }
        date.text=d;
        Name.text=PlayerPrefs.GetString("username");
        profile.sprite=profiles.sprites[PlayerPrefs.GetInt("userprofile")];

        paynumber.text="Fee : "+a.ToString();
        phonenumber.text="Players : "+players.ToString();
    }  
}
