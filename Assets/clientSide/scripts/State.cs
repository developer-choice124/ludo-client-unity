// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.39
// 

using Colyseus.Schema;
[System.Serializable]
public class State : Schema {
	[Type(0, "map", typeof(MapSchema<Player>))]
	public MapSchema<Player> players = new MapSchema<Player>();

	[Type(1, "number")]
	public float turn = 0;

	[Type(2, "string")]
	public string gamePhase = "";

	[Type(3, "number")]
	public float time = 0;

	[Type(4, "number")]
	public float turndelay = 0;

	[Type(5, "string")]
	public string winner = "";

	[Type(6, "string")]
	public string fee = "";

	[Type(7, "string")]
	public string win = "";
}

