namespace Assets.Sources.Helpers.Items
{
	using System;

	public class ItemDatabase : AbstractDatabase<ItemName, IItem>
	{
		public void RegisterItem(IItem item)
		{
			Items.Add(item.Name, item);
		}

		public override void RegisterItem(ItemName key, IItem value)
		{
			throw new NotImplementedException("Different overload should be used when adding items");
		}
	}
}