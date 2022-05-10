using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
public class baseManager : MonoBehaviour
{
    public ColorModes baseColor;
    public SpriteRenderer baseSprite,goalTile;
    public SpriteRenderer[] tiles;
    public List<Transform> rooms;
    public List<Token> myTokens;
    public PathNode myNode;
    [Header("User Type")]
    public PlayerMode user = PlayerMode.Dummy;
    [Header("State Of this base")]
    public Player state;
    BoardCreator BoardCreator;
    public bool canOpenMyToken;
    private List<Transform> totalPath=new List<Transform>();


    
    void Awake()
    {
        // myTokens.Add( Instantiate(BoardCreator.baseTokenPrefab,) )
        BoardCreator = FindObjectOfType<BoardCreator>();
        baseSprite = GetComponent<SpriteRenderer>();
        myNode = GetComponent<PathNode>();
    }
    public void SetTheme()
    {

        if(!BoardCreator.bases.ContainsKey(baseColor)){
            return;
        }

        //getting theme and applaying to this base
        ThemeConfig theme= BoardCreator.bases[baseColor];
        baseSprite.sprite= theme.baseSprite;
        goalTile.sprite= theme.baseSprite?theme.baseSprite:null;

        foreach (var tile in tiles)
        {    
           tile.sprite=theme.tileSprite;
        }
        foreach (var tkn in myTokens)
        {    
           tkn.SetTheme();
        }
    }
    public void SpawnTokens(){

        for (int i = 0; i < rooms.Count; i++)
        {
            Token tkn = Instantiate(BoardCreator.baseTokenPrefab, rooms[i].position, Quaternion.identity).GetComponent<Token>();
            tkn.bm = this;
            tkn.tokenColor = this.baseColor;
            myTokens.Add(tkn);
            tkn.state._id = i;
        }

    }
    public void UpdateBase(){
        MoveTokens();
    }
    private void MoveTokens(){

        foreach (var tkn in myTokens)
        {
            tkn.UpdatePostion();
        } 

    }
}
