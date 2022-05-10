// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.39
// 

using Colyseus.Schema;

[System.Serializable]
public class TokenState : Schema {
	[Type(0, "int8")]
	public int _id = 0;

	[Type(1, "int8")]
	public int pos = 0;

	[Type(2, "boolean")]
	public bool isOpened = false;

	[Type(3, "boolean")]
	public bool isGoaled = false;

	[Type(4, "int16")]
	public short tokensaround = 0;
}

