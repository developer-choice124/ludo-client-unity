using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

[CreateAssetMenu(fileName="profile", menuName="utils/profiles",order=1)]
public class utils :ScriptableObject
{
    public List<Sprite> sprites;
}
public static class MainUrls{

    // public const string BASE_URL="http://192.168.1.12:8000/api/v1";
    // public const string BASE_URL="http://tsngaming.online:8000/api/v1";
    // public const string SOCKET_URL="ws://localhost:8000";             
    // public const string SOCKET_URL="ws://tsngaming.online:8000";

    // public const string BASE_URL="http://192.168.1.3:8000/api/v1";
    // public const string SOCKET_URL="ws://192.168.1.3:8000";
    // public const string BASE_URL="http://192.168.1.12:8000/api/v1";
    // public const string SOCKET_URL="ws://192.168.1.12:8000";


    public const string BASE_URL="http://18.118.11.230:8000/api/v1/";
    public const string SOCKET_URL="ws://18.118.11.230:8000";

    // public const string BASE_URL="http://167.71.233.35:8000/api/v1";
    // public const string SOCKET_URL="ws://167.71.233.35:8000";
	public const float TIME_LIMIT=30;
	public const float FOUND_TIME_LIMIT=30;
    public const int AUTO_EXIT_MENU=10000;
    public const string APP_SECRET = "LudoTechphantAppSecret";
}
