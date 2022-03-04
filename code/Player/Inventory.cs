using System;
using System.Linq;
using Sandbox;

partial class Inventory : BaseInventory
{
	public Inventory( PlayerBase player ) : base( player )
	{

	}

	public override bool Add( Entity entity, bool makeActive = false )
	{
		var player = Owner as PlayerBase;
		var weapon = entity as RPGWeaponBase;

		if ( weapon != null && IsCarryingType( entity.GetType() ) )
		{
			var ammo = weapon.AmmoClip;
			var ammoType = weapon.AmmoType;

			if ( ammo > 0 )
			{
				player.GiveAmmo( ammoType, ammo );
			}

			entity.Delete();

			return false;
		}

		return base.Add( entity, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x.GetType() == t );
	}
}
