using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

partial class PlayerBase
{
	[Net] public int CurrentXP { get; private set; }
	[Net] public int ReqXP { get; private set; }
	[Net] public int Level { get; private set; }
	[Net] public int Money { get; private set; }

	public void NewStats()
	{
		CurrentXP = 0;
		ReqXP = 50;
		Level = 0;
		Money = 0;
	}

	public void AddXP(int XP)
	{
		Log.Info( Client.Name + " gained " + XP + " XP");

		CurrentXP += XP;

		if(CurrentXP >= ReqXP)
		{
			while( CurrentXP >= ReqXP )
			{
				CurrentXP -= ReqXP;
				ReqXP *= 2;
				Level++;

				Log.Info( Client.Name + " leveled up to " + Level );
				Log.Info( "Required XP is now " + ReqXP);
			}

			PlaySound( "levelup" );
		}
	}

	public void AddMoney(int addMoney)
	{
		Log.Info( Client.Name + " received " + addMoney + " gold" );

		Money += addMoney;
	}

	public void HasEnoughMoney()
	{

	}

	public void TakeMoney(int takeMoney)
	{
		Log.Info( Client.Name + " spent (or lost) " + takeMoney + " gold" );

		Money -= takeMoney;
	}
}

