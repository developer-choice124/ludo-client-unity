using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetQuality : MonoBehaviour
{
    public Dropdown qualityoption;
    void Start(){
        if(qualityoption==null)
        qualityoption= GetComponent<Dropdown>();

         qualityoption.value=PlayerPrefs.GetInt("quality");
    }
    public void Set()
    {
        PlayerPrefs.SetInt("quality",qualityoption.value);
        settings._Instance.SetQuality();
    }
}
