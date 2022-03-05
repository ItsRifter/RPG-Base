using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class Crowbar : MeleeBase
{
	public override string WeaponName => "Crowbar";
	public override string Description => "Whack creatures like the one true freeman";
	public override string WorldModel => "models/crowbar/crowbar.vmdl";
	public override float PrimaryRate => 1.5f;
	public override float SecondaryRate => 0.3f;
	public override bool IsMelee => true;
	public override int HoldType => 1;
	public override int Bucket => 1;
	public override int BaseDamage => 25;
	public override int MeleeDistance => 95;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModel );
	}
}
