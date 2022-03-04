using System;
using System.Collections.Generic;

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class InventoryMenu : Panel
{
	public bool IsOpen = false;
	private TimeSince lastOpened;
	private List<(Panel, Panel)> pages = new();
	private int activePnl = -1;
	private Panel navPnl;

	public InventoryMenu()
	{
		StyleSheet.Load( "UI/InventoryMenu.scss" );

		Panel menuPanel = Add.Panel("menu");

		navPnl = menuPanel.Add.Panel( "navigation" );

		Panel mainArea = menuPanel.Add.Panel( "mainarea" );

		Panel invPage = mainArea.Add.Panel( "page" );
		invPage.Add.Panel( "player" );

		Panel skillsPage = mainArea.Add.Panel( "page" );

		Panel playersPage = mainArea.Add.Panel( "page" );

		AddPage( invPage, "Inventory", Color.White );
		AddPage( skillsPage, "Skills", Color.Red );
		AddPage( playersPage, "Players", new Color( 0, 1, 1 ) );
	}

	private void AddPage(Panel pnl, string name, Color btnColour)
	{
		int pageKey = pages.Count;

		Panel button = navPnl.Add.Label( name, "navBtn" );

		button.Style.BorderBottomColor = btnColour;

		button.AddEventListener( "onclick", () =>
		{
			SetActivePage( pageKey );
		});

		pages.Add( (pnl, button) );

		if(pages.Count <= 1)
		{
			SetActivePage( pageKey );
		}
	}

	private void SetActivePage(int pageKey)
	{
		if( activePnl >= 0)
		{
			(Panel, Panel)activeInfo = pages[activePnl];
			activeInfo.Item1.SetClass( "active", false );
			activeInfo.Item2.SetClass( "active", false );
		}

		activePnl = pageKey;

		(Panel, Panel) pageInfo = pages[pageKey];
		pageInfo.Item1.SetClass( "active", true );
		pageInfo.Item2.SetClass( "active", true );
	}

	public override void Tick()
	{
		base.Tick();

		if(Input.Pressed(InputButton.Menu) && lastOpened > 0.1f)
		{
			IsOpen = !IsOpen;
			lastOpened = 0.0f;
		}

		SetClass( "active", IsOpen );
	}
}
