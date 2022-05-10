using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class NativeInternet 
{
    
    public static bool IsNetworkAvailableStatic
    {
        get
        {
            #if UNITY_EDITOR || !UNITY_ANDROID
            return true;
            #else
            AndroidJavaClass connectionClass = new AndroidJavaClass("com.pocket_ludo.network_checker.NetworkUtil");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            return connectionClass.CallStatic<bool>("isNetworkAvailableStatic", currentActivity);
            #endif
        }
        
    }
    public static bool IsNetworkAvailable
    {
        get
        {
            #if UNITY_EDITOR || !UNITY_ANDROID
            return true;
            #else
            AndroidJavaClass connectionClass = new AndroidJavaClass("com.pocket_ludo.network_checker.NetworkUtil");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            return connectionClass.Call<bool>("isNetworkAvailable", currentActivity);
            #endif
        }
    }
    public static void ShowToast(string msg)
    {
             #if UNITY_EDITOR || !UNITY_ANDROID
                Debug.Log(" toast = "+ msg);
            #else

            AndroidJavaClass connectionClass = new AndroidJavaClass("com.pocket_ludo.network_checker.NetworkUtil");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");


            connectionClass.CallStatic("ShowToast", currentActivity,msg);
            #endif
    }
}
