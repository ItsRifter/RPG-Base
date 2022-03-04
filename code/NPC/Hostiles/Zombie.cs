using Sandbox;
using System;
using System.Linq;

public partial class Zombie : BaseNPC
{
	public override string NPCName => "Zombie";
	public override int BaseHealth => 25;
	public override float BaseSpeed => 5;
	public override string BaseModel => "models/citizen/citizen.vmdl";
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
