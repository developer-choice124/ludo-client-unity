using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using Proyecto26;
using Models;
using UnityEngine.SceneManagement;

public class BotClient : MonoBehaviour
{
    Client client;
    public static BotClient _Instance;
    public Room<State> currentRoom;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "bot")
        {
            StartBot();
        }
    }
    void Awake()
    {
        if (!BotClient._Instance)
        {
            BotClient._Instance = this;
            Object.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void StartBot()
    {
        if (PlayerPrefs.HasKey("gameover"))
        {
            if (PlayerPrefs.GetInt("gameover") == 1)
            {
                ConnectBot();
            }
            else
            {
                ReconnectRoom();
            }
        }
        else
        {
            ReconnectRoom();
        }
    }
    public async void ReconnectRoom()
    {

        client = ColyseusManager.Instance.CreateClient(MainUrls.SOCKET_URL);

        try
        {
            currentRoom = await client.Reconnect<State>(PlayerPrefs.GetString("previous_room_id"), PlayerPrefs.GetString("previous_bot_session_id"));
            var confiq = GameClient._Instance.roomconfig;
            confiq.roomid = "5ed4b03b9888ca0b2c144c74";
            confiq.maxPlayers = 2;
            currentRoom.OnMessage<int>("turn", (turn) =>
            {
                Debug.Log("turn  bot = " + turn);
            });
            PlayerPrefs.SetString("previous_bot_session_id", currentRoom.SessionId);

            PlayerPrefs.Save();

        }
        catch (MatchMakeException ex)
        {
            GameEvents.notify_event_invoke("game has been finished " + ex.Message, Color.red);
        }
    }
    public async void Send(object msg)
    {
        if (msg is Message)
        {
            if (currentRoom != null)
            {
                await currentRoom.Send("*", msg);
            }
        }
    }
    public void ConnectBot()
    {
        Dictionary<string,string> headers=new Dictionary<string, string>();
        headers.Add("authorization","JWT "+PlayerPrefs.GetString("token"));
        var body =new botrequest
        {
            phone = "1234567890",
            coins = GameClient._Instance.roomconfig.roomName
        };
        Debug.LogWarning("connecting bot "+JsonUtility.ToJson(body));
        RestClient.Post<Response>(new RequestHelper
        {
            Uri=MainUrls.BASE_URL+"/get-bot-details",
            Headers=headers,
            Body =body
        
        }).Then(async result =>
        {
            if (result.errorvalue)
            {
                Debug.LogWarning("no bot users found");
                return;
            }
            else
            {
                Debug.LogWarning("bot user found");
                PlayerPrefs.SetString("botName", "botv1");
                PlayerPrefs.SetInt("botLevel", 1);
                PlayerPrefs.Save();
                client = ColyseusManager.Instance.CreateClient(MainUrls.SOCKET_URL);
                var options = new Dictionary<string, object>();
                var confiq = GameClient._Instance.roomconfig;
                options.Add("maxPlayers", confiq.maxPlayers);
                options.Add("maxClients", confiq.maxPlayers);
                options.Add("id", result.data._id);
                options.Add("room_id", confiq.roomid);

                if (Mathf.Abs(result.data.userprofile) < 14)
                {
                    options.Add("user_profile", result.data.userprofile);
                }
                else
                {
                    options.Add("user_profile", 1);
                }

                options.Add("user_name", result.data.username);
                options.Add("fee", confiq.roomName);
                options.Add("win", "100");

                currentRoom = await client.Create<State>(GameClient._Instance.roomconfig.roomName, options);
                PlayerPrefs.SetString("previous_bot_session_id", currentRoom.SessionId);
                PlayerPrefs.SetInt("bot", 1);
                PlayerPrefs.Save();
                currentRoom.OnMessage<int>("turn", (turn) =>
                {
                    Debug.Log("turn  bot = " + turn);
                });
                GameClient._Instance.roomconfig.col_roomid = currentRoom.Id;
            }
        }).Catch(err =>
        {
            if(GameClient._Instance.roomconfig.maxPlayers==2){
                GameClient._Instance.roomconfig.method="joinOrCreate";
                SceneManager.LoadScene("2players");
            }else if(GameClient._Instance.roomconfig.maxPlayers==3){
                 GameClient._Instance.roomconfig.method="joinOrCreate";
                SceneManager.LoadScene("3players");
            }else if(GameClient._Instance.roomconfig.maxPlayers==4){
                 GameClient._Instance.roomconfig.method="joinOrCreate";
                SceneManager.LoadScene("4players");
            }
           Debug.LogError(err);
        });
    }
}
