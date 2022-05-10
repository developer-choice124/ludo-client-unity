using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]

public class board
{
    public string name; 
    public int players = 2;
    public baseManager[] bases;
}
[System.Serializable]
public class Basetheme
{
    public Sprite baseSprite, tileSprite, goalSprite, tokenSprite, diceSprite;

}
[System.Serializable]
public class Base
{

    public ColorModes color;
    public Basetheme theme;
}
public class BoardCreator : MonoBehaviour
{
    public ColorModes selectedColor;
    public int selectedPlayers = 2;
    [Header("All of the bases In Loop-Order")]
    public baseManager[] basesLoop;
    [Header("set Token Prefab")]
    public GameObject baseTokenPrefab;
    [Header("set up base theme")]
    public ThemeConfig[] setBases;
    [Header("set up board format")]
    public board[] setBoards;
    [HideInInspector()]
    public List<PathNode> nodePathsLoop = new List<PathNode>();
    public List<Transform> safeTiles = new List<Transform>();
    public Dictionary<int, baseManager[]> boards = new Dictionary<int, baseManager[]>();
    public Dictionary<ColorModes, ThemeConfig> bases = new Dictionary<ColorModes, ThemeConfig>();

    public bool isCreated = false,debugPath=true;
    public LayerMask tokensLayer;

    public Color debugColorEnter = Color.red;

    private GameObject ctkn;
    private Vector3 pos;
    void Awake()
    {
        //add board for different players according to setboards
        foreach (var b in setBoards)
        {
            if (!boards.ContainsKey(b.players))
            {
                boards.Add(b.players, b.bases);
            }
        }
        //add  base theme for different colors according to setbases
        foreach (var b in setBases)
        {
            if (!bases.ContainsKey(b.color))
            {
                bases.Add(b.color, b);
            }
        }
        foreach (var node in nodePathsLoop)
        {
            safeTiles.AddRange(node.safes);
        }
        foreach (var item in basesLoop)
        {

            var tPath = item.gameObject.GetComponent<PathNode>();

            nodePathsLoop.Add(tPath);
            tPath.Init();
            
        }
    }
    void Start()
    {
        //initialize the board for first time according to initial players value
        Create();
       ctkn= Instantiate(baseTokenPrefab,Vector3.zero,Quaternion.identity);
       StartCoroutine("moveTkn",1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Create();
        }
        if (isCreated)
        {
            foreach (var b in basesLoop)
            {
                if (b.enabled == true)
                {
                    b.UpdateBase();
                }
            }
        }

        if (debugPath)
        {
            for (var p = 0; p < nodePathsLoop.Count; p++)
            {
                for (var i = 1; i < nodePathsLoop[p].mypath.Count; i++)
                {

                    Debug.DrawLine(nodePathsLoop[p].mypath[i-1].position, nodePathsLoop[p].mypath[i].position, debugColorEnter);
                }
            }

        }

        // ctkn.transform.position =   Vector3.MoveTowards(ctkn.transform.position,pos,.5f);

    }
    public IEnumerator moveTkn(float seconds){
        Debug.Log("movetkn");

         for (var p = 0; p < nodePathsLoop.Count; p++)
            {
                for (var i = 1; i < nodePathsLoop[p].mypath.Count; i++)
                {
                    // pos = nodePathsLoop[p].mypath[i].position;
                     Debug.DrawLine(nodePathsLoop[p].mypath[i-1].position, nodePathsLoop[p].mypath[i].position, debugColorEnter,100);
                    yield return new WaitForSeconds(seconds);
                }
            }
    }
    public void Create()
    {
        SetBoardLayout();
        SetBaseThemes();
    }
    private void SetBoardLayout()
    {
        //return if players selected isn't in the boards dictionary
        if (!boards.ContainsKey(selectedPlayers)) return;

        //activating and deactivating board for players selected
        foreach (var h in basesLoop)
        {
            h.enabled = false;
        }
        for (int i = 0; i < boards[selectedPlayers].Length; i++)
        {
            boards[selectedPlayers][i].enabled = true;
            boards[selectedPlayers][i].name = (i + 1).ToString() + " player";
        }
    }
    private void SetBaseThemes()
    {

        var colors = ColorModes.GetValues(typeof(ColorModes));

        List<baseManager> basetocolor = basesLoop.ToList().FindAll(bb => (bb.isActiveAndEnabled));

        int i = 0;
        //set colors of bases for theme
        foreach (var color in colors)
        {
            i++;
            foreach (var bb in basetocolor)
            {
                if ((ColorModes)color == selectedColor)
                {

                    if (bb.user == PlayerMode.Human)
                    {
                        bb.baseColor = (ColorModes)color;
                        basetocolor.Remove(bb);
                        break;
                    }
                    else
                    {
                        break;
                    }

                }
                else if (bb.user == PlayerMode.Dummy)
                {

                    bb.baseColor = (ColorModes)color;
                    basetocolor.Remove(bb);
                    break;
                }
            }
            if (i >= selectedPlayers)
            {
                break;
            }

        }
        //set the theme of bases
        foreach (var b in basesLoop)
        {
            // try
            // {

                b.SetTheme();
                
            // }
            // catch (System.Exception ex)
            // {

            //     Debug.Log(b.gameObject.name);
            //     throw ex;

            // }
        }
    }
    public void ArrangeTokens(Token by){

        List<Token> tokensInTile = Physics2D.OverlapBoxAll(by.GetOnTile().position, new Vector2(.5f, .5f), 90,tokensLayer.value).Cast<Token>().ToList();
        if(tokensInTile.Contains(by))
        {
            tokensInTile.Remove(by);
            tokensInTile.Insert(0,by);
        }

        Vector2 from = new Vector2(by.GetOnTile().position.x-.5f,by.GetOnTile().position.y-.5f) ;
        Vector2 to = new Vector2(by.GetOnTile().position.x+.5f,by.GetOnTile().position.y+.5f) ;
        Vector2 area= from - to;

            // for (int i = 0; i < tokensInTile.Count-1; i++)
            // {   
            //      tokensInTile[i].transform.position
            // }
          

    }
}