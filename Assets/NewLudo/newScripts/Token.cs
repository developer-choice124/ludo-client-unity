using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInterfaces;
using System.Threading.Tasks;
using System.Linq;
public class Token : MonoBehaviour
{
    public ColorModes tokenColor; //to know color of my home class
    public baseManager bm;// to know my home class
    public SpriteRenderer tokenSprite;
    private BoardCreator boardCreator; //board creator class of the game
    public TokenState state; // networked state of this token

    private int currentPostion, oldPosition;
    public Vector3 VecPosition;

    public bool itsOnAutoMove;
    public List<Token> tokensInTile = new List<Token>();

    public Transform GetOnTile() => bm.myNode.mypath[oldPosition];

    void Start()
    {
        if (tokenSprite == null)
        {
            tokenSprite = GetComponent<SpriteRenderer>();
        }
        boardCreator = FindObjectOfType<BoardCreator>();

    }

    //when player click on this token
    public void OnClick()
    {
        //checking if the owner of this token is authorized for taking actions
        if (bm.user == PlayerMode.Human)
        {
            if (1 == bm.state.seat)
            {
                CheckForOpen();
                CheckForMove();
            }
        }
        //checking possible actions
        void CheckForMove()
        {
            if (bm.state.isRolledDice && bm.state.isMovedToken == false)
            {
                if (state.isOpened == true)
                {
                    if (this.GetNextPostion() < game.goalPos + 1 && itsOnAutoMove == false)
                    {
                        TakeAction("move_token");
                    }
                }
            }
        }
        void CheckForOpen()
        {
            if (bm.canOpenMyToken == true && !bm.state.Islazy)
            {
                if (state.isOpened == false && state.isGoaled == false)
                {
                    if (bm.state.dicevalue == 6 || bm.state.dicevalue == 1)
                    {

                        TakeAction("open_token");
                    }
                }
            }
        }

    }
    //checking actions and creating msg according to them
    private void TakeAction(string input)
    {
        Message msg = null;
        switch (input)
        {
            case "move_token":
                msg = new Message
                {

                    playerPhase = input,
                    tokenid = state._id,
                    tokenpos = this.GetNextPostion(),
                    isMovedToken = true,
                    id = bm.state.sessionId
                };
                break;
            case "open_token":
                msg = new Message
                {
                    playerPhase = input,
                    tokenid = state._id,
                    opened = 1,
                    id = bm.state.sessionId
                };
                break;
            case "hit_token":
                msg = new Message
                {
                    id = bm.state.sessionId,
                    playerPhase = input,
                    tokenid = tokensInTile[0].state._id,
                    opponentid = tokensInTile[0].bm.state.sessionId
                };
            break;
        }
        //sending msg to msg handler if its not null
        if (msg != null)
        {
            game.PassMessage(msg);
        }
    }
    // set theme by base manager
    public void SetTheme()
    {
        ThemeConfig theme = boardCreator.bases[tokenColor];
        tokenSprite.sprite = theme.tokenSprite;
    }
    public int GetNextPostion()
    {
        return (oldPosition + currentPostion);
    }

    public IEnumerator MoveToken(int to)
    {
        currentPostion = to;

        for (int i = oldPosition; i < currentPostion; i++)
        {
            if (currentPostion < bm.myNode.mypath.Count + 1)
            {
                VecPosition = bm.myNode.mypath[i].position;
            }

            yield return new WaitForSeconds(.25f);

            //place when token has completed moving
            if (i == currentPostion - 1)
            {
                oldPosition = currentPostion;
            }
        }

        if (oldPosition == 57)
        {
            // boardCreator.ArrangeTokens()
        }
        else
        {
            CheckOnStop();
        }
    }

    public void UpdatePostion()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, VecPosition, Time.deltaTime + .4f);
    }

    private void CheckOnStop()
    {
        Transform onTile = bm.myNode.mypath[oldPosition];

        // list all the overlaping tokens
        tokensInTile = Physics2D.OverlapBoxAll(onTile.position, new Vector2(.5f, .5f), 90, boardCreator.tokensLayer.value).Cast<Token>().ToList();

          // abort if im on the safe tile
        if(boardCreator.safeTiles.Contains(onTile)){
            boardCreator.ArrangeTokens(this);
            return;
        }

        //remove myself form list
        if(tokensInTile.Contains(this))
        {
            tokensInTile.Remove(this);
        }
      

        if(tokensInTile.Count==1){
            TakeAction("hit_token");
        }
        for (int i = 0; i < 4; i++)
        {
            Vector2 bound1 = new Vector2(onTile.position.x + .5f, onTile.position.y + .5f);
            Vector2 bound2 = new Vector2(onTile.position.x - .5f, onTile.position.y - .5f);

            Debug.DrawLine(bound1, bound2, Color.black, 10f);
        }

    }
}





