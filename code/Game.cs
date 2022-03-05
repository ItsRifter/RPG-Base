
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


public partial class Game : Sandbox.Game
{
	private RPGHud oldHud;

	public Game()
	{
		if(IsServer)
		{

		}
		if (IsClient)
		{
			oldHud = new RPGHud();
		}
	}

	[Event.Hotload]
	public void UpdateHUD()
	{
		oldHud?.Delete();

		if ( IsClient )
			oldHud = new RPGHud();
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var player = new PlayerBase();
		player.Spawn();

		client.Pawn = player;

		LoadSave( player );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		if ( cl.Pawn is PlayerBase player )
			CommitSave( player );

		base.ClientDisconnect( cl, reason );
	}
}
