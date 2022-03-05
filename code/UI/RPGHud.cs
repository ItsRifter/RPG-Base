using Sandbox;
using Sandbox.UI;
public partial class RPGHud : Sandbox.HudEntity<RootPanel>
{
	public RPGHud()
	{
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<NameTags>();

		//TEMPORARY
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();

		//RPG Huds
		RootPanel.AddChild<PlayerStats>();
		RootPanel.AddChild<InventoryMenu>();
		RootPanel.AddChild<ShopMenu>();
	}
}
