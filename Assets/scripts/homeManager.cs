using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Runtime;
using System;
using MyInterfaces;
using System.Linq;

[System.Serializable]
public class timestamps
{
    public string action;
    public int dicevalue;
    public float time;
}
[System.Serializable]
public class HmUiThings
{
    public Animator homeAnim;
    public Dice dice;
    public Text diceValue;
    public Button diceButton;
    public AudioClip rollClip;
    public PlayPanel userpanel;
    public string username;
    public int profileImage;
}
public enum ColorModes
{
    green, yellow, blue, red,
}
public enum PlayerMode
{
    Human, Dummy, Bot
}
public class homeManager : MonoBehaviour, IActionable<int, int>
{
    public List<timestamps> playerlog = new List<timestamps>();
    public PlayerMode BoardOf;
    public HmUiThings uiArea;
    public ColorModes homecolor;
    public GameObject diceRoller, leavedObject;
    public int Seat;
    public string Id;
    public int diceMinValue = 4, diceMaxValue = 7;
    public float diceRollDelay = .5f;
    public GameObject mytoken_Prefab;
    public Transform[] homeRooms;
    public Transform panelSpawnPoint;
    public GameObject panelPrefab;
    public List<token> mytokens;
    public List<int> valueCount = new List<int>();
    public int goalPos = 57;
    public GameManager gm;
    public PathProvider pp;
    private AudioSource source;
    List<token> moveabletkns = new List<token>();
    List<token> openabletkns = new List<token>();
    public bool canOpenMyToken, isDiceRolled, isMovedToken, isPlayedTurn, isLazy, connected, arrange;
    public int diceRandomValue, openedTokens, goaledTokens, lazyMoves;

    [Header("currently Moving token")]
    public token movingtoken;
    public Vector3 movetopos;
    public string _Id
    {
        get => this.Id;
        set => this.Id = value;
    }
    public int _Seat
    {
        get => this.Seat;
        set => this.Seat = value;
    }
    public int _diceRandomValue
    {
        get => this.diceRandomValue;
        set => this.diceRandomValue = value;
    }
    public bool _islazy
    {
        get => this.isLazy;
        set => this.isLazy = value;
    }
    public bool _connected
    {
        get => this.connected;
        set => this.connected = value;
    }
    public List<token> _myTokens
    {
        get => this.mytokens;
        set => this.mytokens = value;
    }
    public PlayerMode _BoardOf
    {
        get => this.BoardOf;
        set => this.BoardOf = value;
    }
    public int _lazymoves
    {
        get => lazyMoves; set => lazyMoves = value;
    }
    public int _opentokens
    {
        get => openedTokens; set => openedTokens = value;
    }
    public int _goaledtokens
    {
        get => this.goaledTokens; set => goaledTokens = value;
    }
    public bool _isMovedToken
    {
        get => isMovedToken;
        set => isMovedToken = value;
    }
    public bool _isRolledDice
    {
        get => this.isDiceRolled;
        set => isDiceRolled = value;
    }
    public bool _CanOpenToken
    {
        get => canOpenMyToken;
        set => canOpenMyToken = value;
    }
    public HmUiThings _uiarea
    {
        get => this.uiArea;
        set => this.uiArea = value;

    }
    public ColorModes _homeColor
    {
        get => this.homecolor;
        set => this.homecolor = value;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
    
        // if (Input.GetKeyDown(KeyCode.Space) && this.BoardOf == PlayerMode.Human)
        // {
        //     GameClient._Instance.SyncClient();
        // }
        // if (movingtoken != null)
        // {
        //     if (movingtoken.itsOnAutoMove)
        //     {
        //          moveToken();
        //     }
        // }
    }
    public void SpawnTokens()
    {
        for (int i = 0; i < homeRooms.Length; i++)
        {
            token tkn = Instantiate(mytoken_Prefab, homeRooms[i].position, Quaternion.identity).GetComponent<token>();
            tkn.hm = this;
            tkn.color = this.homecolor;
            tkn.pp = pp;
            tkn.gm = gm;
            tkn.id = i;
            mytokens.Add(tkn);
        }
    }
    public void DestroyTokens()
    {
        mytokens.ForEach((t) =>
        {
            Destroy(t.gameObject);
        });
        mytokens = new List<token>();
    }
    public void SpawnPanel()
    {
        GameObject panel = GameObject.Instantiate(panelPrefab, panelSpawnPoint.position, Quaternion.identity);
        uiArea.userpanel = panel.GetComponent<PlayPanel>();
    }
    public void DestroyPanel()
    {
        Destroy(uiArea.userpanel.gameObject);
    }
    public void SetDiceOnClick()
    {
        uiArea.userpanel.diceButton.onClick.AddListener(() =>
        {
            if (BoardOf == PlayerMode.Human)
            {
                if (gm.currentTurnOf == Seat && isDiceRolled == false)
                {
                    //send my client data to server
                    GameClient._Instance.Send(new Message { playerPhase = "roll_dice", dicevalue = diceMaxValue });
                    //isDiceRolled=true;
                }
            }
        });
    }
    public async void rollDice()
    {
        if (gm.currentTurnOf == Seat && isDiceRolled == false)
        {
            uiArea.userpanel.dice.roll();
            await Task.Delay(300);
            uiArea.userpanel.dice.stop(diceRandomValue);
            if (!isLazy)
            {
                if (BoardOf == PlayerMode.Human)
                {
                    AutoMoveActions();
                }
                else if (BoardOf == PlayerMode.Bot)
                {
                    BotActions();
                }
            }
        }
    }
    void AutoMoveActions()
    {
        moveabletkns = mytokens.FindAll(t => (t.isOpened) && (!t.isGoaled) && (t.GetNextPostion() <= this.goalPos));
        int moveabletknscount = moveabletkns.Count;
        openabletkns = mytokens.FindAll(t => (!t.isOpened) && (!t.isGoaled) && (t.GetNextPostion() <= this.goalPos));
        int openabletknscount = openabletkns.Count;

        if (moveabletknscount == 0 && openabletknscount == 0)
        {
            // await Task.Delay(300);
            // SendIplayed();
            //  GameEvents.notify_event_invoke("auto finished 1",Color.yellow);
        }
        else
        {

            if (diceRandomValue == 6)
            {

                availableall(true);

                if (moveabletknscount < 1 && openabletknscount > 0)
                {

                    AutoOpen();

                }
                else if (moveabletknscount < 2 && openabletknscount == 0)
                {

                    AutoMove();

                }

            }
            else
            {

                available(true);

                if (moveabletknscount > 0)
                {
                    if (moveabletknscount < 2)
                    {
                        AutoMove();
                    }
                }
                else
                {
                    // await Task.Delay(300);
                    // SendIplayed();
                    // GameEvents.notify_event_invoke("auto finished 2",Color.yellow);
                }
            }
        }
        void AutoOpen()
        {
            foreach (token t in openabletkns)
            {
                GameClient._Instance.Send(new Message { playerPhase = "open_token", tokenid = t.id });

                break;
            }
        }
        void AutoMove()
        {
            foreach (token t in moveabletkns)
            {
                GameClient._Instance.Send(new Message { playerPhase = "move_token", tokenid = t.id, tokenpos = t.GetNextPostion() });
                break;
            }
        }
    }
    void BotActions()
    {
        var calculatedtkns = GetCalculateTokens();
        int calculatedtknscount =  calculatedtkns.Count;
        moveabletkns = mytokens.FindAll(t => (t.isOpened) && (!t.isGoaled) && (t.GetNextPostion() <= this.goalPos));
        int moveabletknscount = moveabletkns.Count;
        openabletkns = mytokens.FindAll(t => (!t.isOpened) && (!t.isGoaled) && (t.GetNextPostion() <= this.goalPos));
        int openabletknscount = openabletkns.Count;
        var goalabletkns = mytokens.FindAll(t => (t.isOpened) && (!t.isGoaled) && (t.GetNextPostion() == this.goalPos));

        if (moveabletknscount == 0 && openabletknscount == 0)
        {

        }
        else
        {
            if (diceRandomValue == 6)
            {
                availableall(true);
                if (moveabletknscount < 1 && openabletknscount > 0)
                {
                    BotAutoOpen();
                }
                else if (goalabletkns.Count > 0)
                {
                    BotGaol();
                }
                else if (moveabletknscount < 2 && openabletknscount == 0)
                {
                    BotAutoMove();
                }
                else if (openabletknscount < 1)
                {
                    BotMove();
                }
                else
                {
                    if (UnityEngine.Random.Range(0, 1) == 1)
                    {
                        BotMove();
                    }
                    else
                    {
                        BotOpen();
                    }
                }
            }
            else
            {
                available(true);
                if (moveabletknscount > 0)
                {
                    BotMove();
                }
            }
        }
        void BotAutoOpen()
        {
            token to = openabletkns[UnityEngine.Random.Range(0, openabletkns.Count)];
            BotClient._Instance.Send(new Message { playerPhase = "open_token", tokenid = to.id });
        }
        void BotAutoMove()
        {
            token tm = moveabletkns[UnityEngine.Random.Range(0, moveabletkns.Count)];
            BotClient._Instance.Send(new Message { playerPhase = "move_token", tokenid = tm.id, tokenpos = tm.GetNextPostion() });
        }
        async void BotOpen()
        {
            await Task.Delay(1000 * UnityEngine.Random.Range(1, 6));
            token to = openabletkns[UnityEngine.Random.Range(0, openabletkns.Count)];
            BotClient._Instance.Send(new Message { playerPhase = "open_token", tokenid = to.id });
        }
        async void BotMove()
        {
            token tm = moveabletkns[UnityEngine.Random.Range(0, moveabletkns.Count)];
            if(calculatedtknscount>0){
                tm =calculatedtkns[UnityEngine.Random.Range(0, calculatedtkns.Count)];
            }
            await Task.Delay(1000 * UnityEngine.Random.Range(1, 6));
            BotClient._Instance.Send(new Message { playerPhase = "move_token", tokenid = tm.id, tokenpos = tm.GetNextPostion() });
        }
        async void BotGaol()
        {
            await Task.Delay(1000 * UnityEngine.Random.Range(1, 6));
            token tm = goalabletkns[UnityEngine.Random.Range(0, goalabletkns.Count)];
            BotClient._Instance.Send(new Message { playerPhase = "move_token", tokenid = tm.id, tokenpos = tm.GetNextPostion() });
        }
    }
    List<token> GetCalculateTokens()
    {
        var calculatedtkns = new List<token>();
        gm.homes.ForEach(h =>
        {
            if(h==this)return;
            var opptokensData = h.GetTokenLocations();
            mytokens.ForEach(t =>
            {
                if(!t.isOpened||t.GetNextPostion()>goalPos)return;
                var tileid = t.GetNextTileId();
                if(opptokensData.ContainsKey(tileid)){
                    if(opptokensData[tileid]<2){
                        calculatedtkns.Add(t);
                    }
                }
            });
        });  
        string names ="";
        calculatedtkns.ForEach(t =>{
             names+=t.gameObject.name+" ";
        });
        Debug.Log("CalculatedTokens:- "+ names);
        return calculatedtkns;
    }
    public Dictionary<int, int> GetTokenLocations()
    {
        Dictionary<int, int> data = new Dictionary<int, int>();
        mytokens.ForEach(t =>
        {
            if(!t.isOpened)return;
            if (data.ContainsKey(t.GetTileId()))
            {
                data[t.GetTileId()]++;
            }
            else
            {
                data.Add(t.GetTileId(), 1);
            }
        });
        return data;
    }
    void availableall(bool b)
    {

        moveabletkns.ForEach(t =>
        {
            t.available(b);
            t.availableScale(1, Vector3.one);
        });
        openabletkns.ForEach(t =>
        {
            t.available(b);
        });

    }
    void available(bool b)
    {
        moveabletkns.ForEach(t =>
        {
            t.available(b);
            t.availableScale(1, Vector3.one);
        });
    }
    public async void OnTurn()
    {
        Debug.Log("OnTurn-" + name);
        if (BoardOf == PlayerMode.Bot)
        {
            await Task.Delay(1000 * UnityEngine.Random.Range(1, 2));
            if (gm.currentTurnOf == Seat && isDiceRolled == false)
            {
                Debug.Log("OnTurn bot dice roll");
                BotClient._Instance.Send(new Message { playerPhase = "roll_dice", dicevalue = diceMaxValue });
            }
        }
    }
    public void OnDiceRoll(int value)
    {

        playerlog.Add(new timestamps { action = "dice roll", dicevalue = this.diceRandomValue, time = gm.time });
        this.diceRandomValue = value;
        rollDice();

        if (PlayerPrefs.GetInt("sound") == 1)
            source.PlayOneShot(uiArea.rollClip);

    }
    public void OnTurnFinished()
    {
        availableall(false);
        //change  turn on client side when get msg of finished
        moveabletkns.ForEach(t =>
        {
            t.availableScale(0, Vector3.one);
        });
        openabletkns.ForEach(t =>
        {
            t.availableScale(0, Vector3.one);
        });

    }
    public void OnExtraTurn()
    {
        Debug.LogWarning(uiArea.username + " has extra turn");
        foreach (token t in mytokens)
        {
            t.itsOnAutoMove = false;
        }
    }
    public void OnGoaledToken(int id)
    {
        mytokens.ForEach((t) =>
        {
            if (t.id == id)
            {
                t.GoalSound();
            }
        });
    }
    public void OnMoveToken(int id, int nextpos)
    {
        playerlog.Add(new timestamps { action = "move token id = " + id + " pos = " + nextpos, dicevalue = this.diceRandomValue, time = gm.time });
        availableall(false);
        mytokens.ForEach(tkn =>
        {
            if (tkn.id == id)
            {
                tkn.StartCoroutine("prepareMove", nextpos);
            }
        });
    }
    public void OnOpenToken(int id)
    {
        availableall(false);
        mytokens.ForEach(tkn =>
            {
                if (tkn.id == id)
                {
                    tkn.openToken();
                }
            });
    }
    public void OnHit(int tid)
    {
        mytokens.ForEach(t =>
        {
            if (t.id == tid)
            {
                t.KillSound();
                t.ReqArrange(0);
                t.GoBackToHome();
            }
        });
    }
    public void OnTokenChange(TokenState ts, string id)
    {
        int i = int.Parse(id);
        token tkn = mytokens.Find(t => (t.id == i));
        tkn.isOpened = ts.isOpened;
        tkn.isGoaled = ts.isGoaled;
    }
    public void OnLeave()
    {
        mytokens.ForEach(t =>
        {
            Destroy(t.gameObject);
        });
        Instantiate(leavedObject, transform.position, Quaternion.identity);
    }
}
