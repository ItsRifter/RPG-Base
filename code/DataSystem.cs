using System;
using Sandbox;

public partial class Game 
{
	[Event( "rpg_evnt_save" )]
	public void CommitSave( PlayerBase player )
	{
		Log.Info( "Commiting save for " + player.Client.Name );
		int[] array = new int[4];
		array = player.GetStats();

		FileSystem.Data.WriteJson( player.Client.Name.ToLower() + ".json", array );
		Log.Info( player.Client.Name + "'s data has been saved" );
	}

	[Event( "rpg_evnt_load" )]
	public void LoadSave( PlayerBase player )
	{
		var loadData = FileSystem.Data.ReadJson<int[]>( player.Client.Name.ToLower() + ".json" );

		if ( loadData == null )
			return;

		player.SetStats( loadData[0], loadData[1], loadData[2], loadData[3] );
	}
}

