
using UnityEngine;

public class webviewmanager : MonoBehaviour
{
   
    WebViewObject webViewObject;
    GameEventManager gem;
    public string urlCheck;

    void Start(){
        gem=FindObjectOfType<GameEventManager>();
    }
    public void CreateWebView(string url)
    {
        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) =>
        {

            if (msg.StartsWith("url:"))
            {
                urlCheck = msg.Replace("url:", "");
                Debug.Log(msg);

            }

        });
        webViewObject.EvaluateJS(@"
            window.addEventListener('onpageshow', function(){
            Unity.call('url:' + window.location.href);
            }, false);
        ");

        webViewObject.SetMargins(0, 0, 0, 0);
        webViewObject.SetVisibility(true);
        webViewObject.LoadURL(url);
    }
    void GoBack()
    {
        webViewObject.GoBack();
    }
    void GoForward()
    {
        if (webViewObject.CanGoBack() == false)
        {
            CloseWebView();
        }
        else
        {
            webViewObject.GoForward();
        }
    }
    public void CloseWebView()
    {

        GameObject view = GameObject.Find("WebViewObject");

        if (view != null)
        {
            Destroy(view);

        }
    }
    private bool IsPayedMoney()
    {
        if (urlCheck.Contains("success.node"))
        {
            urlCheck = "";
            GameEvents.Fetch_UserData_Event_invoke();
            GameEvents.Refresh_UIData_Event_invoke();
            GameEvents.notify_event_invoke ("successfully purchesed coins",Color.green);
            if(gem){
                 gem.checkforRefrral();
            }
            return true;

        }
        else if (urlCheck.Contains("failed.node"))
        {
            urlCheck = "";
            GameEvents.notify_event_invoke (" failed while purchesing coins",Color.red);
            return true;
        }
        else
        {
            return false;
        }

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CloseWebView();
        }

        if (IsPayedMoney())
        {
            CloseWebView();

        }
        if (webViewObject != null)
        {
            webViewObject.EvaluateJS(@"
           if(location){
            Unity.call('url:' + window.location.href);}
        ");
        }

    }
    
}
