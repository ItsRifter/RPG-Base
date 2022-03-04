using System;
using Sandbox;

partial class TestMelee : RPGWeaponBase
{
	public override string WeaponName => "Test Melee";
	public override string Description => "Stolen from Hidden/Rust, don't @ me";
	public override string WorldModel => "models/rust_boneknife/rust_boneknife.vmdl";
	public override float PrimaryRate => 1.0f;
	public override float SecondaryRate => 0.3f;
	public override bool IsMelee => true;
	public override int HoldType => 1;
	public override int Bucket => 1;
	public override int BaseDamage => 35;
	public virtual int MeleeDistance => 80;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModel );
	}

	public virtual void MeleeStrike( float damage, float force )
	{
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * MeleeDistance, 10f ) )
		{
			if ( !tr.Entity.IsValid() ) continue;

			tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;

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

	public override void AttackPrimary()
	{
		ShootEffects();
		PlaySound( "rust_boneknife.attack" );
		MeleeStrike( BaseDamage, 1.5f );
	}
}
