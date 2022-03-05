using Sandbox;
using System;
using System.Linq;

public partial class PlayerBase : Player
{


	private bool canMove = true;

	public PlayerBase()
	{
		Inventory = new Inventory(this);
	}

	private void SpawnAtPoint()
	{
		var spawnpoints = All.OfType<SpawnPoint>();

		var randomSpawn = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( randomSpawn != null )
		{
			Position = randomSpawn.Position;
			Rotation = randomSpawn.Rotation;
		}
	}

	public override void Spawn()
	{
		base.Spawn();

		NewStats();

		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		CameraMode = new RPGCamera();

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableAllCollisions = true;

		canMove = true;

		SpawnAtPoint();
		Health = 100;

		if(Inventory != null)
		{
			Inventory.Add(new MeleeBase(), true);
		}

	}

	public override void Respawn()
	{
		base.Respawn();

		SetModel( "models/citizen/citizen.vmdl" );
		Health = 100;

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		EnableAllCollisions = true;

		canMove = true;

		if ( Inventory != null )
		{
			Inventory.DeleteContents();
			Inventory.Add( new Crowbar(), true );
		}
	}

	public override void Simulate( Client cl )
	{

		SimulateActiveChild( cl, ActiveChild );

		if ( !canMove )
			return;

		TickPlayerUse();

		EyeRotation = Rotation;

		base.Simulate( cl );
	}

	protected override void TickPlayerUse()
	{
		base.TickPlayerUse();
		
		//Finds a interactable NPC
		using(Prediction.Off())
		{
			if(Input.Pressed(InputButton.Use))
			{
				BaseNPC npc = FindNPC();
				ItemBase item = FindItem();

				if ( npc != null)
					npc.OnNPCUse( this );
				else if ( item != null )
					item.OnUse( this );
			}
		}
	}

	public BaseNPC FindNPC()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			.Ignore( this )
			.Run();

		var ent = tr.Entity;

		if ( ent is BaseNPC npc )
			return npc;

		return null;
	}

	public ItemBase FindItem()
	{
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 85 )
			.Ignore( this )
			.Run();

		var ent = tr.Entity;

		if ( ent is ItemBase item )
			return item;

		return null;
	}

	public override void FrameSimulate( Client cl )
	{
		if ( !canMove )
			return;

		EyeRotation = Rotation;

		base.FrameSimulate( cl );
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( info.Attacker is PlayerBase playerAttacker && playerAttacker != this )
			return;

		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		BecomeRagdollOnClient();

		EnableDrawing = false;
	}
}
