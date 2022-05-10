using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public Sprite[] dicevalues;
    public Animator anim;
    public SpriteRenderer finalSprite;

    void Start()
    {
       
    }
    public void roll()
    {
        if(anim)
        anim.SetBool("roll",true);
    }
    public void stop(int value)
    {   
        if(anim)
        anim.SetBool("roll",false);
        finalSprite.sprite =dicevalues[value];
    }

}
