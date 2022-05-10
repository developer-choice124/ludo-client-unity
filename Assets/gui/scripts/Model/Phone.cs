using System;


namespace Models
{
    [Serializable]
    public class Phone
    {
        public string phone;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
   
}

