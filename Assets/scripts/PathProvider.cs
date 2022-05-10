using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathProvider : MonoBehaviour
{
    public GameObject paths;
    public List<PathInfo> path;
    public List<Transform> green, red, yellow, blue;
    public List<SafeStar> safeStars;
    void Start()
    {
        safeStars = paths.GetComponentsInChildren<SafeStar>().ToList();
        path = paths.GetComponentsInChildren<PathInfo>().ToList();
        for (int i = 0; i < 58; i++)
        {
            for(int l = 0; l < path.Count; l++)
            {
                if (path[l].green == i)
                {
                    green.Add(path[l].transform);
                }
                if (path[l].red == i)
                {
                    red.Add(path[l].transform);
                }
                if (path[l].yellow == i)
                {
                    yellow.Add(path[l].transform);
                }
                if (path[l].blue == i)
                {
                    blue.Add(path[l].transform);
                }
            }
        }
    }
    void Update()
    {
    }
}
