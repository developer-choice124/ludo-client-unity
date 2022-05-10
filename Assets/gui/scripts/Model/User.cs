using System;

namespace Models
{
	[Serializable]
	public class UserDetail
	{
		public string name;
		public string username;
		public int userprofile;
		
		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}

	[Serializable]
	public class Bank {
		public long accountnum;
		public string accountifsc;
		public string accountname;
		public string accountbranchname;

	}
	[Serializable]
	public class redeemRequest
	{
		public long paynumber;
		public long redeemcoins;
		public string paytype;
		public string payupiid;
		public Bank account;

	}
}

