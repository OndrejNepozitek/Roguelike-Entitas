namespace Assets.Sources.Features.Items.Actions
{
	using Features.Actions;
	using Helpers.Items;
	using ProtoBuf;

	[ProtoContract]
	public class EquipAction : IAction
	{
		[ProtoMember(1)]
		public ItemName Item;

		[ProtoMember(2)]
		public EntityReference Entity;

		public bool Validate(GameContext context)
		{
			return true;
		}
	}
}