
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public UiThings UIArea;
    public float nextTurnTime = 15f;
    public int totalPlayers = 4;
    public int currentTurnOf = 1;
    public int playingAs = 1;
    [SerializeField]
    public float time = 0;
    public string gamePhase = "waiting";
    public Dictionary<homeManager, int> playcounts = new Dictionary<homeManager, int>();
    public List<homeManager> homes;
    public string safeStarTag;
    public bool connected = true, gameOver = false;
    public bool automodecheck = true;
    private AudioSource source;
    public bool isSocketConnectionLost, hasinternet = true;
    void Start()
    {
        InvokeRepeating("CheckInternet", 2, 1);
        if (UIArea.waitingPage == null)
        {
            UIArea.waitingPage = GameObject.FindGameObjectWithTag("findingpage");
        }

        UIArea.Board = GameObject.FindWithTag("Board");
        UIArea.TurnInfo.text = UIArea.turnSlung + "" + currentTurnOf.ToString();
        homes = GameObject.FindObjectsOfType<homeManager>().ToList();
        source = GetComponent<AudioSource>();

    }
    void CheckInternet()
    {
        if (!gameOver)
        {

            if (!NativeInternet.IsNetworkAvailableStatic)
            {

                // UIArea.networklostpage.SetActive(true);
                hasinternet = false;

            }
            else
            {
                hasinternet = true;
            }
        }

    }
    void Update()
    {

        // //test
        GameClient._Instance.SocketStrenth -= Time.deltaTime;
        if (hasinternet)
        {
            if (gamePhase == "playing" || gamePhase == "out_of_time")
            {
                if (!gameOver && GameClient._Instance.SocketStrenth < 1)
                {
                    isSocketConnectionLost = true;
                }
                else
                {
                    isSocketConnectionLost = false;
                }
            }
            else
            {
                isSocketConnectionLost = false;
            }

        }
        else
        {
            isSocketConnectionLost = true;
        }
        UIArea.networklostpage.SetActive(isSocketConnectionLost);

        AnimatingTurnHome();
        PhaseUpdate();

        if (connected && GameClient._Instance.reconnectScreen == false)
        {
            UIArea.reconnectingPage.SetActive(false);
        }
        else
        {
            UIArea.reconnectingPage.SetActive(true);
        }
        // for Reconnect when packet loss // socket strenth get resfresh after every patch onStateChange
        // GameClient._Instance.SocketStrenth -= Time.deltaTime;

    }
    public void PhaseUpdate()
    {
        if (!GameClient._Instance.reconnectScreen)
        {
            if (gamePhase == "waiting")
            {
                UIArea.reconnectingPage.SetActive(true);

            }
            else
            {
                UIArea.reconnectingPage.SetActive(false);
            }

        }
        {
            if (gamePhase == "waiting")
            {
                UIArea.waitingPage.SetActive(true);

            }
            else
            {
                UIArea.waitingPage.SetActive(false);
            }
        }
    }
    public void OnServerUpdate()
    {
        CountingForNextTurn();
    }
    void AnimatingTurnHome()
    {
        foreach (homeManager h in homes)
        {
            if (currentTurnOf == h.Seat)
            {
                h.uiArea.homeAnim.SetBool("turn", true);

                if (h.uiArea.userpanel)
                {
                    h.uiArea.userpanel.dice.gameObject.SetActive(true);
                    h.uiArea.userpanel.Timer.gameObject.SetActive(true);
                    h.uiArea.userpanel.UpdateTime(Mathf.RoundToInt(time));
                }


            }
            else
            {

                h.uiArea.homeAnim.SetBool("turn", false);
                if (h.uiArea.userpanel)
                {
                    h.uiArea.userpanel.dice.gameObject.SetActive(false);
                    h.uiArea.userpanel.Timer.gameObject.SetActive(false);
                }

            }
        }
    }
    public async void FindWinner(string id)
    {

        if (gameOver)
        {
            return;
        }

        PlayerPrefs.SetInt("gameover", 1);
        PlayerPrefs.Save();
        UIArea.EndPanelAnim.SetBool("pop", true);
        gamePhase = "gameover";
        isSocketConnectionLost = true;


        UIArea.Board.SetActive(false);
        if (id == GameClient.room.SessionId)
        {

            UIArea.EndPanelState.text = "You Won :)";
            // UIArea.wonprice.text="+"+GameClient._Instance.roomconfig.total;
            UIArea.wingpage.SetActive(true);
            UIArea.losepage.SetActive(false);
            if (PlayerPrefs.GetInt("sound") == 1)
                source.PlayOneShot(UIArea.win);
            gameOver = true;

        }
        else if (id == "kicked")
        {

            // UIArea.kickedloseprice.text="-"+GameClient._Instance.roomconfig.roomName;
            UIArea.wingpage.SetActive(false);
            UIArea.kickedpage.SetActive(true);

            if (PlayerPrefs.GetInt("sound") == 1)
                source.PlayOneShot(UIArea.lose);

            gameOver = true;

        }
        else
        {
            // UIArea.loseprice.text="-"+GameClient._Instance.roomconfig.roomName;
            UIArea.wingpage.SetActive(false);
            UIArea.losepage.SetActive(true);

            if (PlayerPrefs.GetInt("sound") == 1)
                source.PlayOneShot(UIArea.lose);

            gameOver = true;
        }



        await Task.Delay(MainUrls.AUTO_EXIT_MENU);

        settings._Instance.leave();

    }
    void CountingForNextTurn()
    {
        if (UIArea.timer)
            UIArea.timer.text = UIArea.timerSlung + "" + Mathf.RoundToInt(time).ToString();
    }
    public void ArrangeTokensOfSameTile(List<token> tkns, Vector3 inpos)
    {
        float four = .4f;
        float three = .3f;
        float two = .2f;
        float one = .1f;

        for (int i = 0; i < tkns.Count; i++)
        {
            if (tkns.Count > 1)
            {
                tkns[i].sp.sortingOrder = 4 + i;
            }
            else
            {
                tkns[i].sp.sortingOrder = 4;
            }

            switch (tkns.Count)
            {
                case 1:

                    switch (i)
                    {
                        case 0:
                            tkns[i].movetopos = new Vector3(inpos.x, inpos.y, inpos.z);
                            break;
                    }
                    break;
                case 2:
                    switch (i)
                    {
                        case 0:
                            tkns[i].movetopos = new Vector3(inpos.x - two, inpos.y, inpos.z);
                            break;
                        case 1:
                            tkns[i].movetopos = new Vector3(inpos.x + two, inpos.y, inpos.z);
                            break;


                    }

                    break;
                case 3:
                    switch (i)
                    {
                        case 0:
                            tkns[i].movetopos = new Vector3(inpos.x - three, inpos.y, inpos.z);
                            break;
                        case 1:
                            tkns[i].movetopos = new Vector3(inpos.x, inpos.y, inpos.z);
                            break;
                        case 2:
                            tkns[i].movetopos = new Vector3(inpos.x + three, inpos.y, inpos.z);
                            break;

                    }

                    break;
                case 4:
                    switch (i)
                    {
                        case 0:
                            tkns[i].movetopos = new Vector3(inpos.x - three, inpos.y, inpos.z);
                            break;
                        case 1:
                            tkns[i].movetopos = new Vector3(inpos.x - three + one, inpos.y, inpos.z);
                            break;
                        case 2:
                            tkns[i].movetopos = new Vector3(inpos.x + three, inpos.y, inpos.z);
                            break;
                        case 3:
                            tkns[i].movetopos = new Vector3(inpos.x + three - one, inpos.y, inpos.z);
                            break;


                    }

                    break;
                case 5:
                    switch (i)
                    {
                        case 0:
                            tkns[i].movetopos = new Vector3(inpos.x - four, inpos.y, inpos.z);
                            break;
                        case 1:
                            tkns[i].movetopos = new Vector3(inpos.x - four + two, inpos.y, inpos.z);
                            break;
                        case 2:
                            tkns[i].movetopos = new Vector3(inpos.x, inpos.y, inpos.z);
                            break;
                        case 3:
                            tkns[i].movetopos = new Vector3(inpos.x + four, inpos.y, inpos.z);
                            break;
                        case 4:
                            tkns[i].movetopos = new Vector3(inpos.x + four - two, inpos.y, inpos.z);
                            break;

                    }
                    break;

            }
            if (tkns.Count != 0 && tkns.Count > 1)
            {

                tkns[i].myscale = ((new Vector3(1f, 1f, 1f) / tkns.Count) * 1.5f);
                tkns[i].availableScale(0, Vector3.one);

            }
            else
            {
                tkns[i].myscale = Vector3.one;
                tkns[i].availableScale(0, Vector3.one);
            }
        }
    }

}

[System.Serializable]
public class UiThings
{
    public Text timer;
    public string timerSlung;
    public Text TurnInfo;
    public string turnSlung;
    public Animator EndPanelAnim;
    public Text EndPanelState, wonprice, loseprice, kickedloseprice, g_phase;
    public GameObject waitingPage;
    public GameObject reconnectingPage, networklostpage;
    public GameObject Board, wingpage, losepage, kickedpage;
    public AudioClip win, lose;


}

