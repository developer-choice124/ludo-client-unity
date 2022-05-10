  using System;

  namespace Models{

    
  [Serializable]
    public class Data{
      public string name;
      public string username;
      public int userprofile;
      public string _id;
		  public string status;
		  public string token;
      public string phone;
      public string otp;
      public int usercoins;
      public bool existinguser;
      public string url;
      public string orderID;
      
    }
      [Serializable]
    public class Response{
        public Data data;
        public string error;
        public bool errorvalue;
    }
      [Serializable]
    public class msg{
        public string message;
        public string error;
        public bool errorvalue;
    }
        [Serializable]
    public class playernotfound{
      public string costofroom;
      public int numberofplayers;

    }
          [Serializable]
    public class uploadres{
        public string data;
        public string error;
        public bool errorvalue;
    }
      [Serializable]
    public class RTransaction{
        public int redeemcoins;
        public string paynumber;
        public string phone;
        public string created_on;
        public int is_active;
    }
      [Serializable]
    public class RTransactions{
      public RTransaction[] data;
      public string error;
      public bool errorvalue;
    }
     [Serializable]
    public class Transactions{
      public Transaction[] data;
      public string error;
      public bool errorvalue;
    }
        [Serializable]
    public class Transaction{
        public string _id;
        public PlayersTransactions[] players;
        public string room_coin_id;
        public string coly_room_id;
        public int max_players;
        public string date_created;
        public int coins;
        public string date_modified;
        public string winnerId;
       

    }
        [Serializable]
    public class PlayersTransactions{
      public string varifyid;
      public string colyseus_id;

    }



  }
