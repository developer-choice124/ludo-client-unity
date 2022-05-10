using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using Models;
using Proyecto26;

public class ReferralSubmitData{
    public long phone;

}

[System.Serializable]
public class ReferralSubmit
{
    public InputField refPhone;

    public RequestHelper BuildRequest(){

        var body = new ReferralSubmitData{
            phone= Convert.ToInt64(refPhone.text),
        };

        var req = new RequestHelper{
            Uri=MainUrls.BASE_URL+"/PostRefarral",
            Body=body,
            Headers= new Dictionary<string, string>{
                {"authorization","JWT "+PlayerPrefs.GetString("token")},
                {"content-type","application/json"}
            }
        };
        return req;
    }
}