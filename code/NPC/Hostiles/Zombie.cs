using Sandbox;
using System;
using System.Linq;

public partial class Zombie : BaseNPC
{
	public override string NPCName => "Zombie";
	public override int BaseHealth => 25;
	public override float BaseSpeed => 25;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override float AlertRadius => 92;
	public override float AttackCooldown => 5;
	public override float AttackDMG => 1;
	public override string AlertSound => "zombie_alert";
	public override int minRndLevel => 3;
	public override int maxRndLevel => 7;
	public override int minXP => 5;
	public override int maxXP => 25;

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		PlaySound( "zombie_pain" );
	}

	public override void Spawn()
	{
		base.Spawn();

		ItemToSpawn.Add( (new Money(), Rand.Int( 1, 5 ), 100) );
		


		RenderColor = Color.Green;
	}

	public override void OnKilled()
	{
		base.OnKilled();
	}
}
