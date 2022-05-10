// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.39
// 

using Colyseus.Schema;
[System.Serializable]
public class Player : Schema {
	[Type(0, "string")]
	public string sessionId = "";

	[Type(1, "boolean")]
	public bool isBot = false;

	[Type(2, "string")]
	public string varifyId = "";

	[Type(3, "string")]
	public string name = "";

	[Type(4, "int8")]
	public int userprofile = 0;

	[Type(5, "number")]
	public float seat = 0;

	[Type(6, "string")]
	public string playerPhase = "";

	[Type(7, "number")]
	public float opened = 0;

	[Type(8, "number")]
	public float goaled = 0;

	[Type(9, "number")]
	public float lazymoves = 0;

	[Type(10, "number")]
	public float dicevalue = 0;

	[Type(11, "number")]
	public float tokenid = 0;

	[Type(12, "string")]
	public string opponentid = "";

	[Type(13, "map", typeof(MapSchema<TokenState>))]
	public MapSchema<TokenState> mytokens = new MapSchema<TokenState>();

	[Type(14, "boolean")]
	public bool sync = false;

	[Type(15, "boolean")]
	public bool Islazy = false;

	[Type(16, "boolean")]
	public bool isRolledDice = false;

	[Type(17, "boolean")]
	public bool isMovedToken = false;

	[Type(18, "int8")]
	public int tknOldPos = 0;

	[Type(19, "boolean")]
	public bool connected = false;

	[Type(20, "boolean")]
	public bool CanOpenToken = false;
}

