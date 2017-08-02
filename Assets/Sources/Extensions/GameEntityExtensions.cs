public static class GameEntityExtensions
{
	public static InventoryItem GetInventoryItem(this GameEntity entity, InventorySlot slot)
	{
		if (!entity.hasInventory)
		{
			return null;
		}

		var items = entity.inventory.Items;
		InventoryItem item;
		items.TryGetValue(slot, out item);

		return item;
	}

	public static Weapon GetWeapon(this GameEntity entity)
	{
		var item = entity.GetInventoryItem(InventorySlot.Weapon);
		return item != null ? (Weapon) item.Item : null;
	}
}