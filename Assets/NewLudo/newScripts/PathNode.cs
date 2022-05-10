using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathNode : MonoBehaviour
{
    [Header("enter Path")]
    public List<Transform> enter;
    [Header("Exit Path")]
    public List<Transform> exit;
    [Header("round Path")]
    public List<Transform> round;

    [Header("safe pos")]
    public List<Transform> safes;
    public List<Transform> mypath = new List<Transform>();
    private BoardCreator boardCreator;
    public Color debugColorEnter = Color.red;
    public Color debugColorExit = Color.red;
    public Color debugColorRound = Color.red;
    public bool canDebug, realtime,induvidualPaths;
    public void Init()
    {
        //getting refrence of board creator
        boardCreator = FindObjectOfType<BoardCreator>();
        int myIndex = boardCreator.nodePathsLoop.IndexOf(this);

        List<Transform> beforepaths = new List<Transform>();
        List<Transform> afterpaths = new List<Transform>();

        Debug.Log("init");
        
        //getting all the path positions after and before to make full path after combine with my path
        foreach (var node in boardCreator.nodePathsLoop)
        {
            if (boardCreator.nodePathsLoop.IndexOf(node) < myIndex)
            {
                node.round.ForEach((pp) =>
                {
                    beforepaths.Add(pp);
                });
            }
            else if (boardCreator.nodePathsLoop.IndexOf(node) > myIndex)
            {
                node.round.ForEach((pp) =>
                {
                    afterpaths.Add(pp);

                });
            }
        }
        
        //combining all the paths (transforms) to make total path for tokens of my base
        mypath.AddRange(exit);
        mypath.AddRange(afterpaths);
        mypath.AddRange(beforepaths);
        mypath.AddRange(enter);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheckPathFlow();
            Debug.Log("Checking path...");
        }
        if (realtime)
        {
            if(!induvidualPaths)return;

            if (canDebug)
            {
                for (var i = 1; i < round.Count; i++)
                {
                    Debug.DrawLine(round[i - 1].position, round[i].position, debugColorRound,1,true);
                }
            }
            if (canDebug)
            {
                for (var i = 1; i < enter.Count; i++)
                {
                    Debug.DrawLine(enter[i - 1].position, enter[i].position, debugColorEnter,1,true);
                }
            }
            if (canDebug)
            {
                for (var i = 1; i < exit.Count; i++)
                {
                    Debug.DrawLine(exit[i - 1].position, exit[i].position, debugColorExit,1,true);
                }
            }
        }
    }
    void CheckPathFlow()
    {
        if (canDebug)
            for (var i = 1; i < mypath.Count; i++)
            {
                Debug.DrawLine(mypath[i - 1].position, mypath[i].position, debugColorRound, 3f,true);
            }
    }
}
