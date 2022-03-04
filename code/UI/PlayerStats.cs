using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class PlayerStats : Panel
{
	public Label levelLbl;
	public Label xpLbl;
	public Label moneyLbl;

	public PlayerStats()
	{
		StyleSheet.Load( "UI/PlayerStats.scss" );

		levelLbl = Add.Label( "Level");
		xpLbl = Add.Label( "XP" );
		moneyLbl = Add.Label( "Silvers" );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PlayerBase;

		if ( player == null )
			return;

		levelLbl.SetText( "Level: " + player.Level );
		xpLbl.SetText( " XP: " + player.CurrentXP + "/" + player.ReqXP );
		moneyLbl.SetText( " Gold: " + player.Money );
	}
}
