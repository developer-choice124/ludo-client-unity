using System;

namespace Models
{
    [Serializable]
    public class botdetails
    {
        public string phone;
        public string username;
        public string userprofile;
        public long usercoins;
        public string _id;
        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
    [Serializable]
    public class botrequest
    {
        public string phone;
        public string coins;
    }

}

