namespace Assets.Sources.Features.Player
{
	using Entitas;
	using Entitas.CodeGeneration.Attributes;
	using Helpers.Networking;

	public class PlayerComponent : IComponent
	{
		public int Id;
	}

	[Unique]
	public class CurrentPlayer : IComponent
	{
		public EntityReference Entity;
		public Player Player;
	}
}