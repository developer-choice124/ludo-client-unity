using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
using System.Linq;

public class TransactionsHistory : MonoBehaviour
{
    // Start is called before the first frame update
      RequestHelper requestHelper;
      public GameObject container;
      public GameObject transactionObj;
      public int fromlast=25;
      public int mode=0;
      public string apiCall="get-repay-payment-amount-list-by-jwt";
      
    void OnEnable()
    {
        if (mode == 0)
        {
            container.GetComponentsInChildren<Transform>().ToList().ForEach(t =>
            {
                if (t != container.transform)
                {
                    Destroy(t.gameObject);
                }

            });
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
            requestHelper = new RequestHelper
            {
                Uri = MainUrls.BASE_URL + "/" + apiCall,
                Headers = headers,
            };
            RestClient.Get<RTransactions>(requestHelper).Then(res =>
            {
                if (res.errorvalue)
                {
                    GameEvents.notify_event_invoke(" cant load transactions at the moment", Color.red);
                }
                else
                {
                    Debug.Log(JsonUtility.ToJson(res.data));
                    List<RTransaction> tol = res.data.ToList();
                    tol.Reverse();
                    
                    tol.ForEach(o=>{
                        
                       GameObject go = GameObject.Instantiate(transactionObj, container.transform);
                        go.GetComponent<transactionObject>().SetUp(o.redeemcoins, "redeem", o.created_on, o.paynumber, o.phone,o.is_active);
                    });
                   
                }
            }).Catch(err =>
            {
                GameEvents.notify_event_invoke(" cant load transactions at the moment", Color.red);
            });
        }else
        {
             container.GetComponentsInChildren<Transform>().ToList().ForEach(t =>
            {
                if (t != container.transform)
                {
                    Destroy(t.gameObject);
                }

            });
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
            requestHelper = new RequestHelper
            {
                Uri = MainUrls.BASE_URL + "/" + apiCall,
                Headers = headers,
            };
            RestClient.Get<Transactions>(requestHelper).Then(res =>
            {
                if (res.errorvalue)
                {
                    GameEvents.notify_event_invoke(" cant load transactions at the moment", Color.red);
                }
                else
                {

                    List<Transaction> tol = res.data.ToList();
                    tol.Reverse();
                    
                       tol.ForEach(o=>{
                        GameObject go = GameObject.Instantiate(transactionObj, container.transform);
                        go.GetComponent<transactionObject>().SetUp(o.winnerId,o.coins,o.date_modified,o.max_players);
                    });
                }
                  
            }).Catch(err =>
            {
                GameEvents.notify_event_invoke(" cant load transactions at the moment", Color.red);
            }); 
        }
    }

    // Update is called once per frame

}
