namespace Assets.Sources.Features.Loot.Actions
{
	using System.Linq;
	using Features.Actions;
	using Helpers.Map;
	using ProtoBuf;

	[ProtoContract]
	public class OpenChestAction : IAction
	{
		[ProtoMember(1)]
		public EntityReference Player;

		[ProtoMember(2)]
		public EntityReference Chest;

		public bool Validate(GameContext context)
		{
			return true;
		}
	}
}
