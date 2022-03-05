using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class ShopMenu : Panel
{
	public bool IsOpen = false;
	private TimeSince lastOpened;
	private Panel navPnl;

	public ShopMenu()
	{
		StyleSheet.Load( "UI/ShopMenu.scss" );

		Panel menuPanel = Add.Panel("menu");

		navPnl = menuPanel.Add.Panel( "navigation" );

		Panel shopArea = menuPanel.Add.Panel( "mainarea" );

	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as PlayerBase;

		if ( player == null )
			return;


		if(Input.Pressed(InputButton.Use))
		{
			if (player.FindNPC().IsValid())
            {
				if( player.FindNPC().GetType().FullName.Contains( "GeneralShop" ) && lastOpened > 0.1f && !IsOpen )
				{
					IsOpen = true;
					lastOpened = 0.0f;
				} else if ( lastOpened > 0.1f && IsOpen )
				{
					IsOpen = false;
					lastOpened = 0.0f;
				}

            } 
			else
			{
				IsOpen = false;
				lastOpened = 0.0f;
			}
		}
		SetClass( "active", IsOpen );
	}
}
