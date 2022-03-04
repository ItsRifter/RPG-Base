using System;
using Sandbox;
public partial class GeneralShop : BaseNPC
{
	public override string NPCName => "Johnny - General Shop";
	public override int BaseHealth => 1;
	public override float BaseSpeed => 0;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override bool IsFriendly => true;

	public override void Spawn()
	{
		base.Spawn();
	}
}
