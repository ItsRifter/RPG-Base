using System;
using System.Collections.Generic;
using Sandbox;

public partial class ItemBase : AnimEntity
{
	public virtual string ItemName => "Item";
	public virtual string ItemDescription => "Typical Item base for an RPG";
	public virtual string ItemModel => "models/citizen/citizen.vmdl";
	public virtual int ItemAmount { get; set; }
	public virtual int ItemStackAmount { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( ItemModel );

		CollisionGroup = CollisionGroup.Player;
		SetInteractsAs( CollisionLayer.Debris );

		EnableHitboxes = true;
		PhysicsEnabled = true;

		if (Model == null )
		{
			Log.Error( Name + " has an invalid model or doesn't exist" );
			return;
		}

		if ( ItemAmount <= -1 )
			Delete();

		PhysicsEnabled = true;
		UsePhysicsCollision = true;

		SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
	}

	public virtual void OnUse(PlayerBase picker)
	{
		Delete();
	}
}

