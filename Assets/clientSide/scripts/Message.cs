// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.39
// 

using Colyseus.Schema;

[System.Serializable]
public class Message : Schema {
	[Type(0, "string")]
	public string playerPhase = "";

	[Type(1, "number")]
	public float opened = 0;

	[Type(2, "number")]
	public float goaled = 0;

	[Type(3, "number")]
	public float tokenid = 0;

	[Type(4, "number")]
	public float dicevalue = 0;

	[Type(5, "string")]
	public string opponentid = "";

	[Type(6, "int8")]
	public int tokenpos = 0;

	[Type(7, "boolean")]
	public bool isMovedToken = false;

	[Type(8, "boolean")]
	public bool connected = false;

	[Type(9, "string")]
	public string id = "";

	[Type(10, "int16")]
	public short tokensaround = 0;
}

