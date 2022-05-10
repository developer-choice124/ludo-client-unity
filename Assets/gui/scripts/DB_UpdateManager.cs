using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
public class DB_UpdateManager : MonoBehaviour
{
    RequestHelper requestHelper;
    void Awake()
    {
        GameEvents.Fetch_UserData_Event += this.FetchUserData;
        FetchUserData();
  
    }
    public void FetchUserData()
    {

        // Debug.Log("trying to fetch user data");

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
        requestHelper = new RequestHelper
        {
            Uri = MainUrls.BASE_URL + "/get-user-detail",
            Headers = headers
        };
        RestClient.Get<Response>(requestHelper).Then(res =>
        {
            if (res.errorvalue)
            {
                GameEvents.notify_event_invoke(res.error,Color.red);
                // Debug.Log("error while fetching data of user");
            }
            else
            {
                // Debug.Log("fetched user data from server");
                PlayerPrefs.SetString("phone", res.data.phone);
                PlayerPrefs.SetString("_id", res.data._id);
                PlayerPrefs.SetString("name", res.data.name);
                PlayerPrefs.SetString("username", res.data.username);
                PlayerPrefs.SetInt("userprofile", res.data.userprofile);
                PlayerPrefs.SetInt("usercoins", res.data.usercoins);
                PlayerPrefs.Save();
                GameEvents.Refresh_UIData_Event_invoke();
                
            }

        }).Catch(error =>{
            Debug.Log(error); 
            // GameEvents.notify_event_invoke(error.ToString(),Color.red);
        } );
    }

    void OnDisable()
    {
        GameEvents.Fetch_UserData_Event -= this.FetchUserData;
    }
    void OnEnable()
    {
        GameEvents.Fetch_UserData_Event += this.FetchUserData;
    }
}
