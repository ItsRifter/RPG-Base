using System;
using Sandbox;

public partial class WeaponsShop : BaseNPC
{
	public override string NPCName => "Mark - General Shop";
	public override int BaseHealth => 1;
	public override float BaseSpeed => 0;
	public override string BaseModel => "models/citizen/citizen.vmdl";
	public override bool IsFriendly => true;
	public override ShopType Shoptype => ShopType.Weapons;

	public override void Spawn()
	{
		base.Spawn();
	}
}
