using System;
using Sandbox;

[Library( "info_rpg_npc_spawnpoint" )]
[Hammer.EditorModel( "models/citizen/citizen.vmdl" )]
[Hammer.EntityTool( "NPC Spawnpoint", "RPG", "Defines a point where a specific NPC can spawn" )]
public class NPCSpawnpoint : Entity
{
	[Property( "NPC_Type" ), Title("Type Of NPC"), Description( "What type of NPC should spawn here" )]
	public string NPCType { get; set; }

	[Property( "NPC_Respawn_Rate" ), Title( "Respawn Rate" ), Description( "How quick after death should this NPC respawn" )]
	public int NPCRespawnRate { get; set; }

	private BaseNPC NPC;
	private TimeSince timeKilled;

	public override void Spawn()
	{
		base.Spawn();
		
		if ( NPCType == "Zombie" )
		{
			NPC = new Zombie();
			NPC.Position = Position;
			NPC.Rotation = Rotation;
		}
	}

	[Event.Tick.Server]
	public void SpawnpointTick()
	{
		if ( !IsNPCAlive() )
			Spawn();
	}

	public bool IsNPCAlive()
	{
		if ( NPC == null || NPC.Health <= 0 )
			return false;

		return true;
	}
}
