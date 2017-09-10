namespace Assets.Sources.Helpers.Loot
{
	using Items;

	public class LootDatabase : AbstractDatabase<LootGroupName, LootGroup>
	{
		public ItemName? GetLoot(LootGroupName groupName, int seed)
		{
			var group = GetItem(groupName);
			return group.GetLoot(seed);
		} 
	}
}
