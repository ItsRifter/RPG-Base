using System;
using System.Linq;
using System.Collections.Generic;
using Sandbox;

public partial class BaseNPC : AnimEntity
{
	public virtual string NPCName => "Default NPC";
	public virtual int BaseHealth => 1;
	public virtual float BaseSpeed => 10;
	public virtual string BaseModel => "models/citizen/citizen.vmdl";
	public virtual bool IsFriendly => false;
	public virtual int minRndLevel => 1;
	public virtual int maxRndLevel => 2;

	public List<(ItemBase itemType, int amount, int chance)> ItemToSpawn { get; set; }

	private int NPCLevel = 0;
	public virtual int minXP => 1;
	public virtual int maxXP => 2;

	private int xpReward = 0;

	public enum ShopType
	{
		General,
		Weapons,
		Armor,
		Misc
	}

	public virtual ShopType Shoptype => ShopType.General;

	[ConVar.Replicated]
	public static bool nav_drawpath { get; set; }

	[ServerCmd( "rpg_npc_clear" )]
	public static void ClearAllNPCs()
	{
		foreach ( var npc in All.OfType<BaseNPC>().ToArray() )
			npc.Delete();
	}

	NPCNavigation Path = new NPCNavigation();
	public NPCSteering Steer;
	public NPCSteerWander Wander;

	public override void Spawn()
	{
		ItemToSpawn = new List<(ItemBase itemType, int amount, int chance)>();

		Tags.Add( "player", "pawn" );

		SetModel( BaseModel );

		NPCLevel = Rand.Int( minRndLevel, maxRndLevel );
		xpReward = Rand.Int (minXP, maxXP );

		Health = BaseHealth * NPCLevel;

		EyePosition = Position + Vector3.Up * 64;
		CollisionGroup = CollisionGroup.Player;
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, Capsule.FromHeightAndRadius( 72, 8 ) );

		EnableHitboxes = true;

		SetBodyGroup( 1, 0 );
	}

	public NPCDebugDraw Draw => NPCDebugDraw.Once;

	Vector3 InputVelocity;

	Vector3 LookDir;

	[Event.Tick.Server]
	public void Tick()
	{
		InputVelocity = 0;

		if ( Steer != null )
		{
			Steer.Tick( Position );

			if ( !Steer.Output.Finished )
			{
				InputVelocity = Steer.Output.Direction.Normal;
				Velocity = Velocity.AddClamped( InputVelocity * Time.Delta * 500, BaseSpeed );
			}

			if ( nav_drawpath )
			{
				Steer.DebugDrawPath();
			}
		}

		Move( Time.Delta );

		var walkVelocity = Velocity.WithZ( 0 );
		if ( walkVelocity.Length > 0.5f )
		{
			var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100, true );
			var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
			Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
		}

		var animHelper = new CitizenAnimationHelper( this );

		LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
		animHelper.WithLookAt( EyePosition + LookDir );
		animHelper.WithVelocity( Velocity );
		animHelper.WithWishVelocity( InputVelocity );
	}

	protected virtual void Move( float timeDelta )
	{
		var bbox = BBox.FromHeightAndRadius( 64, 4 );

		MoveHelper move = new( Position, Velocity );
		move.MaxStandableAngle = 50;
		move.Trace = move.Trace.Ignore( this ).Size( bbox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			move.TryUnstuck();
			move.TryMoveWithStep( timeDelta, 30 );
		}

		var tr = move.TraceDirection( Vector3.Down * 10.0f );

		if ( move.IsFloor( tr ) )
		{
			GroundEntity = tr.Entity;

			if ( !tr.StartedSolid )
			{
				move.Position = tr.EndPosition;
			}

			if ( InputVelocity.Length > 0 )
			{
				var movement = move.Velocity.Dot( InputVelocity.Normal );
				move.Velocity = move.Velocity - movement * InputVelocity.Normal;
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				move.Velocity += movement * InputVelocity.Normal;

				NPCDebugDraw.Once.Line( tr.StartPosition, tr.EndPosition );

			}
			else
			{
				move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
			}

			
		}
		else
		{
			GroundEntity = null;
			move.Velocity += Vector3.Down * 900 * timeDelta;
			NPCDebugDraw.Once.WithColor( Color.Red ).Circle( Position, Vector3.Up, 10.0f );
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		Rotation = Input.Rotation;
		EyeRotation = Rotation;

		var maxSpeed = 500;

		Velocity += Input.Rotation * new Vector3( Input.Forward, Input.Left, Input.Up ) * maxSpeed * 5 * Time.Delta;
		if ( Velocity.Length > maxSpeed ) Velocity = Velocity.Normal * maxSpeed;

		Velocity = Velocity.Approach( 0, Time.Delta * maxSpeed * 3 );

		Position += Velocity * Time.Delta;

		EyePosition = Position;
	}

	public override void TakeDamage( DamageInfo info )
	{
		if ( IsFriendly )
			return;

		Health -= info.Damage;

		if ( Health <= 0 && info.Attacker is PlayerBase player )
		{
			player.AddXP( xpReward );
			OnKilled();
		}
	
	}

	public override void OnKilled()
	{

		foreach ( var item in ItemToSpawn )
		{
			int randomChance = Rand.Int( 1, 100 );

			if ( item.chance >= randomChance )
			{

				Log.Info( item.itemType.GetType() );

				var newItem = new Money();
				newItem.ItemAmount = item.amount;
				newItem.Spawn();
				newItem.Position = Position;
			} 
		}

		base.OnKilled();
	}

	public virtual void OnNPCUse(PlayerBase player)
	{
		Log.Info( player.Client.Name + " Interacted " + NPCName );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		Rotation = Input.Rotation;
		EyeRotation = Rotation;
		Position += Velocity * Time.Delta;
	}
}
