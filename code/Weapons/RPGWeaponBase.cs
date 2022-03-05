using System;
using Sandbox;
partial class RPGWeaponBase : BaseWeapon
{
	public virtual string WeaponName => "Test weapon";
	public virtual string Description => "A basic test weapon";
	public virtual string WorldModel => "weapons/rust_pistol/rust_pistol.vmdl";
	public virtual AmmoType AmmoType => AmmoType.Pistol;
	public virtual int ClipSize => 16;
	public virtual float ReloadTime => 3.0f;
	public virtual bool IsMelee => false;
	public virtual int Bucket => 1;
	public virtual int BucketWeight => 100;
	public virtual bool UnlimitedAmmo => false;
	public virtual int BaseDamage => 10;
	public virtual int HoldType => 1;

	[Net, Predicted]
	public int AmmoClip { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceReload { get; set; }

	[Net, Predicted]
	public bool IsReloading { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceDeployed { get; set; }

	public int AvailableAmmo()
	{
		if ( Owner is not PlayerBase owner ) return 0;
		return owner.AmmoCount( AmmoType );
	}

	public override void ActiveStart( Entity owner )
	{
		base.ActiveStart( owner );

		TimeSinceDeployed = 0;
	}

	public override void Spawn()
	{
		base.Spawn();

		if( !IsMelee )
			AmmoClip = ClipSize;

		SetModel( WorldModel );
	}

	public override void Reload()
	{
		if ( IsMelee || IsReloading )
			return;

		if ( AmmoClip >= ClipSize )
			return;

		TimeSinceReload = 0;

		if ( Owner is PlayerBase player )
		{
			if ( !UnlimitedAmmo )
			{
				if ( player.AmmoCount( AmmoType ) <= 0 )
					return;
			}
		}

		IsReloading = true;

		(Owner as AnimEntity).SetAnimParameter( "b_reload", true );

	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack();
	}

	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack();
	}

	public virtual void OnReloadFinish()
	{
		IsReloading = false;

		if ( Owner is PlayerBase player )
		{
			if ( !UnlimitedAmmo )
			{
				var ammo = player.TakeAmmo( AmmoType, ClipSize - AmmoClip );

				if ( ammo == 0 )
					return;

				AmmoClip += ammo;
			}
			else
			{
				AmmoClip = ClipSize;
			}
		}
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		ShootEffects();
		ShootBullet( 0.05f, 1.5f, BaseDamage, 3.0f );
	}

	[ClientRpc]
	protected virtual void ShootEffects()
	{
		Host.AssertClient();

		if ( !IsMelee )
		{
			Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
		}

		CrosshairPanel?.CreateEvent( "fire" );
	}

	public virtual void ShootBullet( float spread, float force, float damage, float bulletSize )
	{
		var forward = Owner.EyeRotation.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 5000, bulletSize ) )
		{
			tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100 * force, damage )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}
	}

	public bool TakeAmmo( int amount )
	{
		if ( AmmoClip < amount )
			return false;

		AmmoClip -= amount;
		return true;
	}

	public bool IsUsable()
	{
		if ( IsMelee || ClipSize == 0 || AmmoClip > 0 )
		{
			return true;
		}

		return AvailableAmmo() > 0;
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", HoldType );
		anim.SetAnimParameter( "aim_body_weight", 1.0f );
	}
}
