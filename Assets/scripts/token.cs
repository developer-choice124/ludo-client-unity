using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInterfaces;
using System.Threading.Tasks;

[System.Serializable]
public class TokenSfx
{
    public AudioClip jumpclip, goalclip, openclip, killclip;
}
public class token : MonoBehaviour
{
    public TokenSfx sfx;
    public GameObject goalvfx;
    public ColorModes color;
    public homeManager hm;// to know my home class
    public GameManager gm; //game manager class of the game
    public PathProvider pp; //path provider class to know which is my path to move
    public bool isOpened, isGoaled, itsOnAutoMove;//to keep track of this tokens states
    public int oldPos, id, othertokenscount;
    public Vector3 movetopos;
    private int GetdiceValue;
    private Vector3 myHome;
    private int TempPosTokens = 0;
    private bool stopSettingOldPos;
    private bool inSafePos;
    private Animator anim;
    public SpriteRenderer sp;
    private AudioSource source;
    public Vector3 myscale = Vector3.one;
    private List<token> OtherTokensInMyPos = new List<token>();
    void Awake()
    {
        movetopos = transform.position;
        myHome = transform.position;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (this != null)
        {
            moveToken();
        }
    }
    public void availableScale(int mode, Vector3 s)
    {
        if (mode == 0)
        {
            transform.localScale = myscale;
        }
        else
        {
            transform.localScale = s;
        }
    }
    public void available(bool b)
    {
        anim.SetBool("available", b);
        if (b)
        {
            sp.sortingOrder = 6;
        }
        else
        {
            sp.sortingOrder = 4;
        }
    }
    public void OnMouseDown()
    {

        if (isOpened == true && gm.currentTurnOf == hm.Seat && hm.isDiceRolled && !hm._isMovedToken && !hm._islazy
            && hm.isMovedToken == false && itsOnAutoMove == false && this.GetNextPostion() < hm.goalPos + 1)
        {
            //send my client data to server
            if (hm.BoardOf == PlayerMode.Human)
            {

                GameClient._Instance.Send(new Message { playerPhase = "move_token", tokenid = id, tokenpos = this.GetNextPostion(), isMovedToken = true });
            }
            // StartCoroutine("prepareMove", GetNextPostion());
        }
        if (hm.canOpenMyToken == true && isOpened == false && isGoaled == false && !hm._islazy)
        {

            if (hm.BoardOf == PlayerMode.Human && hm.diceRandomValue == 6)
            {

                GameClient._Instance.Send(new Message { playerPhase = "open_token", tokenid = id, opened = 1 });
            }
            //send my client data to server
            // openToken();
        }

    }
    public async void openToken()
    {

        switch (hm.homecolor)
        {
            case ColorModes.green:
                movetopos = pp.green[0].position;
                break;
            case ColorModes.blue:
                movetopos = pp.blue[0].position;
                break;
            case ColorModes.yellow:
                movetopos = pp.yellow[0].position;
                break;
            case ColorModes.red:
                movetopos = pp.red[0].position;
                break;

        }
        this.transform.localScale = Vector3.one;
        oldPos = 1;
        hm.diceMinValue = 1;

        OpenSound();

        await Task.Delay(500);
        ReqArrange(1);
    }
    public IEnumerator prepareMove(int pos)
    {

        if (GetNextPostion() > hm.goalPos)
        {
            yield break;
        }
        ReqArrange(0);
        hm.canOpenMyToken = false;
        hm.isMovedToken = true;
        anim.enabled = true;

        sp.sortingOrder = 5;
        this.transform.localScale = Vector3.one;


        for (int i = oldPos; i < pos; i++)
        {


            switch (hm.homecolor)
            {

                case ColorModes.green:
                    if (pos < pp.green.Count + 1)
                    {
                        anim.SetBool("jump", true);
                        JumpSound();
                        // hm.movingtoken = this;
                        // hm.movetopos = pp.green[i].position;
                        movetopos = pp.green[i].position;
                        // Move(pp.green[i].position);

                    }
                    else
                    {

                    }
                    break;

                case ColorModes.blue:
                    if (pos < pp.blue.Count + 1)
                    {
                        anim.SetBool("jump", true);
                        JumpSound();
                        // hm.movingtoken = this;
                        // hm.movetopos = pp.blue[i].position;
                        movetopos = pp.blue[i].position;
                        // Move(pp.blue[i].position);

                    }
                    else
                    {


                    }

                    break;
                case ColorModes.yellow:
                    if (pos < pp.yellow.Count + 1)
                    {
                        anim.SetBool("jump", true);
                        JumpSound();
                        // hm.movingtoken = this;
                        // hm.movetopos = pp.yellow[i].position;
                        movetopos = pp.yellow[i].position;
                        // Move(pp.yellow[i].position);


                    }
                    else
                    {

                    }
                    break;

                case ColorModes.red:
                    if (pos < pp.red.Count + 1)
                    {
                        anim.SetBool("jump", true);
                        JumpSound();
                        // hm.movingtoken = this;
                        // hm.movetopos = pp.red[i].position;
                        movetopos = pp.red[i].position;
                        // Move(pp.red[i].position);

                    }
                    else
                    {


                    }
                    break;

            }

            yield return new WaitForSeconds(.25f);

            //place when token has completed moving
            if (i == pos - 1)
            {

                oldPos = pos;
                itsOnAutoMove = false;
                sp.sortingOrder = 4;
                anim.SetBool("jump", false);

            }
        }

        if (oldPos == hm.goalPos)
        {
            ReqArrange(1);

        }
        else
        {

            CheckForSafePlace();
        }
    }
    void CheckForSafePlace()
    {

        inSafePos = false;
        SafeStar ss = pp.safeStars.Find(s => (Vector3.Distance(s.transform.position, transform.position) < .5f));

        if (ss != null)
        {
            ReqArrange(1);

        }
        else if (ss == null)
        {
            CheckForkill();
        }
        else
        {
            ReqArrange(1);

        }

    }
    async void CheckForkill()
    {
        foreach (homeManager h in gm.homes)
        {
            foreach (token t in h.mytokens)
            {
                if (t.hm != this.hm)
                {
                    Vector3 tilepos = Vector3.zero;
                    switch (hm.homecolor)
                    {
                        case ColorModes.green:
                            tilepos = pp.green[oldPos - 1].position;
                            break;
                        case ColorModes.blue:
                            tilepos = pp.blue[oldPos - 1].position;
                            break;
                        case ColorModes.yellow:
                            tilepos = pp.yellow[oldPos - 1].position;
                            break;
                        case ColorModes.red:
                            tilepos = pp.red[oldPos - 1].position;
                            break;

                    }
                    if (Vector3.Distance(tilepos, t.transform.position) <= .4f)
                    {
                        OtherTokensInMyPos.Add(t);
                    }


                }
            }
        }
        if (OtherTokensInMyPos.Count == 1)
        {

            GameClient._Instance.Send(new Message { id = hm.Id, playerPhase = "hit_token", tokenid = OtherTokensInMyPos[0].id, opponentid = OtherTokensInMyPos[0].hm._Id });
            // if (hm.BoardOf == PlayerMode.Human)
            // {
            //     GameClient._Instance.Send(new Message { playerPhase = "hit_token", tokenid = OtherTokensInMyPos[0].id, opponentid = OtherTokensInMyPos[0].hm._Id });


            // } if (hm.BoardOf == PlayerMode.Dummy)
            // {
            //     GameClient._Instance.Send(new Message { id =hm.Id ,playerPhase = "lazy_hit", tokenid = OtherTokensInMyPos[0].id, opponentid = OtherTokensInMyPos[0].hm._Id });
            // }

            OtherTokensInMyPos[0].GoBackToHome();
            OtherTokensInMyPos.Remove(OtherTokensInMyPos[0]);

            await Task.Delay(200);
            ReqArrange(1);

        }
        else if (OtherTokensInMyPos.Count == 2)
        {
            // if(OtherTokensInMyPos[0].hm!=OtherTokensInMyPos[1].hm){
            //      GameClient._Instance.Send(new Message { id =hm.Id ,playerPhase = "hit_token", tokenid = OtherTokensInMyPos[0].id, opponentid = OtherTokensInMyPos[0].hm._Id });
            // }
            ReqArrange(1);

            OtherTokensInMyPos.RemoveRange(0, 2);

        }
        else
        {
            ReqArrange(1);
        }


    }
    public void ReqArrange(int phase)
    {
        if (oldPos - 1 < 0)
        {
            return;
        }
        Vector3 tile = new Vector3(0, 0, 0);
        switch (hm.homecolor)
        {
            case ColorModes.green:

                tile = pp.green[oldPos - 1].position;
                break;
            case ColorModes.blue:
                tile = pp.blue[oldPos - 1].position;
                break;
            case ColorModes.yellow:
                tile = pp.yellow[oldPos - 1].position;
                break;
            case ColorModes.red:
                tile = pp.red[oldPos - 1].position;
                break;
        }
        List<token> tkns = new List<token>();
        if (phase == 1)
        {

            foreach (homeManager h in gm.homes)
            {
                foreach (token t in h.mytokens)
                {
                    if (t)
                    {
                        if (Vector2.Distance(tile, t.transform.position) <= .4f)
                        {

                            tkns.Add(t);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (homeManager h in gm.homes)
            {
                foreach (token t in h.mytokens)
                {
                    if (t)
                        if (Vector2.Distance(tile, t.transform.position) <= .4f && t != this)
                        {

                            tkns.Add(t);
                        }

                }
            }
        }
        othertokenscount = tkns.Count;
        if (tkns.Count > 0)
        {
            gm.ArrangeTokensOfSameTile(tkns, tile);
        }
    }
    public async void GoBackToHome()
    {

        myscale = Vector3.one;
        availableScale(0, Vector3.one);
        movetopos = myHome;
        // isOpened = false;
        this.transform.localScale = Vector3.one;
        await Task.Delay(500);
        movetopos = myHome;

    }
    public int GetNextPostion()
    {
        int pos = oldPos + hm.diceRandomValue;
        return pos;
    }
    public int GetTileId()
    {
        var tile = 0;
        switch (hm.homecolor)
        {
            case ColorModes.green:

                tile = pp.green[oldPos-1].GetInstanceID();
                break;
            case ColorModes.blue:
                tile = pp.blue[oldPos-1].GetInstanceID();
                break;
            case ColorModes.yellow:
                tile = pp.yellow[oldPos-1].GetInstanceID();
                break;
            case ColorModes.red:
                tile = pp.red[oldPos-1].GetInstanceID();
                break;
        }
        Debug.LogWarning(tile);

        return tile;
    }
    public int GetNextTileId()
    {
        int pos = oldPos + hm.diceRandomValue;
        
        Transform tile= null ;
        switch (hm.homecolor)
        {
            case ColorModes.green:
                tile = pp.green[pos-1];
            break;
            case ColorModes.blue:
                tile = pp.blue[pos-1];
                break;
            case ColorModes.yellow:
                tile = pp.yellow[pos-1];
                break;
            case ColorModes.red:
                tile = pp.red[pos-1];
            break;
        }
        int id = tile.GetInstanceID();
        Debug.LogWarning(tile);
        // StartCoroutine("blink",tile.GetComponent<SpriteRenderer>());
        return id;
    }
    IEnumerator blink(SpriteRenderer s){
        s.color=Color.black;
        yield return new WaitForSeconds(2);
        s.color=Color.white;
    }
    public async void SyncPosition(int to)
    {
        if (to < 1)
        {
            movetopos = myHome;
        }
        else
        {
            switch (hm.homecolor)
            {
                case ColorModes.green:
                    movetopos = pp.green[to - 1].position;

                    break;
                case ColorModes.blue:
                    movetopos = pp.blue[to - 1].position;

                    break;
                case ColorModes.yellow:
                    movetopos = pp.yellow[to - 1].position;

                    break;
                case ColorModes.red:
                    movetopos = pp.red[to - 1].position;
                    break;

            }
        }

        this.oldPos = to;
        await Task.Delay(500);
        ReqArrange(1);

    }
    public void moveToken()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, movetopos, Time.deltaTime + .2f);
    }
    public void JumpSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
            source.PlayOneShot(sfx.jumpclip);
    }
    public void GoalSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
            source.PlayOneShot(sfx.goalclip);
        GameObject g = GameObject.Instantiate(goalvfx, transform.position, Quaternion.identity);
        Destroy(g, 3);
    }
    public void OpenSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
            source.PlayOneShot(sfx.openclip);
    }
    public void KillSound()
    {
        if (PlayerPrefs.GetInt("sound") == 1)
            source.PlayOneShot(sfx.killclip);
    }
}





