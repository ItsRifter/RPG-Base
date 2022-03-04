
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


public partial class MyGame : Sandbox.Game
{
	private RPGHud oldHud;

	public MyGame()
	{
		if(IsClient)
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
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );
	}
}
