using System.Collections.Generic;
using UnityEngine;
using Colyseus;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using MyInterfaces;
using System.Threading.Tasks;


[System.Serializable]
public class RoomConfig
{
    public string method;
    public string roomName;
    public string roomid;
    public string col_roomid;
    public int maxPlayers;
    public string total;
}
public class GameClient : MonoBehaviour
{
    public RoomConfig roomconfig = new RoomConfig();
    Client client;
    public static Dictionary<string, IActionable<int,int>> clientPlayers = new Dictionary<string, IActionable<int,int>>();
    public List<IActionable<int,int>> playerboards = new List<IActionable<int,int>>();
    public IActionable<int,int> mainClientBoard;
    public static Room<State> room;
    public static GameClient _Instance;
    public GameManager gameManager;
    public Board board;
    public List<Player> lobby=new List<Player>();
    public State lobbystate;
    public float SocketStrenth=5;
    public bool reconnected=false,reconnectScreen;
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
        if(scene.name=="2players"||scene.name=="3players"||scene.name=="4players"||scene.name=="bot"){
            Connect();
        }
        board=FindObjectOfType<Board>();
    }
    void Awake()
    {
        client =  ColyseusManager.Instance.CreateClient(MainUrls.SOCKET_URL);
        Application.runInBackground=true;

        UnityEngine.Object.DontDestroyOnLoad(this);

        if(GameClient._Instance){
            Destroy(this.gameObject);
        }else{
            _Instance = this;
        }
    }
    public void Connect()  
    {
        gameManager=FindObjectOfType<GameManager>();
        playerboards = FindObjectsOfType<MonoBehaviour>().OfType<IActionable<int,int>>().ToList();
        if (roomconfig.method != null)
        {
            switch (roomconfig.method)
            {
                case "joinorcreate":
                    JoinOrCreate(roomconfig.roomName,roomconfig.maxPlayers);
                    break;
                case "join":
                    JoinRoom(roomconfig.col_roomid, roomconfig.roomName,roomconfig.maxPlayers);
                    break;
                case "create":
                    CreateRoom(roomconfig.roomName,roomconfig.maxPlayers);
                    break;
                case "reconnect":
                    ReconnectRoom();
                break;
                case "bot":
                    ConnectWithBot();
                break;
            }
        }
    }
    public async void ConnectWithBot()  
    {

        await Task.Delay(2000*UnityEngine.Random.Range(1,5));
        gameManager=FindObjectOfType<GameManager>();
        Debug.Log(gameManager.name+ " is manager");
        playerboards = FindObjectsOfType<MonoBehaviour>().OfType<IActionable<int,int>>().ToList();
        
        if(roomconfig.method != null)
        {
            JoinRoom(roomconfig.col_roomid , roomconfig.roomName , roomconfig.maxPlayers,1);
        }

    }
    void SetUpEvents()
    {
        room.OnLeave += (code) =>
        {
            print("ROOM: ON LEAVE");

        };
        room.State.players.OnAdd += OnPlayerAdd;
        room.State.players.OnChange += OnPlayerChange;
        room.State.players.OnRemove += OnPlayerRemove;
        room.OnStateChange += OnStateChange;
        //room.OnMessage += OnMessage;
        room.OnJoin += OnJoin;
        room.OnLeave += OnLeave;
        room.OnMessage<int>("turn",(turn)=>{
            // Debug.Log( "TURN BROADCAST ="+ turn);
            playerboards.Find(p => (p._Seat==turn))?.OnTurn();
        });
        PlayerPrefs.SetString("room_id", room.Id);
        PlayerPrefs.SetString("Session_id", room.SessionId);
        PlayerPrefs.Save();

    }
    void OnStateChange(State state, bool isfirst)
    {   
        SocketStrenth=5;
        if(PlayerPrefs.GetInt("gameover")==0&&!isfirst){
            if(reconnected==false){
                 SyncClient(state);
                 reconnected=true;
            }
        }
        if (isfirst)
        {
            if(gameManager){
                gameManager.time= (float)TimeSpan.FromMilliseconds(state.time).TotalSeconds;                    
                gameManager.currentTurnOf=Mathf.RoundToInt(state.turn);
                gameManager.nextTurnTime=state.turndelay;
                roomconfig.roomName=state.fee;
                roomconfig.total=state.win;
            }
        }else{
            if(gameManager){
                gameManager.UIArea.g_phase.text=state.gamePhase+"..";
                gameManager.gamePhase=state.gamePhase;
                gameManager.time= (float)TimeSpan.FromMilliseconds(state.time).TotalSeconds;                    
                gameManager.currentTurnOf=Mathf.RoundToInt(state.turn);
                gameManager.OnServerUpdate();
                roomconfig.roomName=state.fee;
                roomconfig.total=state.win;
            if(state.winner!=""){
                Debug.Log("there is winner");
                gameManager.FindWinner(state.winner);
            }
        }
        }
       
    }
    void  OnJoin()
    {   
        SetUpEvents();
        GameEvents.notify_event_invoke(" joined room - " + room.Name, Color.green);
    }
    public async void JoinOrCreate(string room_name,int max)
    {  

        var options=new Dictionary<string, object>();
        options.Add("maxPlayers",max);
        options.Add("maxClients",max);
        options.Add("id",PlayerPrefs.GetString("_id"));
        options.Add("room_id",roomconfig.roomid);
        options.Add("user_profile",PlayerPrefs.GetInt("userprofile"));
        options.Add("user_name",PlayerPrefs.GetString("username"));
        options.Add("fee",roomconfig.roomName);
        options.Add("win",roomconfig.total);
        
        room = await client.JoinOrCreate<State>(room_name,options);
        roomconfig.col_roomid=room.Id;
         lobbystate= room.State;

        SetUpEvents();

        //save information of room
        PlayerPrefs.SetInt("gameover",0);
        PlayerPrefs.SetInt("bot",0);
        PlayerPrefs.SetString("previous_room_id",room.Id);
        PlayerPrefs.SetString("previous_session_id",room.SessionId);
        PlayerPrefs.SetInt("previous_max_players",roomconfig.maxPlayers);
        PlayerPrefs.Save();
    }
    public async void JoinRoom(string coly_roomid, string room_name,int max,int bot = 0)
    {
        var options=new Dictionary<string, object>();
        options.Add("id",PlayerPrefs.GetString("_id"));
        options.Add("room_id",roomconfig.roomid);
        options.Add("maxPlayers",max);
        options.Add("maxClients",max);
        options.Add("user_profile",PlayerPrefs.GetInt("userprofile"));
        options.Add("user_name",PlayerPrefs.GetString("username"));
        options.Add("fee",roomconfig.roomName);
        options.Add("room_type","private");

        if(coly_roomid != "")
        {
            try{    
                room = await client.JoinById<State>(coly_roomid, options);
                lobbystate= room.State;
            }catch(MatchMakeException ex){
                GameEvents.notify_event_invoke(ex.Message,Color.red);
            }
            roomconfig.col_roomid=room.Id;
        }
        SetUpEvents();
        //save information of room
        PlayerPrefs.SetInt("gameover",0);
        PlayerPrefs.SetInt("bot",bot);
        PlayerPrefs.SetString("previous_room_id",room.Id);
        PlayerPrefs.SetString("previous_session_id",room.SessionId);
        PlayerPrefs.SetInt("previous_max_players",roomconfig.maxPlayers);
        PlayerPrefs.Save();
    }
    public async void CreateRoom(string room_name,int max)
    {   
        var options=new Dictionary<string, object>();
        options.Add("maxPlayers",max);
        options.Add("id",PlayerPrefs.GetString("_id"));
        options.Add("user_name",PlayerPrefs.GetString("username"));
        options.Add("user_profile",PlayerPrefs.GetInt("userprofile"));
        options.Add("room_id",roomconfig.roomid);
        options.Add("room_type","private");
        options.Add("fee",roomconfig.roomName);
        options.Add("win",roomconfig.total);
        room = await client.Create<State>(room_name,options);
        roomconfig.col_roomid=room.Id;
         lobbystate= room.State;

        SetUpEvents();

        //save information of room
        PlayerPrefs.SetInt("gameover",0);
        PlayerPrefs.SetInt("bot",0);
        PlayerPrefs.SetString("previous_room_id",room.Id);
        PlayerPrefs.SetString("previous_session_id",room.SessionId);
        PlayerPrefs.SetInt("previous_max_players",roomconfig.maxPlayers);
        PlayerPrefs.Save();
        
    }
    public Dictionary<string,object> CreateRoomOptions(){

        var options=new Dictionary<string, object>();
        options.Add("maxPlayers",roomconfig.maxPlayers);
        options.Add("id",PlayerPrefs.GetString("_id"));
        options.Add("user_name",PlayerPrefs.GetString("username"));
        options.Add("user_profile",PlayerPrefs.GetInt("userprofile"));
        options.Add("room_id",roomconfig.roomid);
        options.Add("room_type","private");
        options.Add("fee",roomconfig.roomName);
        options.Add("win",roomconfig.total);
        return options;
    }
    public async void ReconnectRoom()
    {   
        reconnectScreen=true;
        client =  ColyseusManager.Instance.CreateClient(MainUrls.SOCKET_URL);
        reconnected=false;
        await Task.Delay(5000);
        try{
                //connect with the room
                room = await client.Reconnect<State>( PlayerPrefs.GetString("previous_room_id"), PlayerPrefs.GetString("previous_session_id") );
                roomconfig.col_roomid=room.Id;
                lobbystate= room.State;
                //listin for room events
                SetUpEvents();
                //save information of room
                PlayerPrefs.SetInt("gameover",0);
                PlayerPrefs.SetString("previous_room_id",room.Id);
                PlayerPrefs.SetString("previous_session_id",room.SessionId);
                PlayerPrefs.SetInt("previous_max_players",roomconfig.maxPlayers);
                PlayerPrefs.Save();

            }catch(MatchMakeException ex){

                GameEvents.notify_event_invoke("game has been finished "+ex.Message,Color.red);
            }
        reconnectScreen=false;
    }
    public void Clean(){

        clientPlayers = new Dictionary<string, IActionable<int,int>>();
        lobby=new List<Player>();
        room =null;

    }
    private void OnPlayerAdd(Player player, string _id)
    {
         Debug.Log(player.name + " joined");
        if (!lobby.Exists(p => (p==player))){
            lobby.Add(player);
        }
        if(_id!=room.SessionId){
            GameEvents.invoke_user_join_event(1,player.userprofile,player.name);
            GameEvents.notify_event_invoke(player.name+" added in the lobby",Color.green);
        }else{
             GameEvents.notify_event_invoke("you"+" added in the lobby",Color.green);
        }
        if(lobby.Count==roomconfig.maxPlayers){
           
            lobby.ForEach(l=>{
               playerboards.ForEach(p =>
               {
                   if (l.seat == p._Seat)
                   {
                       clientPlayers.Add(l.sessionId, p);

                       if (l.sessionId == room.SessionId)
                       {
                            clientPlayers[l.sessionId]._BoardOf = PlayerMode.Human;
                           if (board)
                           {
                               board.colorselected = clientPlayers[l.sessionId]._homeColor;
                               Send(new Message { playerPhase = "connected", connected = true });
                           }
                       }
                       else
                       {    
                           if( clientPlayers[l.sessionId]._BoardOf!=PlayerMode.Bot){
                            clientPlayers[l.sessionId]._BoardOf = PlayerMode.Dummy;

                           }
                       }
                       clientPlayers[l.sessionId]._uiarea.profileImage = l.userprofile;
                       clientPlayers[l.sessionId]._uiarea.username = l.name;
                       clientPlayers[l.sessionId]._Id = l.sessionId;
                       l.mytokens.OnChange+= clientPlayers[l.sessionId].OnTokenChange;
                       
                   }
               });

            });
            board.ArrengeBoard();
            GameStartedSet();
            

        }
    }
    async void GameStartedSet(){
        await Task.Delay(8000);
        PlayerPrefs.SetInt("gameover",0);
    }
    private void OnPlayerChange(Player player, string _id)
    {
        
        if (clientPlayers.ContainsKey(_id))
        {
              switch (player.playerPhase)
            {
                case "roll_dice":
                // Debug.Log("get msg of roll dice");
                    if(!clientPlayers[_id]._isRolledDice){
                        clientPlayers[_id].OnDiceRoll(Mathf.RoundToInt(player.dicevalue));
                    }
                    break;
                case "dice_rolled":
                    break;
                case "open_token":
                  if(clientPlayers[_id]._CanOpenToken){
                     clientPlayers[_id].OnOpenToken(Mathf.RoundToInt(player.tokenid));
                  }
                    break;
                case "move_token":
                    if(!clientPlayers[_id]._isMovedToken){
                        clientPlayers[_id].OnMoveToken(Mathf.RoundToInt(player.tokenid),player.tknOldPos);
                    }
                    break;
                case "token_moving":
                    break;
                case "finished_turn":
                    clientPlayers[_id].OnTurnFinished();
                    break;
                case "extra_turn":
                    if(_id==room.SessionId && player.goaled < 4){
                        GameEvents.invoke_dialogbox("Extra turn");
                    }
                    clientPlayers[_id].OnExtraTurn();
                    // active=  playerboards.Find(p => p._Seat == room.State.turn);
                    // if(active!=null){
                    //     active.OnTurn();
                    // }
                    break;
                
                case "hit_token":
                    // Debug.Log("hitted "+ _id);
                    clientPlayers[_id].OnHit(Mathf.RoundToInt(player.tokenid));
                    break;
                    case "goaled_token":
                    // Debug.Log("goaled "+ _id);
                    clientPlayers[_id].OnGoaledToken(Mathf.RoundToInt(player.tokenid));
                    break;
                     case "connected":
                        clientPlayers[_id]._connected=player.connected;
                    break;
            }

         
            // clientPlayers[_id]._myTokens[]
           
            clientPlayers[_id]._islazy=player.Islazy;
            clientPlayers[_id]._isMovedToken=player.isMovedToken;
            clientPlayers[_id]._CanOpenToken=player.CanOpenToken;
            clientPlayers[_id]._isRolledDice=player.isRolledDice;
            clientPlayers[_id]._goaledtokens=Mathf.RoundToInt(player.goaled);
            clientPlayers[_id]._opentokens=Mathf.RoundToInt(player.opened);
            clientPlayers[_id]._lazymoves=Mathf.RoundToInt(player.lazymoves);
            
        //    clientPlayers[_id]._myTokens.ForEach((t) =>
        //     {
        //         TokenState tokenState;
        //         player.mytokens.TryGetValue((t.id).ToString(), out tokenState);
                
        //         t.isOpened=tokenState.isOpened;
        //         t.isGoaled=tokenState.isGoaled;
                
        //     });

            if(clientPlayers[_id]._uiarea.userpanel){
                clientPlayers[_id]._uiarea.userpanel.lazymoves.text=player.lazymoves.ToString();
                clientPlayers[_id]._uiarea.userpanel.goaled.text=player.goaled.ToString();
                clientPlayers[_id]._uiarea.userpanel.opened.text=player.opened.ToString();
            }

            if(player.sessionId==room.SessionId){

                if(clientPlayers[_id]._lazymoves>=4){
                 kick();
                }
               
            }
                
        }
    }
    private void OnPlayerRemove(Player player, string _id)
    {
        if (gameManager.gamePhase == "waiting")
        {

            if (!lobby.Exists(p => (p.sessionId == _id))) return;

            lobby.Remove(player);
            if (_id != room.SessionId)
            {
                GameEvents.invoke_user_join_event(0, player.userprofile, player.name);
                GameEvents.notify_event_invoke(player.name + " left the lobby", Color.yellow);
            }
        }
        else
        {
            GameEvents.notify_event_invoke(player.name + " leaved the game", Color.yellow);
            if (clientPlayers.ContainsKey(_id))
            {
                homeManager leavedhome = gameManager.homes.Find((h) => (h.Id == _id));
                if (leavedhome)
                {
                    if (leavedhome.mytokens != null)
                        leavedhome.mytokens.ForEach(t =>
                        {
                            Destroy(t.gameObject);
                        });

                    leavedhome.mytokens = new List<token>();
                    leavedhome.GetComponent<Animator>().SetBool("turn", false);
                    Destroy(leavedhome.uiArea.userpanel.gameObject);
                    gameManager.homes.Remove(leavedhome);
                }
                clientPlayers[_id].OnLeave();
                clientPlayers.Remove(_id);
            }
        }
    }
    void OnMessage(object msg)
    {
        GameEvents.notify_event_invoke(msg.ToString(),Color.yellow);
        if (msg is Message)
        {
            var data = (Message)msg;
            if (data.playerPhase == "winner")
            {          
                clientPlayers[data.id].OnLeave();
            }
        }
    }
    public async void Send(object msg)
    {
        if (msg is Message)
        {
            if (room != null)
            {
                await room.Send("*",msg);
            }
        }
    }
    void OnLeave(NativeWebSocket.WebSocketCloseCode code)
    {     
        
    }
    public void SyncClient(State state)
    {
        int count=0;
        state.players.ForEach((id, p) =>
        {
            if(clientPlayers.ContainsKey(id))
            clientPlayers[id]._myTokens.ForEach((t) =>
            {
                TokenState tokenState;

                p.mytokens.TryGetValue((t.id).ToString(), out tokenState);
               
                count++;
                // GameEvents.notify_event_invoke(t.name+ " is on pos = " +tokenState.pos,Color.black);
                
                
                t.SyncPosition(tokenState.pos);
               
               
                t.isOpened=tokenState.isOpened;
                t.isGoaled=tokenState.isGoaled;
                
            });
        });
        Send( new Message{playerPhase="synced"});
    }
    public async void Leave(bool clearConfig = true )
    {
        mainClientBoard = null;
        if(clearConfig){
        roomconfig = new RoomConfig();
        }
        if(gameManager){
            gameManager.homes=new List<homeManager>();

        }
        lobby=new List<Player>();
        clientPlayers = new Dictionary<string, IActionable<int, int>>();
        await room.Leave(true);
        
    }
    public void OnDestroy(){
        clientPlayers = new Dictionary<string, IActionable<int,int>>();
    }
    async void kick(){
      
        gameManager.FindWinner("kicked");
        await Task.Delay(200);
        Leave();
    }
    async void OnApplicationPause( bool pauseStatus )
    {
        if(pauseStatus){
            Send(new Message{playerPhase="connected", connected=false});
        }else{
            if(gameManager){
                gameManager.connected=false;
            }
            await Task.Delay(6000);

            if(gameManager){
                  gameManager.connected=true;
            }
        } 
    }
}

