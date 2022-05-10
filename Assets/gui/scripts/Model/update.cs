using System;


namespace Models
{
    [Serializable]
    public class updatedata
    {
    public bool type;
	public string version;
	public string version_type;
	public string email;
	public string link;
	public string whatsnew;
    public bool errorvalue;
    public string error;
    public string date;
    }
       [Serializable]
     public class update
    {
        public updatedata data;
        public bool errorvalue;
        public string error;
    }
}
