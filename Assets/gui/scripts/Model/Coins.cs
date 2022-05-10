  using System;

      namespace Models{
  [Serializable]
    public class CoinObject{
       public string _id;
       public int coins;
       public playersRoomData players;
       public int price;
       public int is_active;
       public bool errorvalue;
        public string error;
       
    }
    [Serializable]
    public class playersRoomData{
      public playerObject twoplayers;
      public playerObject threeplayers;
      public playerObject fourplayers;
    }
 [Serializable]
    public class playerObject{
      public int winningcoins;
      public int totalcoins;

    }
    [Serializable]
    public class Coins{
     public CoinObject[] data;
      public string error;
     public bool errorvalue;

    }
    [Serializable]
    public class BuyCoins{
     public CoinObject[] data;

     public string _id;

    }
    
  }