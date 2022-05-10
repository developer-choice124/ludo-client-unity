using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using RSG;
using Models;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Threading.Tasks;
public static class game
{
    public static Texture2D gamesprite;
    public static int goalPos;
    public static int currentTurn;
    public const string BASE_URL = "http://192.168.1.8:8000";
    public const string SOCKET_URL = "ws://192.168.1.8:8000";
    public const float TIME_LIMIT = 30;
    public const string APP_SECRET = "LudoTechphantAppSecret";
    public static RequestHelper currentRequest = new RequestHelper();

    public struct urls
    {
        public struct get
        {
            public const string shopList = BASE_URL + "Get-Shop-List";
            public const string roomList = BASE_URL + "Get-Room-List";
            public const string historyList = BASE_URL + "Get-History-list";
            public const string bannerList = BASE_URL + "Get-Banner-List";
            public const string userData = BASE_URL + "Get-User-Data";
            public const string appUpdate = BASE_URL + "Get-App-Update";
            public const string JoinDetails = BASE_URL + "/get-join-room-details/{joinroom:" + "<roomid>" + " }";
        }
        public struct post
        {
            public const string login = BASE_URL + "get-buy-list";
            public const string verify = BASE_URL + "get-buy-list";
            public const string roomId = BASE_URL + "Ganerate-Room-Id";
        }
    }
    // public static void CreateRoomId(string rd)
    // {
    //     Dictionary<string, string> headers = new Dictionary<string, string>();
    //     headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
    //     headers.Add("apisecret", MainUrls.APP_SECRET);
        
    //     currentRequest = new RequestHelper
    //     {
    //         Headers = headers,
    //         Uri = urls.post.roomId,
    //         Body = new postroomdata
    //         {
    //             joinroomdata = rd
    //         }
    //     };
    //     RestClient.Post<roomdatareq>(currentRequest).Then(res =>
    //     {
    //         if (res.errorvalue)
    //         {
    //             Debug.Log("cant create room");
    //             GameEvents.notify_event_invoke("cant create room id", Color.red);
    //         }
    //         else
    //         {
    //             GameClient._Instance.roomconfig.roomShareId = res.data[0].createdroomid;
    //             InputField roomid = GameObject.FindGameObjectWithTag("roomidtext").GetComponent<InputField>();
    //             roomid.text = res.data[0].createdroomid;
    //             Debug.Log(" create room id" + res.data[0].createdroomid);
    //             GameEvents.notify_event_invoke(" create room id" + res.data[0].createdroomid, Color.red);
    //         }

    //     }).Catch(err => GameEvents.notify_event_invoke("cant create room", Color.red));

    // }
    // public static void ConvertIdToDetails(string ri)
    // {
    //     string rdata = "";
    //     Dictionary<string, string> headers = new Dictionary<string, string>();
    //     headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
    //     headers.Add("apisecret", MainUrls.APP_SECRET);

    //     currentRequest = new RequestHelper
    //     {
    //         Headers = headers,
    //         Uri =  urls.get.JoinDetails.Replace("<roomid>",ri),
    //     };
    //     RestClient.Get<roomdatareqget>(currentRequest).Then(res =>
    //     {
    //         if (res.errorvalue)
    //         {
    //             rdata = "";
    //             Debug.Log("cant join room");
    //             GameEvents.notify_event_invoke("cant join room id", Color.red);
    //         }
    //         else
    //         {
    //             Debug.Log(" join id " + res.data.createdroomid);
    //             rdata = res.data.createdroomid;

    //         }
    //         return res.data.createdroomid;

    //     }).Catch(err =>
    //     {
    //         rdata = "";
    //         GameEvents.notify_event_invoke("cant join room", Color.red);
    //     });

    // }
    // public static void ShareApp()
    // {
    //     string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
    //     File.WriteAllBytes(filePath,gamesprite.EncodeToPNG());
    //     Dictionary<string, string> headers = new Dictionary<string, string>();
    //     headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
    //     headers.Add("apisecret", MainUrls.APP_SECRET);

    //     currentRequest = new RequestHelper
    //     {
    //         Headers = headers,
    //         Uri = urls.get.appUpdate
    //     };
    //     RestClient.Get<update>(currentRequest).Then(res =>
    //     {
    //         if (res.errorvalue)
    //         {
    //             GameEvents.notify_event_invoke("cant get share link at the moment", Color.red);
    //         }
    //         else
    //         {
    //             NativeShare ns = new NativeShare();
    //             ns.SetTitle("World Pro LUDO").AddFile(filePath);
    //             ns.SetText("Download this free awesome game called WorldPro LUDO and earn money with your skills and luck.. use my number - " + PlayerPrefs.GetString("phone") + " as referral code to get coins for free. " +
    //             PlayerPrefs.GetString("applink"));
    //             ns.Share();
    //         }

    //     }).Catch(err => GameEvents.notify_event_invoke("cant get share link at the moment", Color.red));

    // }
    // public static void ShareRoomid(string id)
    // {
    //     string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
    //     File.WriteAllBytes(filePath, gamesprite.EncodeToPNG());
    //     Dictionary<string, string> headers = new Dictionary<string, string>();
    //     headers.Add("authorization", "JWT " + PlayerPrefs.GetString("token"));
    //     headers.Add("apisecret", MainUrls.APP_SECRET);

    //     currentRequest = new RequestHelper
    //     {
    //         Headers = headers,
    //         Uri = urls.get.appUpdate
    //     };
    //     RestClient.Get<update>(currentRequest).Then(res =>
    //     {
    //         if (res.errorvalue)
    //         {
    //             GameEvents.notify_event_invoke("cant get share link at the moment", Color.red);
    //         }
    //         else
    //         {
    //             NativeShare ns = new NativeShare();
    //             ns.SetTitle(PlayerPrefs.GetString("username") + "'s Room").AddFile(filePath);
    //             ns.SetText("Download this free awesome game called WorldPro LUDO and earn money with your skills and luck.. use my number - " + PlayerPrefs.GetString("phone") + " as referral code to get coins for free. " +
    //             PlayerPrefs.GetString("applink") + "and join with RoomID- " + "*" + id + "*");
    //             ns.Share();
    //         }

    //     }).Catch(err => GameEvents.notify_event_invoke("cant get share link at the moment", Color.red));

    // }
    // public static void ReconnectRoom()
    // {
    //     if (!NativeInternet.IsNetworkAvailableStatic)
    //     {
    //         GameEvents.notify_event_invoke("please first connect to the internet. ", Color.red);
    //         return;
    //     }
    //     //emety everything
    //     GameClient._Instance.mainClientBoard = null;
    //     GameClient._Instance.roomconfig = new RoomConfig();

    //     if(GameClient._Instance.gameManager)
    //     GameClient._Instance.gameManager.homes = new List<homeManager>();
    //     GameClient._Instance.Clean();
    //     // GameClient._Instance.

    //     if (PlayerPrefs.HasKey("gameover"))
    //     {
    //         if (PlayerPrefs.GetInt("gameover") == 0)
    //         {
    //             int players = PlayerPrefs.GetInt("previous_max_players");
    //             Debug.LogWarning("unfinished game has " + players + " players");

    //             switch (players)
    //             {
    //                 case 2:
    //                     SceneManager.LoadScene("2players");
    //                     break;
    //                 case 3:
    //                     SceneManager.LoadScene("3players");
    //                     break;
    //                 case 4:
    //                     SceneManager.LoadScene("4players");
    //                     break;
    //                 default:
    //                     break;
    //             }
    //             GameClient._Instance.roomconfig = new RoomConfig { maxPlayers = players};
    //             GameClient._Instance.ReconnectRoom();
    //         }
    //     }

    // }
    public static void PassMessage(Message msg)
    {
        // if (settings._Instance.islocal)
        // {
        //     settings._Instance?.comManager.onMessageHandle(msg.id, msg);
        // }
        // else
        // {
        //     GameClient._Instance?.Send(msg);
        // }
    }

}
