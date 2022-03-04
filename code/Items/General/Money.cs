using System;
using Sandbox;

public partial class Money : ItemBase
{
	public override string ItemName => "Gold";
	public override string ItemDescription => "Gold to keep you going";
	public override string ItemModel => "models/citizen_props/coin01.vmdl";
	public override int ItemAmount { get; set; }
	public override int ItemStackAmount { get; set; }

	public override void Spawn()
	{
		base.Spawn();
	}

	public override void OnUse( PlayerBase picker )
	{
		picker.AddMoney(ItemAmount);

		base.OnUse(picker);
	}
}
