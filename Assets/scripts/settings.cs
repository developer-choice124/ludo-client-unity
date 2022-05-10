using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;
using Proyecto26;
using Models;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class MyType : Attribute
{
    public int Index;
    public string FieldType;
    public System.Type ChildType;
    public string ChildPrimitiveType;

    public MyType(int index, string type, System.Type childType = null)
    {
        Index = index; // GetType().GetFields() doesn't guarantee order of fields, need to manually track them here!
        FieldType = type;
        ChildType = childType;
    }

    public MyType(int index, string type, string childPrimitiveType)
    {
        Index = index; // GetType().GetFields() doesn't guarantee order of fields, need to manually track them here!
        FieldType = type;
        ChildPrimitiveType = childPrimitiveType;
    }
}


[System.Serializable]
public class OnLoadConfig
{
    public bool playerselect_page, twoplayer_page, threeplayer_page, fourplayer_page, buycoins_page, redeem_page;
}
public class settings : MonoBehaviour
{
    public Texture2D gamesprite;
    public pages list;
    public OnLoadConfig loadConfig = new OnLoadConfig();
    RequestHelper currentRequest;

    public static settings _Instance;
    public QualityManager qualityManager;
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Debug.Log(scene.name + " loaded");
        if (scene.name == "MainMenu")
        {
            if (list == null)
            {
                list = FindObjectOfType<pages>();
            }
            SetupConfig();
            GameEvents.Fetch_UserData_Event_invoke();
        }
        qualityManager = FindObjectOfType<QualityManager>();

    }
    async void SetupConfig()
    {

        await Task.Delay(1000);

        list.playeroptions.ToList().ForEach(g =>
        {
            // Debug.LogWarning(g.name + " is " + loadConfig.playerselect_page);
            g.SetActive(loadConfig.playerselect_page);
            if (loadConfig.playerselect_page)
            {
                list.tab.SetActive(true);
            }

        });
        list.buypage.ToList().ForEach(g =>
        {
            g.SetActive(loadConfig.buycoins_page);
            if (loadConfig.buycoins_page)
            {
                list.tab.SetActive(true);
            }
        });
        list.redeem.ToList().ForEach(g =>
        {
            g.SetActive(loadConfig.redeem_page);
            if (loadConfig.redeem_page)
            {
                list.tab.SetActive(true);
            }
        });
    }
    void Awake()
    {
        SetQuality();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        PlayerPrefs.SetInt("sound", 1);
        PlayerPrefs.Save();

        UnityEngine.Object.DontDestroyOnLoad(this);
        if (settings._Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            settings._Instance = this;
        }
        if (list == null)
        {
            list = GameObject.FindObjectOfType<pages>();
        }


    }
    public void SetQuality()
    {
        if (!PlayerPrefs.HasKey("quality"))
        {
            Application.targetFrameRate = 45;
        }
        if (PlayerPrefs.GetInt("quality") == 0)
        {
            Application.targetFrameRate = 30;

        }
        else if (PlayerPrefs.GetInt("quality") == 1)
        {
            Application.targetFrameRate = 45;

        }
        else if (PlayerPrefs.GetInt("quality") == 2)
        {
            Application.targetFrameRate = 60;
        }


    }
    public void leave()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();

        // Debug.LogWarning("under setting");
        GameClient._Instance.Leave();

        SceneManager.LoadScene("MainMenu");
    }
    public void Setleave()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        GameClient._Instance.Leave(false);

    }
    public void leave_options()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        GameClient._Instance.Leave();
        loadConfig.playerselect_page = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void leave_shop()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        GameClient._Instance.Leave();
        loadConfig.buycoins_page = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void leave_redeem()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        GameClient._Instance.Leave();
        loadConfig.redeem_page = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void mute(int v)
    {
        PlayerPrefs.SetInt("sound", v);
        PlayerPrefs.Save();
    }
    public void graphics(int v)
    {
        PlayerPrefs.SetInt("graphics", v);
        PlayerPrefs.Save();
        qualityManager.SetQuality(v);
    }
    public void Exit()
    {
        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        GameClient._Instance.Leave();
        Application.Quit();
    }
    public void openlink(string link)
    {
        Application.OpenURL(link);
    }
    public void shareapp()
    {
        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, gamesprite.EncodeToPNG());
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
        headers.Add("apisecret", MainUrls.APP_SECRET);
        currentRequest = new RequestHelper
        {
            Headers = headers,
            Uri = MainUrls.BASE_URL + "/get-app-update-data"
        };
        RestClient.Get<update>(currentRequest).Then(res =>
        {
            if (res.errorvalue)
            {
                GameEvents.notify_event_invoke("cant get share link at the moment", Color.red);
            }
            else
            {
                NativeShare ns = new NativeShare();
                ns.SetTitle("Pocket Ludo king ").AddFile(filePath);
                ns.SetText("Download this free awesome game called pocket ludo king  and earn money with your skills and luck.. use my number - " + PlayerPrefs.GetString("phone") + " as referral code to get coins for free. " +
                PlayerPrefs.GetString("applink"));
                ns.Share();
            }
        }).Catch(err => GameEvents.notify_event_invoke("cant get share link at the moment", Color.red));
    }
    public void shareroomid(string id)
    {
        NativeShare ns = new NativeShare();
        ns.SetTitle(PlayerPrefs.GetString("username") + "'s Room");
        ns.SetText(id);
        ns.Share();
    }
    public void reconnectgame()
    {
        if (!NativeInternet.IsNetworkAvailableStatic)
        {
            GameEvents.notify_event_invoke("please first connect to the internet. ", Color.red);
            return;
        }
        GameClient._Instance.mainClientBoard = null;
        GameClient._Instance.roomconfig = new RoomConfig();

        if (GameClient._Instance.gameManager){
            GameClient._Instance.gameManager.homes = new List<homeManager>();
            GameClient._Instance.Clean();
        }
        if (PlayerPrefs.HasKey("gameover"))
        {
            int pplayers = PlayerPrefs.GetInt("previous_max_players");
            if (PlayerPrefs.GetInt("gameover") == 0)
            {
                if (PlayerPrefs.HasKey("bot") && PlayerPrefs.GetInt("bot") == 1)
                {
                    SceneManager.LoadScene("bot");
                }
                else
                {
                    switch (pplayers)
                    {
                        case 2:
                            SceneManager.LoadScene("2players");
                            break;
                        case 3:
                            SceneManager.LoadScene("3players");
                            break;
                        case 4:
                            SceneManager.LoadScene("4players");
                            break;
                        default:
                            break;
                    }
                }
                GameClient._Instance.roomconfig = new RoomConfig { maxPlayers = pplayers,method="reconnect"};
                // GameClient._Instance.ReconnectRoom();
            }
        }
    }
    public void playernotfound(int mode)
    {

        if (mode == 0)
        {
            GameEvents.notify_event_invoke("cant find players at the moment try other rooms", Color.red);
            leave();
            return;
        }
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
        headers.Add("apisecret", MainUrls.APP_SECRET);

        currentRequest = new RequestHelper
        {
            Headers = headers,
            Uri = MainUrls.BASE_URL + "/post-user-wait-after-data",
            Body = new playernotfound
            {
                costofroom = GameClient._Instance.roomconfig.roomName,
                numberofplayers = GameClient._Instance.roomconfig.maxPlayers
            }
        };
        RestClient.Post<uploadres>(currentRequest).Then(res =>
        {
            if (res.errorvalue)
            {

            }
            else
            {
                GameEvents.notify_event_invoke("cant find players at the moment try other rooms", Color.red);
                leave();
            }
        }).Catch(err => GameEvents.notify_event_invoke("cant find players at the moment try other rooms", Color.red));

    }
    public void PlayWithBot()
    {

        PlayerPrefs.SetInt("previous_max_players", -1);
        Setleave();
        GameClient._Instance.roomconfig.method="bot";
        GameClient._Instance.roomconfig.maxPlayers=2;
        SceneManager.LoadScene("bot");

    }
    void OnEnable()
    {

        qualityManager = FindObjectOfType<QualityManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
