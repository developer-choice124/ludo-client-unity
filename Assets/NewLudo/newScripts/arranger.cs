using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arranger : MonoBehaviour
{
    public List<Transform> tkns= new List<Transform>();
    
    public  float areaX=.5f;
    public  int maxHorizontalLimit=5;
    public Transform tile;

    // private float 
    void Start()
    {

    }
    void Update(){

        // if(Input.GetKeyDown(KeyCode.Space)){
            arrange(tkns,tile);
        // }
    }

    void arrange(List<Transform> tkns,Transform tile)
    {   
       
        float areaX = tile.localScale.x/2;
        float fromX=( tile.position.x-areaX);
        float toX=( tile.position.x+areaX);

        float areaY = tile.localScale.y/2;
        float fromY=( tile.position.y-areaY);
        float toY=( tile.position.y+areaY);

        float factor = (1/((float)tkns.Count+1));
        float nextFactorX = 0 ;
        float nextFactorY = 0 ;

        Debug.Log(factor);    


           for (int i = 0; i < tkns.Count; i++)
            {
                nextFactorX += factor;

                float nextX = Mathf.Lerp(fromX,toX,nextFactorX);
                float nextY = Mathf.Lerp(fromX,toX,nextFactorY);

                if(i>=maxHorizontalLimit){
                        nextFactorY += factor;
                       tkns[i].position = new Vector2( nextFactorY,.2f);
         
                 }else{
                    tkns[i].position = new Vector2( nextX , tile.position.y);

                    Debug.Log(nextX);
                 }

            }

        // tkns.ForEach(tkn=>{

        //     nextFactor += factor;

        //     float nextX = Mathf.Lerp(fromX,to,nextFactor);

        //     tkn.position = new Vector2( nextX , tile.position.y);

        //     Debug.Log(nextX);

        // });

    }
    public void reasemble(ref Transform tkn,int order){

       
    }
}
