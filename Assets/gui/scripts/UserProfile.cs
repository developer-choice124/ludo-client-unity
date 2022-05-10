using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfile : MonoBehaviour
{
    Dropdown profileSelector;
    public utils profilesValues;
    
    void Start()
    {

        profileSelector=GetComponent<Dropdown>();
        
         foreach (Sprite sp in profilesValues.sprites)
        {
            profileSelector.options.Add(new Dropdown.OptionData(sp));
        }
         
        
    }

}
