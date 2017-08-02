using System.Collections.Generic;
using Entitas;

public sealed class InventoryComponent : IComponent
{
	public Dictionary<InventorySlot, InventoryItem> Items = new Dictionary<InventorySlot, InventoryItem>();
}

public sealed class ItemComponent : IComponent
{
	public IItem Item;
}